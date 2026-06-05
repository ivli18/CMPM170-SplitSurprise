using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;

public class ReadKeyboardInput : MonoBehaviour
{
    [Header("[== SYSTEM REFERENCES ==]")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ScoringSystem scoreSystem;
    [SerializeField] private TimerSystem timerSystem;   

    [Header("[== TEXT OBJ REFERENCES ==]")]
    [SerializeField] private TMP_Text guessText;

    [Header("[== SETTINGS ==]")]
    [SerializeField] private float initialDelay = 0.5f;
    [SerializeField] private float repeatDelay = 0.025f;


    private InputActions inputActions;
    private bool backspaceHeld;
    private float nextDeleteTime;

    private void Awake()
    {
        inputActions = new InputActions();
        guessText.text = "";
    }

    private void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.BackspaceKey.started += OnBackspaceStarted;
        inputActions.Player.BackspaceKey.canceled += OnBackspaceCancelled;
        inputActions.Player.EnterKey.performed += OnEnter;
        if (Keyboard.current != null)
            Keyboard.current.onTextInput += OnTextInput;
    }

    private void OnDisable()
    {
        inputActions.Player.BackspaceKey.started -= OnBackspaceStarted;
        inputActions.Player.BackspaceKey.canceled -= OnBackspaceCancelled;
        inputActions.Player.EnterKey.performed -= OnEnter;
        inputActions.Disable();
        if (Keyboard.current != null)
            Keyboard.current.onTextInput -= OnTextInput;
    }

    private void OnBackspaceStarted(InputAction.CallbackContext context)
    {
        if (timerSystem.paused) return;

        backspaceHeld = true;
        // delete a character immediately
        DeleteCharacter();
        nextDeleteTime = Time.time + initialDelay;
    }

    private void OnBackspaceCancelled(InputAction.CallbackContext context)
        => backspaceHeld = false;

    private void Update()
    {
        if (!timerSystem.Running) SceneManager.LoadScene("LossScene");
        if (!backspaceHeld) return;
        if (Time.time >= nextDeleteTime)
        {
            DeleteCharacter();
            nextDeleteTime = Time.time + repeatDelay;
        }
    }

    private void DeleteCharacter()
    {
        if (guessText.text.Length > 0) guessText.text = guessText.text[..^1];
        AudioManager.Instance.PlaySFX(AudioManager.SFXType.DeleteSFX);
    }

    private void OnTextInput(char c)
    {
        if (timerSystem.paused) return;

        if (c == '\b' || c == '\r' || c == '\n') return;
        if (!char.IsLetter(c)) return; // we don't care about spaces, only letters!
        guessText.text += char.ToUpper(c);
        AudioManager.Instance.PlaySFX(AudioManager.SFXType.TypeSFX);    
    }

    private void OnEnter(InputAction.CallbackContext context)
    {
        if (timerSystem.paused) return;

        string guess = guessText.text;
        switch(scoreSystem.CalculateScore(guess))
        {
            case true:
                AudioManager.Instance.PlaySFX(AudioManager.SFXType.ValidSFX);
                guessText.text = "";
                gameManager.SubmittedWords.Add(guess.ToUpper());
                gameManager.UpdateState();
                timerSystem.AddTime(5f);
                // Checks if selected end letter is in word
                if (guess.ToUpper().Contains(char.ToUpper(gameManager.EndWord[gameManager.EndLetterIndex])))
                {
                    gameManager.CompleteEndLetter(gameManager.EndLetterIndex);
                }
                break;
            case false:
                AudioManager.Instance.PlaySFX(AudioManager.SFXType.InvalidSFX);
                StartCoroutine(Invalid());
                break;
        }
    }
    IEnumerator Invalid()
    {
        guessText.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        guessText.color = gameManager.OriginalColor;
        yield return new WaitForSeconds(0.05f);
        guessText.color = gameManager.IncorrectColor;
        yield return new WaitForSeconds(0.15f);
        guessText.color = gameManager.OriginalColor;
        guessText.text = "";
    }
}