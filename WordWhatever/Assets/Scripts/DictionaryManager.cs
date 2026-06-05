using UnityEngine;
using System.Collections.Generic;

public class DictionaryManager : MonoBehaviour
{
    public static DictionaryManager Instance;
    public HashSet<string> ValidWords { get; private set; }

    [SerializeField] private TextAsset vocabularyList;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadDictionary();
    }

    private void LoadDictionary()
    {
        ValidWords = new HashSet<string>();
        string[] lines = vocabularyList.text.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines) ValidWords.Add(line.Trim().ToLower());
    }
}