using UnityEngine;
using TMPro;

public class GlobalVolume : MonoBehaviour
{
    private static float[] volumeSteps = { 0f, 0.25f, 0.5f, 0.75f, 1f };
    private int volumeIndex = 2;
    [SerializeField] private TMP_Text volumeLabel;

    private void Start()
    {
        AudioManager.Instance.SetMasterVolume(volumeSteps[volumeIndex]);
        volumeLabel.text = GetFormattedVolume();
    }
    public void CycleVolume()
    {
        volumeIndex = (volumeIndex + 1) % volumeSteps.Length;
        AudioManager.Instance.SetMasterVolume(volumeSteps[volumeIndex]);
        volumeLabel.text = GetFormattedVolume();
    }
    public string GetFormattedVolume()
    {
        return $"Sound: {(int)(AudioListener.volume * 100)}%";
    }
}
