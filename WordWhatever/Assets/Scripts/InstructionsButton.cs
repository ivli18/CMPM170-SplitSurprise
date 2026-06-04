using UnityEngine;

public class InstructionsButton : MonoBehaviour
{
    public GameObject panel;
    public void ShowPanel()
    {
        panel.SetActive(true);
    }
    public void HidePanel()
    {
        panel.SetActive(false);
    }
}