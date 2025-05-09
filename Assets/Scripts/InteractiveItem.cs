using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractiveItem : MonoBehaviour
{
    public GameObject player;
    public Animator playerAnimator;
    public ObjectiveScript objectiveScript;
    public BackgroundColorManager bgcManager;
    public AudioManager audioManager;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        objectiveScript = GameObject.Find("ObjectiveManager").GetComponent<ObjectiveScript>();
        bgcManager = GameObject.Find("BackgroundColor").GetComponent<BackgroundColorManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
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
            glassPickup.PickUp();
            // audioManager.PlayClip(audioManager.drinkGlass);
        }
        else if (name == "LightSwitch")
        {
            LightSwitch lightSwitch = GetComponent<LightSwitch>();
            lightSwitch.ToggleLightSwitch();
            audioManager.PlayClip(audioManager.lightSwitch);
        }
        else if (name == "AppartmentDoor")
        {
            SceneManager.LoadScene("Outside");
            objectiveScript.ChangeObjective("Work");
            objectiveScript.GoToWork();
            audioManager.PlayClip(audioManager.doorOpen);
        }
        else if (name == "WorkDoor")
        {
            objectiveScript.ChangeObjective("Got to work, Again.");
            SceneManager.LoadScene("Appartment");
            audioManager.PlayClip(audioManager.doorOpen);
        }
        else if (name == "WorkChair")
        {
            WorkMinigame workMinigame = GameObject.Find("WorkMinigameManager").GetComponent<WorkMinigame>();
            workMinigame.playerIsSitting = true;
        }
        else if (name == "Printer")
        {
            // Paper coming out of printer animation
            // audioManager.PlayClip(audioManager.printer);
        }

    }
}