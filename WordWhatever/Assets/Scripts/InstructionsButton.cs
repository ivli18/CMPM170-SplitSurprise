using UnityEngine;

public class InstructionsButton : MonoBehaviour
{
    public GameObject panel;
    [SerializeField] private TimerSystem timerSystem;
    public void ShowPanel()
    {
        if (timerSystem != null) timerSystem.paused = false;
        panel.SetActive(true);
    }
    public void HidePanel()
    {
        panel.SetActive(false);
        if (timerSystem != null) timerSystem.paused = true;
    }
}