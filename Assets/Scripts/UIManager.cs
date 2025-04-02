using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas HUDCanvas;
    [SerializeField] private TextMeshProUGUI horseSelectedText;
    [SerializeField] private Canvas StartCanvas;
    [SerializeField] private Canvas HorseSelectionCanvas;
    
    private int _horseSelection;
    private event Action<int> OnHorseSelected;  

    public void HorseSelection(int horseIndex)
    {
        _horseSelection = horseIndex;
        OnHorseSelected?.Invoke(horseIndex);
        horseSelectedText.text = $"Your choice\nHorse #{horseIndex}";
    }
}
