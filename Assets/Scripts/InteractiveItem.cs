using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractiveItem : MonoBehaviour
{
    public GameObject player;
    public Animator playerAnimator;
    public ObjectiveScript objectiveScript;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InteractionManager.Instance?.ShowButton(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InteractionManager.Instance?.HideButton();
        }
    }

    public void TriggerInteraction()
    {
        string name = gameObject.name;
        playerAnimator = player.GetComponent<Animator>();

        if (name == "Glass")
        {
            GlassPickup glassPickup = GetComponent<GlassPickup>();
            if (glassPickup != null)
            {
                glassPickup.PickUp();
            }
        }
        else if (name == "AppartmentDoor")
        {
            SceneManager.LoadScene("Outside");
        }
        else if (name == "WorkDoor")
        {
            if (objectiveScript != null)
                objectiveScript.workCounter++;
            SceneManager.LoadScene("Appartment");
        }
        else if (name == "WorkChair")
        {
            WorkMinigame workMinigame = GameObject.Find("WorkMinigameManager").GetComponent<WorkMinigame>();
            workMinigame.playerIsSitting = true;
        }
    }
}