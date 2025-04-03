using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GateController gateController;
    [SerializeField] private HorseController[] horseControllers;
    [SerializeField] private CameraManager cameraManager;

    private readonly List<int> _lastTop3 = new();
    private readonly List<int> _finishedOrder = new();
    private bool _resultShown;

    private void OnEnable()
    {
        uiManager.Reset();
        uiManager.OnHorseSelected += StartGame;
    }

    private void OnDisable()
    {
        uiManager.OnHorseSelected -= StartGame;
    }

    private void StartGame(int horseIndex)
    {
        cameraManager.selectedHorse = horseIndex - 1;
        uiManager.StartRace();
        gateController.StartRace();

        foreach (var controller in horseControllers)
        {
            controller.StartRunning();
        }

        _finishedOrder.Clear();
        _resultShown = false;
        _lastTop3.Clear();
    }

    private void Update()
    {
        if (horseControllers == null || horseControllers.Length < 3) return;
        
        var distanceList = new List<(int index, float distance)>();
        for (var i = 0; i < horseControllers.Length; i++)
        {
            distanceList.Add((i + 1, horseControllers[i].DistanceAlongTrack));
        }

        var newTop3 = distanceList
            .OrderByDescending(h => h.distance)
            .Take(3)
            .Select(h => h.index)
            .ToList();

        uiManager.UpdateTop3(newTop3);
        
        for (var i = 0; i < horseControllers.Length; i++)
        {
            if (horseControllers[i].IsFinished && !_finishedOrder.Contains(i))
            {
                _finishedOrder.Add(i);
            }
        }
        
        var selectedIndex = cameraManager.selectedHorse;
        if (!_resultShown && _finishedOrder.Contains(selectedIndex))
        {
            var place = _finishedOrder.IndexOf(selectedIndex) + 1;
            uiManager.ShowPlayerResult(place);
            _resultShown = true;
            
            gateController.ResetGate();
            foreach (var horseController in horseControllers)
            {
                horseController.ResetHorse();
            }
        }
    }
}