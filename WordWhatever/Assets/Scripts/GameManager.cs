using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("[== REFERENCES ==]")]
    [SerializeField] private WordCheck wordCheck;
    
    [Header("[== TEXT OBJ REFERENCES ==]")]
    [SerializeField] private TMP_Text startingWord;
    [SerializeField] private TMP_Text endingWord;

    private string startWord;
    private string endWord;
    private List<char> chosenLetters;
    private List<string> submittedWords;

    public string StartWord => startWord;
    public string EndWord => endWord;
    public List<char> ChosenLetters => chosenLetters;
    public List<string> SubmittedWords => submittedWords;

    void Start()
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

        // then, generate startWord based off endWord
        startWord = wordCheck.GetRandomWord();
        chosenLetters = wordCheck.GetChosenLetters(2, startWord);
        Debug.Log($"CHOSEN LETTERS: {string.Join(" ", chosenLetters).ToUpper()}");

        UpdateStartWord(startWord.ToUpper());
        UpdateEndWord(endWord.ToUpper());
    }

    public void UpdateStartWord(string newWord)
        => startingWord.text = newWord;

    public void UpdateEndWord(string newWord)
        => endingWord.text = newWord;

    void Update()
    {
        
    }
}