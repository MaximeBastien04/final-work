using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    private TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index = 0;
    private bool playerIsClose;
    private bool dialogueFinished = false;

    public System.Action OnFinalLineStarted;



    public float wordSpeed = 0.06f;

    private Coroutine typingCoroutine;

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
            if (Input.GetKeyDown(KeyCode.E))
            {
                Talk();

            }

            if (dialogueFinished)
            {
                StartCoroutine(SceneTransitionManager.Instance.LoadSceneAfterDelay(5f, "Appartment"));
            }
        }
    }

    public void Talk()
    {
        if (!dialoguePanel.activeInHierarchy)
        {
            dialoguePanel.SetActive(true);

            // If finished, replay only the last sentence
            if (dialogueFinished && dialogue.Length > 0)
            {
                index = dialogue.Length - 1;
            }

            typingCoroutine = StartCoroutine(Typing());
        }
        else if (dialogueText.text == dialogue[index])
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
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        if (index < dialogue.Length - 1)
        {
            index++;
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            typingCoroutine = StartCoroutine(Typing());

            if (index == dialogue.Length - 1) // Just started last line
            {
                OnFinalLineStarted?.Invoke(); // 🔥 Trigger the tag update
            }
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

    public void ReplaceText(string[] newText)
    {
        dialogue = newText;
        dialogueFinished = false;
        index = 0;
    }
}
