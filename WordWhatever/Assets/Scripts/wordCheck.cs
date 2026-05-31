using UnityEngine;
using System.Collections.Generic;
using System.Xml.Schema;

public class WordCheck : MonoBehaviour
{
    [Header("[== REFERENCES ==]")]
    [SerializeField] private TextAsset vocabularyList;

    private HashSet<string> validWords = new HashSet<string>();
    private void Awake() => LoadDictionary();

    private void LoadDictionary()
    {
        string[] lines = vocabularyList.text.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines) validWords.Add(line.Trim().ToLower());
    }

    public bool IsPossible(List<char> letters)
    {
        foreach (string word in validWords)
        {
            if (word.Length < letters.Count) continue; // skip words that are smaller than possible letters
            if (ContainsLettersInOrder(word, letters)) return true;
        }
        return false;
    }

    private bool ContainsLettersInOrder(string word, List<char> letters)
    {
        int letterIndex = 0;
        foreach (char c in word)
        {
            if (c == letters[letterIndex])
            {
                letterIndex++;
                if (letterIndex == letters.Count) return true;
            }
        }
        return false;
    }
    public string GetRandomWord(int minLength = 4)
    {
        List<string> pool = new List<string>();
        foreach (string word in validWords)
        {
            if (word.Length >= minLength) pool.Add(word);
        }
        if (pool.Count == 0) { return null; }
        return pool[Random.Range(0, pool.Count)];
    }

    public bool IsValidWord(string guess) => validWords.Contains(guess.ToLower());
}