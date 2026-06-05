using UnityEngine;
using TMPro;

public class GlobalVolume : MonoBehaviour
{
    private static float[] volumeSteps = { 0f, 0.25f, 0.5f, 0.75f, 1f };
    private int volumeIndex = 4;
    [SerializeField] private TMP_Text volumeLabel;

    public void CycleVolume()
    {
        volumeIndex = (volumeIndex + 1) % volumeSteps.Length;
        AudioListener.volume = volumeSteps[volumeIndex];
        volumeLabel.text = GetFormattedVolume();
    }
    public string GetFormattedVolume()
    {
        return $"Sound: {(int)(AudioListener.volume * 100)}%";
    }
}
