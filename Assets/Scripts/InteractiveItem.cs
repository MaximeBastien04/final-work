using Unity.VisualScripting;
using UnityEngine;

public class InteractiveItem : MonoBehaviour
{
    private GameObject interactButton;
    private Animator playerAnimator;
    private bool inRange = false;

    void Start()
    {
        playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();

        interactButton = gameObject.transform.Find("InteractButton").GameObject();
        interactButton.SetActive(false);
    }

    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            // Glass drink animation
            if (gameObject.name == "Glass")
            {
                Debug.Log("Drink glass animation");
                // playerAnimator.SetTrigger("DrinkWater");
            }
            else if (gameObject.name == "AppartmentDoor")
            {
                Debug.Log("Go outside");
                // Play door sound
                // Load outside scene
            }
        }
    }

    // GameObject has to have a 2D collider that is set to trigger and have child named InteractButton.
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered trigger with: " + other.name);
        if (other.name == "Player")
        {
            interactButton.SetActive(true);
            inRange = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            interactButton.SetActive(false);
            inRange = false;
        }
    }
}
