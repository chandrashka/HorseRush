using UnityEngine;

public class HorseController : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private Animator animator;

    [Header("Speed Settings")]
    [SerializeField] private float minSpeed = 15f;
    [SerializeField] private float maxSpeed = 30f;
    [SerializeField] private float variationAmount = 0.2f;
    [SerializeField] private float variationSpeed = 0.5f;

    [SerializeField] private float rotationSpeed = 5f;

    private float _baseSpeed;
    private float _speedOffset;
    private int _currentIndex;
    private bool _isRunning;
    private Vector3 _startPosition;
    
    public bool IsFinished { get; private set; }

    private void Start()
    {
        _startPosition = transform.position;
        animator.Play("Idle");
    }

    private void Update()
    {
        if (!_isRunning || IsFinished || waypoints.Length == 0) return;

        var target = waypoints[_currentIndex];
        var direction = (target.position - transform.position).normalized;

        var lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        var speed = _baseSpeed + Mathf.Sin(Time.time * variationSpeed + _speedOffset) * variationAmount;
        var step = speed * Time.deltaTime;
        transform.position += transform.forward * step;

        if (Vector3.Distance(transform.position, target.position) < 1f)
        {
            _currentIndex++;

            if (_currentIndex >= waypoints.Length)
            {
                _isRunning = false;
                IsFinished = true;
                animator.Play("Idle");
            }
        }
    }

    public void StartRunning()
    {
        if (waypoints.Length == 0) return;

        _isRunning = true;
        IsFinished = false;
        _currentIndex = 0;

        _baseSpeed = Random.Range(minSpeed, maxSpeed);
        _speedOffset = Random.Range(0f, Mathf.PI * 2f);

        animator.Play("Run");
    }

    public void ResetHorse()
    {
        _isRunning = false;
        IsFinished = false;
        _currentIndex = 0;
        transform.position = _startPosition;
        transform.rotation = Quaternion.LookRotation(waypoints[1].position - waypoints[0].position);
        animator.Play("Idle");
    }
    
    public float DistanceAlongTrack
    {
        get
        {
            var distance = 0f;

            if (_currentIndex == 0)
            {
                distance += Vector3.Distance(_startPosition, transform.position);
                return distance;
            }
            
            distance += Vector3.Distance(_startPosition, waypoints[0].position);
            
            for (var i = 1; i < _currentIndex; i++)
            {
                distance += Vector3.Distance(waypoints[i - 1].position, waypoints[i].position);
            }
            
            if (_currentIndex < waypoints.Length)
            {
                var from = waypoints[_currentIndex - 1].position;
                var to = waypoints[_currentIndex].position;
                var pos = transform.position;

                var segmentLength = Vector3.Distance(from, to);
                var projected = Vector3.Project(pos - from, to - from).magnitude;

                distance += Mathf.Clamp(projected, 0, segmentLength);
            }

            return distance;
        }
    }
}
