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
    [SerializeField] private Color completeColor = Color.green;
    [SerializeField] private Color originalColor = Color.white;
    [SerializeField] private Color incorrectColor = Color.red;

    private string startWord;
    private string endWord;
    private char randEndLetter;
    private List<char> chosenLetters;
    private int endLetterIndex;
    private List<int> completedLettersIndex;
    private List<string> submittedWords = new List<string>();

    public Color HighlightColor => highlightColor;
    public Color CompleteColor => completeColor;
    public Color OriginalColor => originalColor;
    public Color IncorrectColor => incorrectColor;
    public string StartWord => startWord;
    public string EndWord => endWord;
    public List<char> ChosenLetters => chosenLetters;
    public List<int> CompletedLettersIndex => completedLettersIndex;
    public List<string> SubmittedWords => submittedWords;
    public int EndLetterIndex 
    { 
        get => endLetterIndex; 
        private set => endLetterIndex = value; 
    }

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
        completedLettersIndex = new List<int>();
        endLetterIndex = wordCheck.GetEndRandomLetter(endWord, completedLettersIndex);

        // then, generate startWord using a random letter from endWord
        randEndLetter = endWord[Random.Range(0, endWord.Length)];
        startWord = wordCheck.GetRandomWordContainingLetter(randEndLetter);
        chosenLetters = wordCheck.GetInitChosenVowels(2, startWord);
        startWord = wordCheck.ReturnWithColor(startWord, chosenLetters);
        Debug.Log($"CHOSEN LETTERS: {string.Join(" ", chosenLetters).ToUpper()}");

        UpdateStartWord(startWord);
        UpdateEndWord();
    }
    public void UpdateState()
    {
        startWord = submittedWords.Last();
        List<char> prevLetters = new List<char>(chosenLetters);
        while (true)
        {
            chosenLetters.Clear();
            chosenLetters = wordCheck.GetChosenVowels(2, startWord);
            randEndLetter = endWord[Random.Range(0, endWord.Length)];
            // chosenLetters.Add(randEndLetter);
            if (!chosenLetters.Equals(prevLetters) && wordCheck.IsPossible(prevLetters))
                break;
        }
        startWord = wordCheck.ReturnWithColor(startWord, chosenLetters);
        Debug.Log($"CHOSEN LETTERS: {string.Join(" ", chosenLetters).ToUpper()}");
        UpdateStartWord(startWord);
    }


    public void CompleteEndLetter(int index)
    {
        completedLettersIndex.Add(index);
        if (completedLettersIndex.Count == endWord.Length)
        {
            CycleWords();
            return;
        } else {
            endLetterIndex = wordCheck.GetEndRandomLetter(endWord, completedLettersIndex);
            UpdateEndWord();
            AudioManager.Instance.PlaySFX(AudioManager.SFXType.ValidSFX);
        }
    }
    private void CycleWords()
    {
        startWord = endWord;
        chosenLetters = wordCheck.GetInitChosenVowels(2, startWord);
        startWord = wordCheck.ReturnWithColor(startWord, chosenLetters);
        UpdateStartWord(startWord);

        while (true)
        {
            endWord = wordCheck.GetRandomWord();
            List<string> matches = wordCheck.GetWordsStartingWith(endWord);
            if (matches.Count >= 5) break;
        }
        completedLettersIndex = new List<int>();
        endLetterIndex = wordCheck.GetEndRandomLetter(endWord, completedLettersIndex);
        AudioManager.Instance.PlaySFX(AudioManager.SFXType.SuccessSFX);
        UpdateEndWord();
    }
    public void UpdateStartWord(string newWord)
        => startingWord.text = newWord.ToUpper();

    public void UpdateEndWord()
        => endingWord.text = wordCheck.ReturnEndWithColor(endWord, completedLettersIndex, endLetterIndex).ToUpper();
}