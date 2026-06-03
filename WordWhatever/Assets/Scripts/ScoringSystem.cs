using System.Linq;
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
        bool validAnswer,
            exists = wordChecker.IsValidWord(text),
            inOrder = wordChecker.ContainsLettersInOrder(text, gameManager.ChosenLetters),
            containsEndLetter = text.ToLower().Contains(gameManager.RandEndLetters.Last()),
            isMinLength = text.Length >= minSubmitLength,
            notRepeated = !gameManager.SubmittedWords.Contains(text);
        if (validAnswer = exists && inOrder && containsEndLetter && isMinLength && notRepeated)
        {
            score++;
            UpdateScoreText();
        }
        Debug.Log(
            $"DOES IT PASS? EXISTS: {exists} ORDERED: {inOrder} CONTAINS END LETTER: {containsEndLetter} MIN LENGTH: {isMinLength} NOT REPEATED: {notRepeated}"
        );
        return validAnswer;
    }

    private void UpdateScoreText()
        => scoreText.text = $"SCORE: {score}";
}