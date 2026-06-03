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
    [SerializeField] private Color completedColor = Color.green;
    [SerializeField] private Color originalColor = Color.white;
    [SerializeField] private Color incorrectColor = Color.red;

    // original words
    private string startWord;
    private string endWord;
    // words with markup syntaxing
    private string startWordMarkup;
    private string endWordMarkup;
    private List<char> chosenLetters;
    private List<char> randEndLetters = new List<char>();
    private List<string> submittedWords = new List<string>();

    public Color OriginalColor => originalColor;
    public Color IncorrectColor => incorrectColor;
    public string StartWord => startWord;
    public string EndWord => endWord;
    public List<char> RandEndLetters => randEndLetters;
    public List<char> ChosenLetters => chosenLetters;
    public List<string> SubmittedWords => submittedWords;

    // has to be Start instead of Awake to prevent race conditions against WordCheck
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

        // then, generate startWord using a random letter from endWord
        randEndLetters.Add(char.ToLower(endWord[Random.Range(0, endWord.Length)]));
        startWord = wordCheck.GetRandomWordContainingLetter(randEndLetters.Last());
        chosenLetters = wordCheck.GetInitChosenVowels(2, startWord);
        startWordMarkup = wordCheck.ReturnWithColor(startWord, chosenLetters, highlightColor);
        endWordMarkup = wordCheck.ReturnWithColor(endWord, randEndLetters, highlightColor);
        Debug.Log($"CHOSEN LETTERS: {string.Join(" ", chosenLetters).ToUpper()}");
        Debug.Log($"CHOSEN END LETTER: {char.ToUpper(randEndLetters.Last())}");

        UpdateStartWord(startWordMarkup.ToUpper());
        UpdateEndWord(endWordMarkup.ToUpper());
    }

    public void UpdateState()
    {
        startWord = submittedWords.Last();
        List<char> prevLetters = new List<char>(chosenLetters);
        char endLetter;
        while (true)
        {
            chosenLetters.Clear();
            chosenLetters = wordCheck.GetChosenVowels(2, startWord);
            endLetter = wordCheck.GetRandomLetterExclusive(endWord, randEndLetters);
            List<char> allLetters = new List<char>(chosenLetters) {endLetter};
            if (!chosenLetters.Equals(prevLetters) && wordCheck.IsPossible(allLetters))
                break;
        }
        randEndLetters.Add(endLetter);
        startWordMarkup = wordCheck.ReturnWithColor(startWord, chosenLetters, highlightColor);
        endWordMarkup = wordCheck.ReturnWithColorEnd(endWord, randEndLetters, highlightColor, completedColor);
        Debug.Log($"CHOSEN LETTERS: {string.Join(" ", chosenLetters).ToUpper()}");
        Debug.Log($"CHOSEN END LETTER: {char.ToUpper(randEndLetters.Last())}");
        UpdateStartWord(startWordMarkup.ToUpper());
        UpdateEndWord(endWordMarkup.ToUpper());
    }

    public void UpdateStartWord(string newWord)
        => startingWord.text = newWord;

    public void UpdateEndWord(string newWord)
        => endingWord.text = newWord;
}