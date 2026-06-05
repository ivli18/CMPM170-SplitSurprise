using UnityEngine;
using TMPro;

public class FinalScore : MonoBehaviour
{
    private void Start()
    {
        GetComponent<TMP_Text>().text = $"SCORE: {ScoringSystem.FinalScore}";
    }
}