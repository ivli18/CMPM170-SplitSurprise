using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReadKeyboardInput : MonoBehaviour
{
    [Header("[== REFERENCES ==]")]
    [SerializeField] private TMP_Text canvasText;
    [SerializeField] private TMP_Text previousWordText;
    [SerializeField] private ScoringSystem scoreSystem;


    [Header("[== SETTINGS ==]")]
    [SerializeField] private float initialDelay = 0.5f;
    [SerializeField] private float repeatDelay = 0.025f;

    private InputActions inputActions;
    private bool backspaceHeld;
    private float nextDeleteTime;

    private void Awake()
    {
        inputActions = new InputActions();
        canvasText.text = "";
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.BackspaceKey.started += OnBackspaceStarted;
        inputActions.Player.BackspaceKey.canceled += OnBackspaceCancelled;
        inputActions.Player.EnterKey.performed += OnEnter;
        if (Keyboard.current != null) Keyboard.current.onTextInput += OnTextInput;
    }

    private void OnDisable()
    {
        inputActions.Player.BackspaceKey.started -= OnBackspaceStarted;
        inputActions.Player.BackspaceKey.canceled -= OnBackspaceCancelled;
        inputActions.Player.EnterKey.performed -= OnEnter;
        inputActions.Disable();
        if (Keyboard.current != null) Keyboard.current.onTextInput -= OnTextInput;
    }

    private void OnBackspaceStarted(InputAction.CallbackContext context)
    {
        backspaceHeld = true;
        // delete a character immediately
        DeleteCharacter();
        nextDeleteTime = Time.time + initialDelay;
    }

    private void OnBackspaceCancelled(InputAction.CallbackContext context) => backspaceHeld = false;

    private void Update()
    {
        if (!backspaceHeld) return;
        if (Time.time >= nextDeleteTime)
        {
            DeleteCharacter();
            nextDeleteTime = Time.time + repeatDelay;
        }
    }

    private void DeleteCharacter()
    {
        if (canvasText.text.Length > 0) canvasText.text = canvasText.text[..^1];
        AudioManager.Instance.PlaySFX(AudioManager.SFXType.DeleteSFX);
    }

    private void OnTextInput(char c)
    {
        if (c == '\b' || c == '\r' || c == '\n') return;
        if (!char.IsLetter(c)) return; // we don't care about spaces, only letters!
        canvasText.text += char.ToUpper(c);
        AudioManager.Instance.PlaySFX(AudioManager.SFXType.TypeSFX);    
    }

    private void OnEnter(InputAction.CallbackContext context)
    {
        switch(scoreSystem.CalculateScore(canvasText.text))
        {
            case true:
                previousWordText.text = canvasText.text;
                canvasText.text = "";
                AudioManager.Instance.PlaySFX(AudioManager.SFXType.ValidSFX);
                break;
            case false:
                canvasText.color = Color.red;
                AudioManager.Instance.PlaySFX(AudioManager.SFXType.InvalidSFX);
                break;
        }
    }
}