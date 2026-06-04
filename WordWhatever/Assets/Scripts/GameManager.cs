using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;

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
    private List<string> submittedWords = new List<string>();
    private List<EndLetter> randEndLetters = new List<EndLetter>();

    public Color OriginalColor => originalColor;
    public Color IncorrectColor => incorrectColor;
    public string StartWord => startWord;
    public string EndWord => endWord;
    public List<char> ChosenLetters => chosenLetters;
    public List<string> SubmittedWords => submittedWords;
    public List<EndLetter> RandEndLetters => randEndLetters;

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
        int index = Random.Range(0, endWord.Length);
        randEndLetters.Add(new EndLetter { letter = endWord[index], index = index });
        /*foreach (EndLetter eL in randEndLetters) Debug.Log(eL);*/
        startWord = wordCheck.GetRandomWordContainingLetter(randEndLetters.Last().letter);
        chosenLetters = wordCheck.GetInitChosenVowels(2, startWord);
        startWordMarkup = wordCheck.ReturnWithColor(startWord, chosenLetters, highlightColor);
        endWordMarkup = wordCheck.ReturnWithColor(endWord, EndLetter.ToCharList(randEndLetters), highlightColor);
        Debug.Log($"CHOSEN LETTERS: {string.Join(" ", chosenLetters).ToUpper()}");
        Debug.Log($"CHOSEN END LETTER: {char.ToUpper(randEndLetters.Last().letter)}");

        UpdateStartWord(startWordMarkup.ToUpper());
        UpdateEndWord(endWordMarkup.ToUpper());
    }

    public void UpdateState()
    {
        startWord = submittedWords.Last();
        List<char> prevLetters = new List<char>(chosenLetters);
        EndLetter endLetter = default;
        bool valid = false;
        while (!valid)
        {
            chosenLetters.Clear();
            chosenLetters = wordCheck.GetChosenVowels(2, startWord);
            endLetter = wordCheck.GetRandomLetterExclusive(endWord, randEndLetters);
            for (int i = 0; i < chosenLetters.Count && !valid; i++)
            {
                List<char> allLetters = new List<char>(chosenLetters);
                allLetters.Insert(i, endLetter.letter);
                // check if combination has not been used previously and is possible
                if (wordCheck.IsPossible(allLetters) && !chosenLetters.All(prevLetters.Contains))
                    valid = true;
            }
        }
        randEndLetters.Add(endLetter);
        /*for (int j = 0; j < randEndLetters.Count; j++) Debug.Log($"{j}...{randEndLetters[j]}");*/
        startWordMarkup = wordCheck.ReturnWithColor(startWord, chosenLetters, highlightColor);
        endWordMarkup = wordCheck.ReturnWithColorEnd(endWord, randEndLetters, highlightColor, completedColor);
        Debug.Log($"CHOSEN LETTERS: {string.Join(" ", chosenLetters).ToUpper()}");
        Debug.Log($"CHOSEN END LETTER: {char.ToUpper(randEndLetters.Last().letter)}");
        UpdateStartWord(startWordMarkup.ToUpper());
        UpdateEndWord(endWordMarkup.ToUpper());
    }

    public void UpdateStartWord(string newWord)
        => startingWord.text = newWord;

    public void UpdateEndWord(string newWord)
        => endingWord.text = newWord;
    
    public struct EndLetter
    {
        public char letter { get; set; }
        public int index { get; set; }

        public static List<char> ToCharList(List<EndLetter> list)
            => list.Select(x => x.letter).ToList();

        public static List<int> ToIndexList(List<EndLetter> list)
            => list.Select(x => x.index).ToList();

        public override string ToString()
            => $"({letter}: {index})";
    }
}