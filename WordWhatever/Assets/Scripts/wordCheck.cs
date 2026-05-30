using UnityEngine;
using System.Collections.Generic;

public class wordCheck : MonoBehaviour
{
    private HashSet<string> validWords = new HashSet<string>();
    void Start()
    {
        LoadDictionary();
    }
    void LoadDictionary()
    {
        TextAsset dictFile = Resources.Load<TextAsset>("dictionary");
        string[] lines = dictFile.text.Split('\n');

        foreach (string line in lines)
        {
            string word = line.Trim().ToLower();
            if (!string.IsNullOrEmpty(word))
            {
                validWords.Add(word);
            }
        }
    }

    public bool IsValidWord(string guess)
    {
        return validWords.Contains(guess.ToLower());
    }
}
