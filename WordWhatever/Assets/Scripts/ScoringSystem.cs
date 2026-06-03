using System.Linq;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    [Header("[== REFERENCES ==]")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private WordCheck wordChecker;

    [Header("[== SETTINGS ==]")]
    // setting to 4 for testing purposes
    [SerializeField] private int minSubmitLength = 4;

    private int score = 0;

    public int Score => score;

    public bool CalculateScore(string text)
    {
        bool exists = wordChecker.IsValidWord(text),
            inOrder = wordChecker.ContainsLettersInOrder(text, gameManager.ChosenLetters),
            isMinLength = text.Length >= minSubmitLength,
            notRepeated = !gameManager.SubmittedWords.Contains(text);
        bool validAnswer
            = exists && inOrder && isMinLength && notRepeated;
        if (validAnswer)
        {
            score++;
            UpdateScoreText();
        }
        Debug.Log(
            $"DOES IT PASS? EXISTS: {exists}\nORDERED: {inOrder}\n MIN LENGTH: {isMinLength}\n NOT REPEATED: {notRepeated}"
        );
        return validAnswer;
    }

    private void UpdateScoreText() => scoreText.text = $"SCORE: {score}";
}