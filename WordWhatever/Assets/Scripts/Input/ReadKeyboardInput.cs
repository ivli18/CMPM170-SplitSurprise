using TMPro;
using UnityEngine;

public class ReadKeyboardInput : MonoBehaviour
{
    [SerializeField] private TMP_Text canvasText;

    private bool submitted = false;

    private void Update()
    {
        if (submitted) return;
        UpdateText(); // borrowed from https://docs.unity3d.com/ScriptReference/Input-inputString.html
    }

    private void UpdateText()
    {
        foreach (char c in Input.inputString)
        {
            // if character is new line or return carriage
            if (c == '\n' || c == '\r')
            {
                canvasText.color = Color.green;
                submitted = true;
            }
            else
            {
                Debug.Log(canvasText.text.Length > 0);
                canvasText.text += char.ToUpper(c);
            }
        }
    }
}
