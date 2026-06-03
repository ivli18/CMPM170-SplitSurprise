using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [Header("[== REFERENCES ==]")]
    [SerializeField] private WordCheck wordCheck;
    
    [Header("[== TEXT OBJ REFERENCES ==]")]
    [SerializeField] private TMP_Text startingWord;
    [SerializeField] private TMP_Text endingWord;

    [Header("[== SETTINGS ==]")]
    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField] private Color originalColor = Color.white;
    [SerializeField] private Color incorrectColor = Color.red;

    private string startWord;
    private string endWord;
    private List<char> chosenLetters;
    private List<string> submittedWords = new List<string>();

    public Color HighlightColor => highlightColor;
    public Color OriginalColor => originalColor;
    public Color IncorrectColor => incorrectColor;
    public string StartWord => startWord;
    public string EndWord => endWord;
    public List<char> ChosenLetters => chosenLetters;
    public List<string> SubmittedWords => submittedWords;

    private void Start()
    {
        // start with generating endWord
        while (true)
        {
            endWord = wordCheck.GetRandomWord();
            List<string> matches = wordCheck.GetWordsStartingWith(endWord);
            if (matches.Count >= 5)
            {
                Debug.Log($"WORDS FOR {endWord}: {string.Join(", ", matches)}");
                break;
            }
        }

        // then, generate startWord
        startWord = wordCheck.GetRandomWord();
        chosenLetters = wordCheck.GetInitChosenVowels(2, startWord);
        startWord = wordCheck.ReturnWithColor(startWord, chosenLetters);
        Debug.Log($"CHOSEN LETTERS: {string.Join(" ", chosenLetters).ToUpper()}");

        UpdateStartWord(startWord.ToUpper());
        UpdateEndWord(endWord.ToUpper());
    }

    public void UpdateState()
    {
        startWord = submittedWords.Last();
        List<char> prevLetters = new List<char>(chosenLetters);
        while (true)
        {
            chosenLetters.Clear();
            chosenLetters = wordCheck.GetChosenVowels(2, startWord);
            if (!chosenLetters.Equals(prevLetters))
                break;
        }
        startWord = wordCheck.ReturnWithColor(startWord, chosenLetters);
        Debug.Log($"CHOSEN LETTERS: {string.Join(" ", chosenLetters).ToUpper()}");
        UpdateStartWord(startWord.ToUpper());
    }

    public void UpdateStartWord(string newWord)
        => startingWord.text = newWord;

    public void UpdateEndWord(string newWord)
        => endingWord.text = newWord;
}