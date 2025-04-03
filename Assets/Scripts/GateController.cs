using UnityEngine;

public class GateController : MonoBehaviour
{
    private static readonly int Open = Animator.StringToHash("Open");

    [SerializeField] private Animator[] gateAnimators;

    private void OpenGate()
    {
        foreach (var animator in gateAnimators)
        {
            animator.SetTrigger(Open);
        }
    }

    public void ResetGate()
    {
        foreach (var animator in gateAnimators)
        {
            animator.Play("Idle", 0, 0f);
        }
    }

    public void StartRace()
    {
        OpenGate();
    }
}