using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReadKeyboardInput : MonoBehaviour
{
    [Header("[== REFERENCES ==]")]
    [SerializeField] private TMP_Text canvasText;
    [SerializeField] public wordCheck wordChecker;

    [Header("[== SETTINGS ==]")]
    [SerializeField] private float initialDelay = 0.5f;
    [SerializeField] private float repeatDelay = 0.025f;

    private InputActions inputActions;
    private bool submitted = false;
    private bool backspaceHeld;
    private float nextDeleteTime;

    private void Awake() => inputActions = new InputActions();

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
        if (!backspaceHeld || submitted) return;
        if (Time.time >= nextDeleteTime)
        {
            DeleteCharacter();
            nextDeleteTime = Time.time + repeatDelay;
        }
    }

    private void DeleteCharacter()
    {
        if (canvasText.text.Length > 0) canvasText.text = canvasText.text[..^1];
    }

    private void OnTextInput(char c)
    {
        if (submitted) return;
        if (c == '\b' || c == '\r' || c == '\n') return;
        if (!char.IsLetter(c)) return; // we don't care about spaces, only letters!
        canvasText.text += char.ToUpper(c);
    }

    private void OnEnter(InputAction.CallbackContext context)
    {
        submitted = true;
        if (wordChecker.IsValidWord(canvasText.text))
        {
            canvasText.color = Color.green;
        }
        else
        {
            canvasText.color = Color.black;
        }
    }
}