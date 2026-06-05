using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerSystem : MonoBehaviour
{
    [Header("[== REFERENCES ==]")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text popupText;
    [SerializeField] private Image timerImageLeft;
    [SerializeField] private Image timerImageRight;

    [Header("[== SETTINGS ==]")]
    [SerializeField] private float totalTime = 40f;

    private float elapsed = 0f;
    private bool running = true;
    public bool paused = false;

    public bool Running => running;

    private void Start()
    {
        timerImageLeft.type = timerImageRight.type = Image.Type.Filled;
        timerImageLeft.fillMethod = timerImageRight.fillMethod = Image.FillMethod.Horizontal;
    }

    private void Update()
    {
        if (!running || paused) return;
        
        UpdateUI();
        elapsed += Time.deltaTime;
        if (elapsed >= totalTime)
        {
            running = false;
        }
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
        StartCoroutine(PopupTime(addTime));
    }
    IEnumerator PopupTime(float amount)
    {
        popupText.fontSize = 36 + (amount / 5);
        popupText.text = $"+ <color=green>{amount}s</color>";
        popupText.alpha = 1f;
        yield return new WaitForSeconds(0.5f);
        float t = 0f;
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            popupText.alpha = 1f - (t / 0.3f);
            yield return null;
        }
        popupText.alpha = 0f;
    }
}