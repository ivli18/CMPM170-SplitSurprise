using TMPro;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private WordCheck wordChecker;

    private int score = 0;

    public int Score => score;

    public bool CalculateScore(string text)
    {
        bool valid = wordChecker.IsValidWord(text);
        score += valid ? 1 : 0;
        UpdateScoreText();
        return valid;
    }

    private void UpdateScoreText() => scoreText.text = $"SCORE: {score}";
}
