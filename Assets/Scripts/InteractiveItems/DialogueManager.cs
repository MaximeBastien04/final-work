using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages dialogue display, typing effect, player interaction, and choice UI in the game.
/// </summary>
public class DialogueManager : MonoBehaviour
{

    /// <summary>
    /// Struct representing a line of dialogue and how long to pause after it.
    /// </summary>
    [System.Serializable]
    public struct DialogueLine
    {
        [TextArea]
        public string text;
        public float pauseAfter;
    }

    public GameObject dialoguePanel;
    private TextMeshProUGUI dialogueText;
    public DialogueLine[] dialogue;
    private int index = 0;
    public bool playerIsClose;
    private bool dialogueFinished = false;

    public System.Action OnFinalLineStarted;
    public float wordSpeed = 0.06f;

    private Coroutine typingCoroutine;
    private bool isWaiting = false;
    PlayerControls controls;

    [Header("Choice Panel")]
    public GameObject choicePanel;
    public Button yesButton;
    public Button noButton;

    private System.Action onYes;
    private System.Action onNo;

    [Header("End Game")]
    public GameObject gameTitle;

    public CutsceneManager cutsceneManager;

    void Awake()
    {
        controls = new PlayerControls();
    }

    void OnEnable() => controls.Gameplay.Enable();
    void OnDisable() => controls.Gameplay.Disable();

    void Start()
    {
        dialogueText = dialoguePanel.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        dialogueText.text = "";
        dialoguePanel.SetActive(false);
        if (SceneManager.GetActiveScene().name == "HomeScene")
        {
            cutsceneManager = GameObject.Find("WindowInnerFrame").GetComponent<CutsceneManager>();
        }
    }

    /// <summary>
    /// Checks for interaction input and handles dialogue progression in the "HomeScene".
    /// </summary>
    void Update()
    {
        // Debug.Log(cutsceneManager.windowIsClosed);
        if (SceneManager.GetActiveScene().name == "HomeScene" && cutsceneManager.windowIsClosed)
        {
            controls.Gameplay.Interact.performed += ctx => OnInteract();
            if (dialogueFinished)
            {
                StartCoroutine(SceneTransitionManager.Instance.LoadSceneAfterDelay(5f, "Appartment"));
            }
        }
        else if (SceneManager.GetActiveScene().name == "EndGame")
        {
            controls.Gameplay.Interact.performed += ctx => OnInteract();
            if (dialogueFinished)
            {
                gameTitle.SetActive(true);
                StartCoroutine(QuitGameAfterTitle(8f));
            }
        }
    }

    /// <summary>
    /// Called when the player interacts; starts or advances dialogue.
    /// </summary>
    private void OnInteract()
    {
        if (!isWaiting)
        {
            Talk();
        }
    }

    /// <summary>
    /// Starts or advances dialogue typing. Shows dialogue panel if not active.
    /// </summary>
    public void Talk()
    {
        if (isWaiting) return;

        if (!dialoguePanel.activeInHierarchy)
        {
            dialoguePanel.SetActive(true);

            if (dialogueFinished && dialogue.Length > 0)
            {
                index = dialogue.Length - 1;
            }

            typingCoroutine = StartCoroutine(Typing());
        }
        else if (typingCoroutine == null && dialogueText.text == dialogue[index].text)
        {
            NextLine();
        }
    }

    /// <summary>
    /// Stops current typing coroutine, clears dialogue, hides panel, and resets state.
    /// </summary>
    public void RemoveText()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        dialogueFinished = true;
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    /// <summary>
    /// Coroutine that types out the current dialogue line letter-by-letter with pauses.
    /// Invokes OnFinalLineStarted event if the last line starts.
    /// </summary>
    IEnumerator Typing()
    {
        dialogueText.text = "";
        DialogueLine currentLine = dialogue[index];
        foreach (char letter in currentLine.text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }

        typingCoroutine = null;

        if (index == dialogue.Length - 1)
        {
            OnFinalLineStarted?.Invoke();
        }

        if (currentLine.pauseAfter > 0f)
        {
            StartCoroutine(PauseThenNextLine(currentLine.pauseAfter));
        }
    }

    /// <summary>
    /// Coroutine that pauses for a duration, temporarily hides and shows dialogue panel, then advances dialogue.
    /// </summary>
    /// <param name="duration">Pause duration in seconds.</param>
    IEnumerator PauseThenNextLine(float duration)
    {
        isWaiting = true;
        yield return new WaitForSeconds(2f);
        dialoguePanel.SetActive(false);
        yield return new WaitForSeconds(duration);
        dialoguePanel.SetActive(true);
        isWaiting = false;
        NextLine();
    }

    /// <summary>
    /// Advances to the next dialogue line or ends dialogue if finished.
    /// </summary>
    public void NextLine()
    {
        if (isWaiting) return;

        if (index < dialogue.Length - 1)
        {
            index++;
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            typingCoroutine = StartCoroutine(Typing());
        }
        else
        {
            RemoveText();
        }
    }

    /// <summary>
    /// Detects when the player enters the dialogue trigger area.
    /// </summary>
    /// <param name="other">The collider entering the trigger.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    /// <summary>
    /// Detects when the player exits the dialogue trigger area and stops dialogue.
    /// </summary>
    /// <param name="other">The collider exiting the trigger.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            dialogueText.text = "";
            dialoguePanel.SetActive(false);
        }
    }

    /// <summary>
    /// Replaces the current dialogue lines with new lines and resets dialogue state.
    /// </summary>
    /// <param name="newText">New array of dialogue lines.</param>
    public void ReplaceText(DialogueLine[] newText)
    {
        dialogue = newText;
        dialogueFinished = false;
        index = 0;
    }

    /// <summary>
    /// Displays a yes/no choice panel with provided callback actions.
    /// </summary>
    /// <param name="yesAction">Action to invoke when yes is chosen.</param>
    /// <param name="noAction">Action to invoke when no is chosen.</param>
    public void ShowChoice(System.Action yesAction, System.Action noAction)
    {
        TextMeshProUGUI choiceText = choicePanel.transform.Find("ChoiceText").GetComponent<TextMeshProUGUI>();
        if (index < dialogue.Length)
        {
            choiceText.text = dialogue[index].text;
        }

        onYes = yesAction;
        onNo = noAction;

        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(() =>
        {
            choicePanel.SetActive(false);
            onYes?.Invoke();
        });

        noButton.onClick.AddListener(() =>
        {
            choicePanel.SetActive(false);
            onNo?.Invoke();
        });

        choicePanel.SetActive(true);
    }

    /// <summary>
    /// Displays a multiple choice panel with a question and arbitrary number of choices.
    /// </summary>
    /// <param name="question">The question text to display.</param>
    /// <param name="choices">Array of tuples containing label and action for each choice.</param>
    public void ShowMultiChoice(string question,
    (string label, System.Action action)[] choices)
    {
        TextMeshProUGUI choiceText = choicePanel.transform.Find("ChoiceText").GetComponent<TextMeshProUGUI>();
        choiceText.text = question;

        Button[] allButtons = choicePanel.GetComponentsInChildren<Button>();
        foreach (Button btn in allButtons)
        {
            btn.gameObject.SetActive(false);
            btn.onClick.RemoveAllListeners();
        }
        for (int i = 0; i < choices.Length; i++)
        {
            string buttonName = $"{choices[i].label}Button";
            Transform buttonTransform = choicePanel.transform.Find(buttonName);
            if (buttonTransform != null)
            {
                Button btn = buttonTransform.GetComponent<Button>();
                TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
                btnText.text = choices[i].label;

                btn.gameObject.SetActive(true);

                int choiceIndex = i;
                btn.onClick.AddListener(() =>
                {
                    choicePanel.SetActive(false);
                    choices[choiceIndex].action?.Invoke();
                });
            }
            else
            {
                Debug.LogWarning($"Button {buttonName} not found in ChoicePanel.");
            }
        }
        choicePanel.SetActive(true);
    }


    private IEnumerator QuitGameAfterTitle(float duration)
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene("HomeScene");
    }
}