using Unity.VisualScripting;
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
    private GetIceCream getIceCream;
    private bool hasBeenInteracted = false;
    private int interactionAmount = 0;
    private AudioSource discoverySFX;
    private static bool hasWorked = false;

    [Header("Ice Cream Child Quest")]
    private bool playerHasIceCream = false;
    [SerializeField] private GameObject iceCream;



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
            SceneTransitionManager.Instance.lastExitDoor = name;
            SceneManager.LoadScene("Outside");
            audioManager.PlayClip(audioManager.doorOpen);
        }
        else if (name == "AppartmentDoorOutside")
        {
            SceneTransitionManager.Instance.lastExitDoor = name;
            SceneManager.LoadScene("Appartment");
            audioManager.PlayClip(audioManager.doorOpen);
        }
        else if (name == "WorkDoorOutside")
        {
            SceneTransitionManager.Instance.lastExitDoor = name;
            SceneManager.LoadScene("Work");
            audioManager.PlayClip(audioManager.doorOpen);
        }
        else if (name == "WorkDoor")
        {
            SceneTransitionManager.Instance.lastExitDoor = name;
            SceneManager.LoadScene("Outside"); // at position of door
            audioManager.PlayClip(audioManager.doorOpen);
            // player.transform.position = new Vector3(0,)
        }
        else if (name == "WorkChair")
        {
            if (!hasWorked)
            {
                WorkMinigame workMinigame = GameObject.Find("WorkMinigameManager").GetComponent<WorkMinigame>();
                workMinigame.playerIsSitting = true;
                hasWorked = true;
            }
            else
                GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (name == "Printer")
        {
            Printer printer = GetComponent<Printer>();
            printer.PrintPaper();
        }
        else if (name == "Grandma")
        {
            DialogueManager dialogueManager = GetComponent<DialogueManager>();

            dialogueManager.Talk();
            dialogueManager.OnFinalLineStarted += () =>
            {

                gameObject.tag = "HappinessIncrease";
                if (interactionAmount == 0)
                {
                    hasBeenInteracted = false;
                    interactionAmount++;
                }
            };
        }
        else if (name == "Dog")
        {
            // Dog pet animation
            GetComponent<Animator>().SetTrigger("bark");
            GetComponent<AudioSource>().Play();
            playerAnimator.SetTrigger("pet");
        }
        else if (name == "FlowerGarden")
        {
            GetComponent<DialogueManager>().Talk();
        }
        else if (name == "IceCreamVendor")
        {
            DialogueManager dialogueManager = GetComponent<DialogueManager>();
            dialogueManager.Talk();
            dialogueManager.OnFinalLineStarted += () =>
            {
                getIceCream = GetComponent<GetIceCream>();
                getIceCream.IceCream();
                playerHasIceCream = GetIceCream.iceCreamGiven;

                gameObject.tag = "HappinessIncrease";
                if (interactionAmount == 0)
                {
                    hasBeenInteracted = false;
                    interactionAmount++;
                }
            };
        }
        else if (name == "Child")
        {
            DialogueManager dialogueManager = GetComponent<DialogueManager>();

            if (GetIceCream.iceCreamGiven)
            {
                string[] newDialogue = { "You got ice cream for me?", "Thanks!" };
                dialogueManager.ReplaceText(newDialogue);

                dialogueManager.OnFinalLineStarted += () =>
                {
                    if (iceCream.activeSelf)
                    {
                        Transform leftHand = transform.Find("Stomach").Find("LowerTorso").Find("UpperTorso").Find("UpperLeftArm").Find("LowerLeftArm").Find("LeftHand");
                        iceCream.transform.SetParent(leftHand);
                        iceCream.transform.localPosition = new Vector3(0.2f, -0.15f, 0);
                        iceCream.transform.localRotation = new Quaternion(0, 0, 210, 0);
                        iceCream.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                    }

                    gameObject.tag = "HappinessIncrease";
                    GetComponent<Animator>().SetTrigger("eat");
                    if (interactionAmount == 0)
                    {
                        hasBeenInteracted = false;
                        interactionAmount++;
                    }
                };
            }

            dialogueManager.Talk();
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