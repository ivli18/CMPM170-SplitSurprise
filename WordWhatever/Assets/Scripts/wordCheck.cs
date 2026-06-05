using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WordCheck : MonoBehaviour
{
    [Header("[== REFERENCES ==]")]
    [SerializeField] private GameManager gameManager;

    private HashSet<string> validWords = new HashSet<string>();

    private void Awake()
    {
        validWords = DictionaryManager.Instance.ValidWords;
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

    public bool ContainsLettersInOrder(string word, List<char> letters)
    {
        int letterIndex = 0;
        foreach (char c in letters)
        {
            /*Debug.Log($"{c} == {letters[letterIndex]}: {c == letterIndex}");*/
            int found = word.ToLower().IndexOf(char.ToLower(c), letterIndex);
            if (found == -1) {
                return false;
            }
            letterIndex = found + 1;
        }
        return true;
    }

    /*
    public bool ContainsLetterAtPosition(int pos, char letter, string word)
        => char.ToLower(letter) == char.ToLower(word[pos]);
    */

    public List<string> GetWordsStartingWith(string prefix)
    {
        List<string> matches = new List<string>();
        foreach (string word in validWords)
        {
            if (word.ToLower().StartsWith(prefix.ToLower()))
                matches.Add(word);
        }
        return matches;
    }

    public string GetRandomWord(int minLength = 4, int minVowels = 2)
    {
        string vowels = "aeiou";
        List<string> pool = new List<string>();
        foreach (string word in validWords)
        {
            if (word.Length >= minLength && word.Count(c => vowels.Contains(char.ToLower(c))) >= minVowels)
                pool.Add(word);
        }
        if (pool.Count == 0) { return null; }
        return pool[Random.Range(0, pool.Count)];
    }

    public string GetRandomWordContainingLetter(char letter, int minLength = 4, int minVowels = 2)
    {
        string vowels = "aeiou";
        List<string> pool = new List<string>();
        foreach (string word in validWords)
        {
            if (word.Length >= minLength && word.Count(c => vowels.Contains(char.ToLower(c))) >= minVowels && word.ToLower().Contains(char.ToLower(letter)))
                pool.Add(word);
        }
        if (pool.Count == 0) { return null; }
        return pool[Random.Range(0, pool.Count)];
    }
    /*
    public string GetRandomWordStartingWith(string starting, int minLength = 4)
    {
        List<string> pool = new List<string>();
        foreach (string word in validWords)
        {
            if (word.Length >= minLength && word.ToLower().StartsWith(starting.ToLower()))
                pool.Add(word);
        }
        if (pool.Count == 0) return null;
        return pool[Random.Range(0, pool.Count)];
    }
    */

    /*
    public int ChooseLetterNumber(string word) => defaultLetterCount + (word.Length / 4);
    */

    /*
    public List<char> GetChosenLetters(int letterCount, string word)
    {
        // thank you to this stackoverflow post for the algorithm :)
        // https://stackoverflow.com/questions/1450774/splitting-a-string-into-chunks-of-a-certain-size
        IEnumerable<string> splitWord
            = Enumerable.Range(0, letterCount)
                .Select(i => word.Substring(i * letterCount, letterCount));
        List<char> possibleLetters = new List<char>();
        while (true)
        {
            possibleLetters.Clear();
            foreach (string section in splitWord)
                possibleLetters.Add(
                    section[Random.Range(0, section.Length)]
                );
            if (IsPossible(possibleLetters))
                return new List<char>(possibleLetters);
        }
    }
    */

    public List<char> GetInitChosenVowels(int vowelCount, string word)
    {
        List<char> possibleVowels = new List<char>();
        while (true)
        {
            possibleVowels.Clear();
            possibleVowels = GetChosenVowels(vowelCount, word);
            if (IsPossible(possibleVowels))
                return possibleVowels;
        }
    }

    public List<char> GetChosenVowels(int vowelCount, string word)
    {
        string vowels = "aeiou";
        List<int> possibleInd = new List<int>();
        while (possibleInd.Count < vowelCount)
        {
            int index = Random.Range(0, word.Length);
            if (!possibleInd.Contains(index) && vowels.Contains(char.ToLower(word[index])))
                possibleInd.Add(index);
        }
        // place in ascending index order
        possibleInd.Sort();
        List<char> possibleLetters = possibleInd
            .Select(i => word[i])
            .ToList();
        return possibleLetters;
    }
    public int GetEndRandomLetter(string word, List<int> completedIndexes)
    {
        List<int> remaining = new List<int>();
        for (int i = 0; i < word.Length; i++)
        {
            if (!completedIndexes.Contains(i))
            {
                remaining.Add(i);
            }
        }
        if (remaining.Count == 0) return -1;
        int chosen = remaining[Random.Range(0, remaining.Count)];
        return chosen;
    }

    public bool IsValidWord(string guess)
        => validWords.Contains(guess.ToLower());

    public string ReturnWithColor(string word, List<char> letters)
    {
        string result = "";
        int letterIndex = 0;
        foreach (char c in word)
        {
            if (letterIndex < letters.Count && char.ToLower(c) == char.ToLower(letters[letterIndex]))
            {
                string hexText = ColorUtility.ToHtmlStringRGB(gameManager.HighlightColor);
                result += $"<color=#{hexText}>{c}</color>";
                letterIndex++;
            }
            else
            {
                result += c;
            }
        }
        return result;
    }
    public string ReturnEndWithColor(string word, List<int> completedIndexes, int currentIndex)
    {
        string result = "";
        for (int i = 0; i < word.Length; i++)
        {
            if (completedIndexes.Contains(i))
            {
                string hex = ColorUtility.ToHtmlStringRGB(gameManager.CompleteColor);
                result += $"<color=#{hex}>{word[i]}</color>";
            }else if (i == currentIndex)
            {
                string hex = ColorUtility.ToHtmlStringRGB(gameManager.HighlightColor);
                result += $"<color=#{hex}>{word[i]}</color>";
            } else
            {
                result += word[i];
            }
        }
        return result;
    }
}