using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerSystem : MonoBehaviour
{
    [Header("[== REFERENCES ==]")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private Image timerImageLeft;
    [SerializeField] private Image timerImageRight;

    [Header("[== SETTINGS ==]")]
    [SerializeField] private Color startColor = Color.white;
    [SerializeField] private Color endColor = Color.red;
    [SerializeField] private float totalTime = 30f;

    private float elapsed = 0f;
    private bool running = true;

    public bool Running => running;

    private void Start()
    {
        timerImageLeft.type = timerImageRight.type = Image.Type.Filled;
        timerImageLeft.fillMethod = timerImageRight.fillMethod = Image.FillMethod.Horizontal;
    }

    private void Update()
    {
        if (!running) return;
        
        UpdateUI();
        elapsed += Time.deltaTime;
        if (elapsed >= totalTime)
            running = false;
    }

    private void UpdateUI()
    {
        float timerProgress = 1 - Mathf.Clamp01(elapsed / totalTime);
        timerImageLeft.fillAmount = timerImageRight.fillAmount = timerProgress;
        timerText.text = GetFormattedTime();

        // convert rgb (red, green, blue) values
        // into hsv (hue, saturation, value) values
        Color.RGBToHSV(
            startColor,
            out float startHue,
            out float startSaturation,
            out float startLight
        );
        Color.RGBToHSV(
            endColor,
            out float endHue,
            out float endSaturation,
            out float endLight
        );

        // calculate new lerp values
        float
            hue = Mathf.LerpAngle(
                endHue,
                startHue,
                timerProgress
            ),
            saturation = Mathf.Lerp(
                endSaturation,
                startSaturation,
                timerProgress
            ),
            light = Mathf.Lerp(
                endLight,
                startLight,
                timerProgress
            );
            
        timerImageLeft.color = timerImageRight.color = timerText.color = Color.HSVToRGB(
            hue,
            saturation,
            light
        );
    }

    public string GetFormattedTime()
    {
        float remaining = Mathf.Max(0f, totalTime - elapsed);
        int minutes = Mathf.FloorToInt(remaining / 60f);
        int seconds = Mathf.FloorToInt(remaining % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
    public void AddTime(float addTime)
        => elapsed = Mathf.Max(0f, elapsed - addTime);
}