using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public static bool hasWorked = false;

    [Header("Specific items")]
    [SerializeField] private GameObject iceCream;
    [SerializeField] private GameObject coffee;
    private bool hasStartedOldLadyDialogue = false;
    private bool hasAnsweredMeditationQuestion = false;


    void Start()
    {
        player = GameObject.FindWithTag("Player");
        storyManager = StoryManager.Instance;
        bgcManager = GameObject.Find("BackgroundColor").GetComponent<BackgroundColorManager>();
        postProManager = GameObject.Find("Global Volume").GetComponent<PostProcessingManager>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        discoverySFX = GameObject.Find("DiscoverySFX").GetComponent<AudioSource>();
    }

    /// <summary>
    /// Shows the interaction button when the player enters the trigger zone.
    /// </summary>
    /// <param name="other">Collider of the object that entered the trigger zone.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InteractionManager.Instance?.ShowButton(gameObject);
        }
    }

    /// <summary>
    /// Hides the interaction button when the player exits the trigger zone.
    /// </summary>
    /// <param name="other">Collider of the object that exited the trigger zone.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InteractionManager.Instance?.HideButton();
        }
    }

    void Update()
    {
        Debug.Log(hasWorked);
    }

    /// <summary>
    /// Triggers an interaction based on the name of the interactive object. Each object has a unique behavior.
    /// </summary>
    public void TriggerInteraction()
    {
        string name = gameObject.name;
        string interactionID = name;
        playerAnimator = player.GetComponent<Animator>();
        interactionManager = GameObject.Find("InteractionManager").GetComponent<InteractionManager>();

        if (name == "OldLady")
        {
            DialogueManager dialogueManager = GetComponent<DialogueManager>();
            GameObject playerSit = transform.Find("ProtagonistSitting").gameObject;
            CameraFollow mainCamera = GameObject.Find("Main Camera").GetComponent<CameraFollow>();

            if (storyManager.HasCompleted(interactionID))
            {
                DialogueManager.DialogueLine[] lastSentence = new DialogueManager.DialogueLine[]
                {
                    new DialogueManager.DialogueLine { text = "And come sit with me again sometime. The bench is always here.", pauseAfter = 0f }
                };

                dialogueManager.ReplaceText(lastSentence);
                dialogueManager.Talk();
                return;
            }
            player.GetComponent<PlayerScript>().DisableMovement();
            player.GetComponent<SpriteRenderer>().enabled = false;
            playerSit.SetActive(true);
            mainCamera.target = playerSit.transform;

            if (!hasStartedOldLadyDialogue)
            {
                hasStartedOldLadyDialogue = true;
                StartCoroutine(StartOldLadyDialogueWithDelay(dialogueManager));
            }
            else
            {
                dialogueManager.Talk();
            }

            dialogueManager.OnFinalLineStarted += () =>
            {
                // Trigger happiness increase BEFORE the player stands up
                TriggerHappinessIncrease();

                // Now wrap up the interaction
                player.GetComponent<SpriteRenderer>().enabled = true;
                playerSit.SetActive(false);
                player.GetComponent<PlayerScript>().EnableMovement();
                mainCamera.target = player.transform;

                storyManager.MarkCompleted(interactionID);
            };

        }
        else if (name == "MeditationLady")
        {
            DialogueManager dialogueManager = GetComponent<DialogueManager>();
            GameObject playerMeditate = transform.Find("MeditationProtagonist").gameObject;
            CameraFollow mainCamera = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
            Animator eyesClosed = GameObject.Find("BlackSquare").GetComponent<Animator>();

            if (storyManager.HasCompleted(interactionID))
            {
                DialogueManager.DialogueLine[] lastSentence = new DialogueManager.DialogueLine[]
                {
                    new DialogueManager.DialogueLine { text = "You're welcome to meditate here anytime.", pauseAfter = 0f }
                };

                dialogueManager.ReplaceText(lastSentence);
                dialogueManager.Talk();
                return;
            }

            dialogueManager.Talk();


            if (!storyManager.hasInteractedWithMeditationLady)
            {
                dialogueManager.OnFinalLineStarted += () =>
                {
                    GetComponent<BoxCollider2D>().enabled = false;

                    dialogueManager.ShowChoice(
                            yesAction: () =>
                            {
                                GetComponent<BoxCollider2D>().enabled = true;
                                dialogueManager.dialoguePanel.SetActive(false);
                                dialogueManager.RemoveText();
                                player.GetComponent<PlayerScript>().DisableMovement();
                                player.GetComponent<SpriteRenderer>().enabled = false;
                                playerMeditate.SetActive(true);
                                mainCamera.target = playerMeditate.transform;

                                eyesClosed.SetTrigger("fadeIn");


                                DialogueManager.DialogueLine[] meditationDialogue = new DialogueManager.DialogueLine[]
                                {
                                new DialogueManager.DialogueLine { text = "Start by closing your eyes.", pauseAfter = 3f },
                                new DialogueManager.DialogueLine { text = "Good.", pauseAfter = 0f },
                                new DialogueManager.DialogueLine { text = "Now tell me.", pauseAfter = 0f },
                                new DialogueManager.DialogueLine { text = "What do you hear?", pauseAfter = 0f }
                                };

                                dialogueManager.ReplaceText(meditationDialogue);
                                dialogueManager.Talk();

                                dialogueManager.OnFinalLineStarted = () =>
                                {
                                    DialogueManager.DialogueLine[] afterChoiceDialogue = new DialogueManager.DialogueLine[]
                                    {
                                    new DialogueManager.DialogueLine { text = "I see.", pauseAfter = 1f },
                                    new DialogueManager.DialogueLine { text = "Whenever I feel overwhelmed, I stop for a moment.", pauseAfter = 0f },
                                    new DialogueManager.DialogueLine { text = "I listen. I breathe.", pauseAfter = 0f },
                                    new DialogueManager.DialogueLine { text = "I'm consious about my surroundings.", pauseAfter = 2f },
                                    new DialogueManager.DialogueLine { text = "Just being present helps me feel calm again.", pauseAfter = 0f },
                                    new DialogueManager.DialogueLine { text = "Tone the pressure down.", pauseAfter = 1.5f },
                                    new DialogueManager.DialogueLine { text = "You're welcome to meditate here anytime.", pauseAfter = 0f }
                                    };

                                    if (!hasAnsweredMeditationQuestion)
                                    {
                                        hasAnsweredMeditationQuestion = true;

                                        dialogueManager.ShowMultiChoice("What do you hear?",
                                            new (string, System.Action)[]
                                            {
                                            ("Birds", () => {
                                                GetComponent<BoxCollider2D>().enabled = true;
                                                dialogueManager.ReplaceText(afterChoiceDialogue);
                                                dialogueManager.Talk();
                                                storyManager.hasInteractedWithMeditationLady = true;
                                            }),
                                            ("Dog", () => {
                                                GetComponent<BoxCollider2D>().enabled = true;
                                                dialogueManager.ReplaceText(afterChoiceDialogue);
                                                dialogueManager.Talk();
                                                storyManager.hasInteractedWithMeditationLady = true;
                                            }),
                                            ("Cars", () => {
                                                GetComponent<BoxCollider2D>().enabled = true;
                                                dialogueManager.ReplaceText(afterChoiceDialogue);
                                                dialogueManager.Talk();
                                                storyManager.hasInteractedWithMeditationLady = true;
                                            })
                                            });
                                    }
                                    else
                                    {
                                        dialogueManager.ReplaceText(afterChoiceDialogue);
                                        dialogueManager.Talk();
                                    }

                                    dialogueManager.OnFinalLineStarted = () =>
                                    {
                                        GetComponent<BoxCollider2D>().enabled = true;
                                        eyesClosed.SetTrigger("fadeOut");

                                        player.GetComponent<SpriteRenderer>().enabled = true;
                                        playerMeditate.SetActive(false);
                                        player.GetComponent<PlayerScript>().EnableMovement();
                                        mainCamera.target = player.transform;

                                        gameObject.tag = "HappinessIncrease";
                                        if (interactionAmount == 0)
                                        {
                                            hasBeenInteracted = false;
                                            interactionAmount++;
                                        }

                                        storyManager.MarkCompleted(interactionID);
                                    };
                                };
                            },
                            noAction: () =>
                            {
                                GetComponent<BoxCollider2D>().enabled = true;
                                dialogueManager.dialoguePanel.SetActive(false);
                                dialogueManager.choicePanel.SetActive(false);
                                player.GetComponent<SpriteRenderer>().enabled = true;
                                playerMeditate.SetActive(false);
                                player.GetComponent<PlayerScript>().EnableMovement();
                                mainCamera.target = player.transform;
                            }
                        );
                };
            }
            else
            {

                DialogueManager.DialogueLine[] afterChoiceDialogue = new DialogueManager.DialogueLine[]
                {
                    new DialogueManager.DialogueLine { text = "You're welcome to meditate here anytime.", pauseAfter = 0f }
                };
                gameObject.tag = "HappinessIncrease";
                if (interactionAmount == 0)
                {
                    hasBeenInteracted = false;
                    interactionAmount++;
                }
            }
        }
        else if (name == "Glass")
        {
            if (storyManager.HasCompleted(interactionID))
            {
                hasBeenInteracted = true;
            }

            player.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
            GlassPickup glassPickup = GetComponent<GlassPickup>();
            glassPickup.PickUp();
            storyManager.MarkCompleted(interactionID);
        }
        else if (name == "LightSwitch")
        {
            LightSwitch lightSwitch = GetComponent<LightSwitch>();
            lightSwitch.ToggleLightSwitch();
            audioManager.PlayClip(audioManager.lightSwitch);
        }
        else if (name == "AppartmentDoor")
        {
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
            SceneManager.LoadScene("Outside");
            audioManager.PlayClip(audioManager.doorOpen);
        }
        else if (name == "WorkChair")
        {
            if (storyManager.HasCompleted(interactionID))
            {
                GetComponent<BoxCollider2D>().enabled = false;
                return;
            }

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
        else if (name == "Dog")
        {
            if (storyManager.HasCompleted(interactionID))
            {
                hasBeenInteracted = true;
            }

            GetComponent<Animator>().SetTrigger("bark");
            GetComponent<AudioSource>().Play();
            player.GetComponent<PlayerScript>().DisableMovement();
            player.GetComponent<PlayerScript>().DisableInteraction();
            playerAnimator.SetTrigger("pet");
            storyManager.MarkCompleted(interactionID);
        }
        else if (name == "FlowerGarden")
        {
            if (storyManager.HasCompleted(interactionID))
            {
                hasBeenInteracted = true;
            }

            GetComponent<DialogueManager>().Talk();
            storyManager.MarkCompleted(interactionID);
        }
        else if (name == "IceCreamVendor")
        {
            DialogueManager dialogueManager = GetComponent<DialogueManager>();

            if (storyManager.HasCompleted(interactionID))
            {
                DialogueManager.DialogueLine[] lastSentence = new DialogueManager.DialogueLine[]
                {
                    new DialogueManager.DialogueLine { text = "Here you go.", pauseAfter = 0f }
                };

                dialogueManager.ReplaceText(lastSentence);
                dialogueManager.Talk();
                return;
            }

            dialogueManager.Talk();
            dialogueManager.OnFinalLineStarted += () =>
            {
                getIceCream = GetComponent<GetIceCream>();
                getIceCream.IceCream();
                storyManager.MarkCompleted(interactionID);

                TriggerHappinessIncrease();
            };
        }
        else if (name == "Child")
        {
            DialogueManager dialogueManager = GetComponent<DialogueManager>();

            if (storyManager.HasCompleted(interactionID))
            {
                DialogueManager.DialogueLine[] lastSentence = new DialogueManager.DialogueLine[]
                {
                    new DialogueManager.DialogueLine { text = "Thanks!", pauseAfter = 0f }
                };

                dialogueManager.ReplaceText(lastSentence);
                dialogueManager.Talk();
                return;
            }

            if (PlayerInventory.Instance.hasIceCream)
            {
                DialogueManager.DialogueLine[] newDialogue = new DialogueManager.DialogueLine[]
                {
                    new DialogueManager.DialogueLine { text = "You got ice cream for me?", pauseAfter = 0f },
                    new DialogueManager.DialogueLine { text = "Thanks!", pauseAfter = 0f }
                };

                dialogueManager.ReplaceText(newDialogue);

                dialogueManager.OnFinalLineStarted += () =>
                {

                    if (PlayerInventory.Instance.hasIceCream)
                    {
                        Transform leftHand = transform.Find("Stomach").Find("LowerTorso").Find("UpperTorso").Find("UpperLeftArm").Find("LowerLeftArm").Find("LeftHand");
                        GameObject childIceCream = Instantiate(iceCream, leftHand);
                        childIceCream.transform.localPosition = new Vector3(0.2f, -0.15f, 0);
                        childIceCream.transform.localRotation = new Quaternion(0, 0, 210, 0);
                        childIceCream.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);

                        PlayerInventory.Instance.hasIceCream = false;
                        Transform hand = GameObject.FindWithTag("Player")
                            .transform.Find("lowerTorso/midTorso/upperTorso/upperRightArm/lowerRightArm/rightHand");

                        Transform iceCreaminhand = hand?.Find("IceCream");

                        if (iceCream != null)
                        {
                            Destroy(iceCreaminhand.gameObject);  // ✅ This destroys the whole GameObject properly
                        }
                        else
                        {
                            Debug.LogWarning("IceCream not found in rightHand!");
                        }
                    }

                    TriggerHappinessIncrease();
                    GetComponent<Animator>().SetTrigger("eat");
                    storyManager.MarkCompleted(interactionID);
                    if (interactionAmount == 0)
                    {
                        hasBeenInteracted = false;
                        interactionAmount++;
                    }
                };
            }

            dialogueManager.Talk();
        }
        else if (name == "CoffeeMachine")
        {
            if (storyManager.HasCompleted(interactionID))
            {
                hasBeenInteracted = true;
            }

            player.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(0, 0);
            player.GetComponent<PlayerScript>().DisableMovement();
            Transform handTransform = player.transform.Find("lowerTorso").Find("midTorso").Find("upperTorso").Find("upperRightArm").Find("lowerRightArm").Find("rightHand");

            coffee.transform.SetParent(handTransform);
            coffee.transform.localPosition = new Vector3(0.3f, -0.35f, 0);
            coffee.transform.localRotation = new Quaternion(0, 0, 180, 0);

            playerAnimator.SetTrigger("drinking");
            storyManager.MarkCompleted(interactionID);
        }
        else if (name == "Coworker")
        {
            DialogueManager dialogueManager = GetComponent<DialogueManager>();

            if (storyManager.HasCompleted(interactionID))
            {
                DialogueManager.DialogueLine[] lastSentence = new DialogueManager.DialogueLine[]
                {
                    new DialogueManager.DialogueLine { text = "Try working a bit, it will cheer you up!", pauseAfter = 0f }
                };

                dialogueManager.ReplaceText(lastSentence);
                dialogueManager.Talk();
                return;
            }

            dialogueManager.Talk();
            dialogueManager.OnFinalLineStarted += () =>
                {
                    storyManager.MarkCompleted(interactionID);
                    TriggerHappinessIncrease();
                    if (interactionAmount == 0)
                    {
                        hasBeenInteracted = false;
                        interactionAmount++;
                    }
                };
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

    private void TriggerHappinessIncrease()
    {
        Debug.Log("Happiness increase");
        gameObject.tag = "HappinessIncrease";
        if (interactionAmount == 0)
        {
            hasBeenInteracted = false;
            interactionAmount++;
        }

        if (!hasBeenInteracted)
        {
            discoverySFX.Play();
            bgcManager.UpdateBackgroundColor();
            postProManager.DecreaseVignetteSmoothly();
            storyManager.StoryFinished();
        }

        hasBeenInteracted = true;
    }


    /// <summary>
    /// Coroutine that starts the dialogue with the Old Lady after a delay.
    /// </summary>
    /// <param name="dialogueManager">The dialogue manager controlling the conversation.</param>
    /// <returns>Waits for a few seconds before starting the dialogue.</returns>
    private IEnumerator StartOldLadyDialogueWithDelay(DialogueManager dialogueManager)
    {
        yield return new WaitForSeconds(6f);
        dialogueManager.Talk();
    }


}