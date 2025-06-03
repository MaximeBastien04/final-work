using System.Collections;
using UnityEngine;

public class EndGameScript : MonoBehaviour
{
    private DialogueManager dialogueManager;
    void Start()
    {
        Debug.Log("Start");
        dialogueManager = GetComponent<DialogueManager>();
        StartCoroutine(WaitForEndDialogue(6f));
        GameObject.Destroy(GameObject.Find("Global Volume"));
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private IEnumerator WaitForEndDialogue(float duration)
    {
        yield return new WaitForSeconds(duration);
        Debug.Log("talk");
        dialogueManager.Talk();
    }
}
