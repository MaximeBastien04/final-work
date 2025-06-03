using System.Collections;
using UnityEngine;

public class EndGameScript : MonoBehaviour
{
    private DialogueManager dialogueManager;
    void Start()
    {
        dialogueManager = GetComponent<DialogueManager>();
        StartCoroutine(WaitForEndDialogue(4.5f));

        Destroy(GameObject.Find("Global Volume"));
        Destroy(GameObject.Find("StoryManager"));
        Destroy(GameObject.Find("BackgroundColor"));
        Destroy(GameObject.Find("InteractionManager"));
        Destroy(GameObject.Find("AudioManager"));
        Destroy(GameObject.Find("SceneTransitionManager"));
        Destroy(GameObject.Find("InputManager"));
    }
    
    private IEnumerator WaitForEndDialogue(float duration)
    {
        yield return new WaitForSeconds(duration);
        dialogueManager.Talk();
    }
}
