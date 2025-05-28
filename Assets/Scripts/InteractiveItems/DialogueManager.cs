using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
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
    private bool playerIsClose;
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

    void Awake()
    {
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void Start()
    {
        dialogueText = dialoguePanel.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        dialogueText.text = "";
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "HomeScene")
        {
            controls.Gameplay.Interact.performed += ctx => OnInteract();
            if (dialogueFinished)
            {
                StartCoroutine(SceneTransitionManager.Instance.LoadSceneAfterDelay(5f, "Appartment"));
            }
        }
    }

    private void OnInteract()
    {
        if (!isWaiting)
        {
            Talk();
        }
    }

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

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

    public void ReplaceText(DialogueLine[] newText)
    {
        dialogue = newText;
        dialogueFinished = false;
        index = 0;
    }

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
}