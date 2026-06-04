using UnityEngine;

public class InstructionsButton : MonoBehaviour
{
    public GameObject panel;
    [SerializeField] private TimerSystem timerSystem;
    public void ShowPanel()
    {
        timerSystem.running = false;
        panel.SetActive(true);
    }
    public void HidePanel()
    {
        panel.SetActive(false);
        timerSystem.running = true;
    }
}