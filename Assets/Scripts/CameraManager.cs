using UnityEngine;
using System.Linq;

public class CameraManager : MonoBehaviour
{
    public Camera raceCam;
    public HorseController[] horses;
    
    public float sideOffset = 10f;
    public float frontOffset = 15f;
    public float backOffset = 12f;
    public float height = 6f;
    public float topHeight = 20f;
    public float moveSmooth = 2f;
    public float lookSmooth = 5f;
    public float switchInterval = 6f;

    private float _switchTimer;
    private Vector3 _velocity;

    private HorseController _currentTarget;
    private ViewType _currentView;
    public int selectedHorse;

    private enum ViewType
    {
        SideLeft,
        SideRight,
        FrontDrone,
        TopFar,
        DiagonalBack
    }

    private void Start()
    {
        _switchTimer = switchInterval;
        PickTargetAndView();
    }

    private void Update()
    {
        if (horses == null || horses.Length < 3) return;

        _switchTimer -= Time.deltaTime;
        if (_switchTimer <= 0f)
        {
            PickTargetAndView();
            _switchTimer = switchInterval;
        }

        if (_currentTarget == null) return;

        var leaderPos = _currentTarget.transform.position;
        var forward = _currentTarget.transform.forward;
        var right = _currentTarget.transform.right;

        var targetPos = raceCam.transform.position;

        switch (_currentView)
        {
            case ViewType.SideLeft:
                targetPos = leaderPos - right * sideOffset + Vector3.up * height;
                break;
            case ViewType.SideRight:
                targetPos = leaderPos + right * sideOffset + Vector3.up * height;
                break;
            case ViewType.FrontDrone:
                targetPos = leaderPos - forward * frontOffset + right * 2f + Vector3.up * (height + 2f);
                break;
            case ViewType.TopFar:
                targetPos = leaderPos + Vector3.up * topHeight;
                break;
            case ViewType.DiagonalBack:
                targetPos = leaderPos - forward * backOffset + right * 4f + Vector3.up * (height + 3f);
                break;
        }

        raceCam.transform.position = Vector3.SmoothDamp(raceCam.transform.position, targetPos, ref _velocity, 1f / moveSmooth);

        var lookTarget = leaderPos + Vector3.up * 1.5f;
        var lookRot = Quaternion.LookRotation(lookTarget - raceCam.transform.position);
        raceCam.transform.rotation = Quaternion.Slerp(raceCam.transform.rotation, lookRot, Time.deltaTime * lookSmooth);
    }

    private void PickTargetAndView()
    {
        var top3 = horses
            .OrderByDescending(h => h.DistanceAlongTrack)
            .Take(3)
            .ToList();

        if (top3.Count == 0) return;
        
        var rand = Random.value;
        _currentTarget = rand < 0.7f ? top3[0] : horses[selectedHorse];
        
        var possibleViews = System.Enum.GetValues(typeof(ViewType))
            .Cast<ViewType>()
            .Where(v => v != _currentView)
            .ToArray();

        _currentView = possibleViews[Random.Range(0, possibleViews.Length)];
    }
}