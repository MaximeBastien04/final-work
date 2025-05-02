using Unity.VisualScripting;
using UnityEngine;

public class InteractiveItem : MonoBehaviour
{
    private GameObject interactButton;

    void Start()
    {
        interactButton = gameObject.transform.Find("InteractButton").GameObject();
        interactButton.SetActive(false);
    }


    // GameObject has to have a 2D collider that is set to trigger and have child named InteractButton.
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered trigger with: " + other.name);
        if (other.name == "Player")
        {
        interactButton.SetActive(true);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player")
        {
        interactButton.SetActive(false);
        }
    }
}
