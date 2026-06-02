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
    [SerializeField] private int minSubmitLength = 5;

    private int score = 0;

    public int Score => score;

    public bool CalculateScore(string text)
    {
        bool exists = wordChecker.IsValidWord(text),
            inOrder = wordChecker.ContainsLettersInOrder(text, gameManager.ChosenLetters),
            containsBeginningLetter = wordChecker.ContainsLetterAtPosition(score, text[0], gameManager.EndWord),
            isMinLength = text.Length >= minSubmitLength,
            notRepeated = !gameManager.SubmittedWords.Contains(text);
        bool validAnswer
            = exists && inOrder && containsBeginningLetter && isMinLength && notRepeated;
        if (validAnswer)
        {
            score++;
            UpdateScoreText();
        }
        Debug.Log(
            $"DOES IT PASS? EXISTS: {exists} ORDERED: {inOrder} CONTAINS BEGINNING: {containsBeginningLetter} MIN LENGTH: {isMinLength} NOT REPEATED: {notRepeated}"
        );
        return validAnswer;
    }

    private void UpdateScoreText() => scoreText.text = $"SCORE: {score}";
}