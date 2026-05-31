using UnityEngine;
using System.Collections.Generic;

public class WordCheck : MonoBehaviour
{
    [Header("[== REFERENCES ==]")]
    [SerializeField] private TextAsset vocabularyList;

    private HashSet<string> validWords = new HashSet<string>();
    private void Start() => LoadDictionary();

    private void LoadDictionary()
    {
        string[] lines = vocabularyList.text.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines) validWords.Add(line.Trim().ToLower());
    }

    public bool IsValidWord(string guess) => validWords.Contains(guess.ToLower());
}