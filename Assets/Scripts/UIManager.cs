using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    [FormerlySerializedAs("HUDCanvas")] [SerializeField] private Canvas hudCanvas;
    [SerializeField] private TextMeshProUGUI horseSelectedText;
    [SerializeField] private TextMeshProUGUI top3Text;
    [SerializeField] private Canvas startCanvas;
    [SerializeField] private Canvas horseSelectionCanvas;
    [SerializeField] private Canvas endCanvas;
    [SerializeField] private TextMeshProUGUI resultsText;
    public event Action<int> OnHorseSelected;  

    public void HorseSelection(int horseIndex)
    {
        OnHorseSelected?.Invoke(horseIndex);
        horseSelectedText.text = $"Your choice\nHorse #{horseIndex}";
    }

    public void OnStartClicked()
    {
        startCanvas.enabled = false;
        horseSelectionCanvas.enabled = true;
        hudCanvas.enabled = false;
        endCanvas.enabled = false;
    }

    public void Reset()
    {
        startCanvas.enabled = true;
        horseSelectionCanvas.enabled = false;
        hudCanvas.enabled = false;
        endCanvas.enabled = false;
    }


    public void StartRace()
    {
        startCanvas.enabled = false;
        horseSelectionCanvas.enabled = false;
        hudCanvas.enabled = true;
        endCanvas.enabled = false;
    }

    public void UpdateTop3(List<int> newTop3)
    {
        top3Text.text = $"Top 3\n1. Horse #{newTop3[0]}\n2. Horse #{newTop3[1]}\n3. Horse #{newTop3[2]}";
    }
    
    public void ShowPlayerResult(int place)
    {
        startCanvas.enabled = false;
        horseSelectionCanvas.enabled = false;
        hudCanvas.enabled = false;
        endCanvas.enabled = true;

        resultsText.text = place == 1 ? "You Win!\n Your horse is 1!" : $"You Lose!\n Your horse is in {place} place!";
    }

}
