using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractiveItem : MonoBehaviour
{
    public GameObject player;
    public Animator playerAnimator;
    public StoryManager storyManager;
    public BackgroundColorManager bgcManager;
    private PostProcessingManager postProManager;
    public AudioManager audioManager;
    private InteractionManager interactionManager;
    private bool hasBeenInteracted = false;
    private AudioSource discoverySFX;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        // storyManager = GameObject.Find("StoryManager").GetComponent<StoryManager>();
        bgcManager = GameObject.Find("BackgroundColor").GetComponent<BackgroundColorManager>();
        postProManager = GameObject.Find("Global Volume").GetComponent<PostProcessingManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        discoverySFX = GameObject.Find("DiscoverySFX").GetComponent<AudioSource>();
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
        interactionManager = GameObject.Find("InteractionManager").GetComponent<InteractionManager>();

        if (name == "Glass")
        {
            GlassPickup glassPickup = GetComponent<GlassPickup>();
            glassPickup.PickUp();
            interactionManager.IncreaseCounter();
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
            // only make this available if tutorial is done
            SceneManager.LoadScene("Outside");
            audioManager.PlayClip(audioManager.doorOpen);
        }
        else if (name == "AppartmentDoorOutside")
        {
            SceneManager.LoadScene("Appartment");
            audioManager.PlayClip(audioManager.doorOpen);
        }
        else if (name == "WorkDoor")
        {
            // Load outside of work
            audioManager.PlayClip(audioManager.doorOpen);
        }
        else if (name == "WorkChair")
        {
            WorkMinigame workMinigame = GameObject.Find("WorkMinigameManager").GetComponent<WorkMinigame>();
            workMinigame.playerIsSitting = true;
            if (!hasBeenInteracted)
            {
                bgcManager.UpdateBackgroundColor();
            }
        }
        else if (name == "Printer")
        {
            Printer printer = GetComponent<Printer>();
            printer.PrintPaper();
        }
        else if (name == "Grandma")
        {
            GetComponent<DialogueManager>().Talk();
            if (!hasBeenInteracted)
            {
                bgcManager.UpdateBackgroundColor();
                postProManager.DecreaseVignetteSmoothly();
            }
        }
        else if (name == "Dog")
        {
            GetComponent<Animator>().SetTrigger("bark");
            GetComponent<AudioSource>().Play();
            if (!hasBeenInteracted)
            {
                bgcManager.UpdateBackgroundColor();
                postProManager.DecreaseVignetteSmoothly();
            }
        }
        else if (name == "FlowerGarden")
        {
        }

        if (gameObject.tag == "HappinessIncrease")
        {
            if (!hasBeenInteracted)
            {
                discoverySFX.Play();
                bgcManager.UpdateBackgroundColor();
                postProManager.DecreaseVignetteSmoothly();
            }
        }




        // check to only make interactable items interactable once
        if (!hasBeenInteracted)
        {
            hasBeenInteracted = true;
        }
    }
}