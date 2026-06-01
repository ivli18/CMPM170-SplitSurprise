using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    [Header("[== REFERENCES ==]")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private WordCheck wordChecker;

    private int score = 0;

    public int Score => score;

    public bool CalculateScore(string text)
    {
        bool exists = wordChecker.IsValidWord(text);
        bool inOrder = wordChecker.ContainsLettersInOrder(text, gameManager.ChosenLetters);
        Debug.Log($"DOES IT PASS? EXISTS: {exists} ORDERED: {inOrder}");
        if (exists && inOrder)
        {
            score++;
            UpdateScoreText();
        }
        return exists && inOrder;
    }

    private void UpdateScoreText() => scoreText.text = $"SCORE: {score}";
}