using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    private TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index = 0;
    private bool playerIsClose;

    public float wordSpeed = 0.06f;

    private Coroutine typingCoroutine;

    void Start()
    {
        dialogueText = dialoguePanel.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        dialogueText.text = "";
        dialoguePanel.SetActive(false);
    }
    
    public void Talk()
    {
        if (!dialoguePanel.activeInHierarchy)
        {
            dialoguePanel.SetActive(true);
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
            RemoveText();
        }
    }
}
