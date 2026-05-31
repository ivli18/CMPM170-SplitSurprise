using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private WordCheck wordCheck;
    // textbox references
    [SerializeField] private TMP_Text startingWord;
    [SerializeField] private TMP_Text endingWord;

    void Start()
    {
        string startWord = wordCheck.GetRandomWord();
        string endWord = wordCheck.GetRandomWord();

        startingWord.text = startWord.ToUpper();
        endingWord.text = endWord.ToUpper();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
