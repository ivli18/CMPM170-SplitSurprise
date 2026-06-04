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
    [SerializeField] private float totalTime = 30f;

    private float elapsed = 0f;
    public bool running = true;


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
    }

    public string GetFormattedTime()
    {
        float remaining = Mathf.Max(0f, totalTime - elapsed);
        int minutes = Mathf.FloorToInt(remaining / 60f);
        int seconds = Mathf.FloorToInt(remaining % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
    public void AddTime(float addTime)
    {
        elapsed = Mathf.Max(0f, elapsed - addTime);
    }
}