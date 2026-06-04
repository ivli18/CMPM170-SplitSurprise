using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class WordCheck : MonoBehaviour
{
    [Header("[== REFERENCES ==]")]
    [SerializeField] private TextAsset vocabularyList;

    private HashSet<string> validWords = new HashSet<string>();

    private void Awake()
        => LoadDictionary();

    private void LoadDictionary()
    {
        string[] lines = vocabularyList.text.Split(
            '\n', System.StringSplitOptions.RemoveEmptyEntries
        );
        foreach (string line in lines)
            validWords.Add(line.Trim().ToLower());
    }

    public bool IsPossibleInit(List<char> letters)
    {
        foreach (string word in validWords)
        {
            if (word.Length < letters.Count) continue; // skip words that are smaller than possible letters
            if (ContainsLettersInOrder(word, letters)) return true;
        }
        return false;
    }

    public bool IsPossible(List<char> letters)
    {
        foreach (string word in validWords)
        {
            int letterCount = 0;
            if (word.Length < letters.Count) continue; // skip words that are smaller than possible letters
            foreach (char letter in letters)
                if (word.Contains(char.ToLower(letter)))
                    letterCount++;
            if (letterCount == letters.Count)
                return true;
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

    public GameManager.EndLetter GetRandomLetterExclusive(string word, List<GameManager.EndLetter> endLetters)
    {
        List<GameManager.EndLetter> possibleLetters = new List<GameManager.EndLetter>();
        for (int i = 0; i < word.Length; i++)
        {
            char letter = word[i];
            if (!GameManager.EndLetter.ToCharList(endLetters).Contains(char.ToLower(letter)) && !GameManager.EndLetter.ToIndexList(endLetters).Contains(i))
                possibleLetters.Add(new GameManager.EndLetter { letter = word[i], index = i });
        }
        return possibleLetters[UnityEngine.Random.Range(0, possibleLetters.Count)];
    }

    public string GetRandomWord(int minLength = 4)
    {
        List<string> pool = new List<string>();
        foreach (string word in validWords)
        {
            if (word.Length >= minLength)
                pool.Add(word);
        }
        if (pool.Count == 0) { return null; }
        return pool[UnityEngine.Random.Range(0, pool.Count)];
    }

    public string GetRandomWordContainingLetter(char letter, int minLength = 4)
    {
        List<string> pool = new List<string>();
        foreach (string word in validWords)
            if (word.Length >= minLength && word.ToLower().Contains(char.ToLower(letter)))
                pool.Add(word);
        if (pool.Count == 0) return null;
        return pool[UnityEngine.Random.Range(0, pool.Count)];
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
            if (IsPossibleInit(possibleVowels))
                return possibleVowels;
        }
    }

    public List<char> GetChosenVowels(int vowelCount, string word)
    {
        string vowels = "aeiou";
        List<int> possibleInd = new List<int>();
        while (possibleInd.Count < vowelCount)
        {
            int index = UnityEngine.Random.Range(0, word.Length);
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

    public bool IsValidWord(string guess)
        => validWords.Contains(guess.ToLower());

    public string ReturnWithColor(string word, List<char> letters, Color color)
    {
        string result = "";
        int letterIndex = 0;
        foreach (char c in word)
        {
            if (letterIndex < letters.Count && char.ToLower(c) == char.ToLower(letters[letterIndex]))
            {
                string hexText = ColorUtility.ToHtmlStringRGB(color);
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

    public string ReturnWithColorEnd(string word, List<GameManager.EndLetter> originalLetters, Color highlightColor, Color completedColor)
    {
        string result = "";

        // grab highlight letter and index
        List<GameManager.EndLetter> letters
            = new List<GameManager.EndLetter>(originalLetters);
        GameManager.EndLetter endLetter;
        endLetter = letters.Last();
        letters.RemoveAt(letters.Count - 1);
        
        for (int i = 0; i < word.Length; i++)
        {
            string hexText;
            char c = char.ToLower(word[i]);
            Debug.Log($"{endLetter}...{string.Join(" ", letters)}");
            if (
                (GameManager.EndLetter.ToCharList(letters).Contains(c) && GameManager.EndLetter.ToIndexList(letters).Contains(i))
                || (letters.Count == 0 && c == char.ToLower(endLetter.letter) && i == endLetter.index)
            )
            {
                hexText = ColorUtility.ToHtmlStringRGB(completedColor);
                result += $"<color=#{hexText}>{c}</color>";
            }
            else if (c == char.ToLower(endLetter.letter) && i == endLetter.index)
            {
                hexText = ColorUtility.ToHtmlStringRGB(highlightColor);
                result += $"<color=#{hexText}>{c}</color>";
            }
            else
            {
                result += c;
            }
        }
        return result;
    }
}