using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    [Header("[== REFERENCES ==]")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private WordCheck wordChecker;

    [Header("[== SETTINGS ==]")]
    [SerializeField] private int defaultLetterCount = 2;

    private int score = 0;
    private List<char> chosenLetters;

    public int Score => score;

    private void Start() => UpdateChosenLetters(defaultLetterCount);

    public bool CalculateScore(string text)
    {
        bool valid = wordChecker.IsValidWord(text);
        if (valid)
        {
            score++;
            UpdateScoreText();
            UpdateChosenLetters(ChooseLetterNumber(text));
        }
        return valid;
    }

    private int ChooseLetterNumber(string word) => defaultLetterCount + (word.Length / 4);

    private void UpdateChosenLetters(int letterCount)
    {
        string alphabet = "abcdefghijklmnopqrstuvwxyz";
        List<char> possibleLetters = new List<char>();
        while (true)
        {
            possibleLetters.Clear();
            for (int i = 0; i < letterCount; i++)
            {
                int randInd = Random.Range(0, alphabet.Length);
                possibleLetters.Add(alphabet[randInd]);
            }
            if (wordChecker.IsPossible(possibleLetters)) break;
        }
        chosenLetters = new List<char>(possibleLetters);
        Debug.Log($"CHOSEN LETTERS: {string.Join("", chosenLetters)}");
    }

    private void UpdateScoreText() => scoreText.text = $"SCORE: {score}";
}