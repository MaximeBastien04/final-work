using UnityEngine;
using UnityEngine.UI;

public class WorkMinigame : MonoBehaviour
{
    [Header("Visual")]
    private GameObject player;
    public Image completionCircle;
    public GameObject playerSit;
    public float fillSpeed = 0.01f;
    public GameObject MinigameVisual;
    public BackgroundColorManager bgcManager;
    private PostProcessingManager postProManager;

    [Header("Audio")]
    public AudioSource audioSource;
    private AudioSource discoverySFX;

    public bool playerIsSitting;
    private CameraFollow mainCamera;
    [SerializeField] private GameObject interactableItem;

    void Start()
    {
        playerIsSitting = false;
        MinigameVisual.SetActive(false);
        completionCircle.fillAmount = 0;
        mainCamera = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        player = GameObject.FindWithTag("Player");
        discoverySFX = GameObject.Find("DiscoverySFX").GetComponent<AudioSource>();
        bgcManager = GameObject.Find("BackgroundColor").GetComponent<BackgroundColorManager>();
        postProManager = GameObject.Find("Global Volume").GetComponent<PostProcessingManager>();
    }

    void Update()
    {
        MinigameLogic();
    }

    public void MinigameLogic()
    {
        if (playerIsSitting)
        {
            mainCamera.target = playerSit.transform;
            // player.SetActive(false);
            player.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            player.GetComponent<PlayerScript>().DisableMovement();
            GameObject.Find("WorkChair").GetComponent<BoxCollider2D>().enabled = false;
            playerSit.SetActive(true);

            MinigameVisual.SetActive(true);

            if (Input.GetKey(KeyCode.E) && completionCircle.fillAmount < 1)
            {
                completionCircle.fillAmount += fillSpeed * Time.deltaTime;

                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }

                if (completionCircle.fillAmount > 0f && completionCircle.fillAmount != 1)
                {
                    completionCircle.fillAmount -= fillSpeed * Time.deltaTime * 2;
                    if (completionCircle.fillAmount < 0f)
                        completionCircle.fillAmount = 0f;
                }
                else if (completionCircle.fillAmount == 1)
                {
                    mainCamera.target = player.transform;
                    player.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
                    player.GetComponent<PlayerScript>().EnableMovement();
                    playerIsSitting = false;
                    playerSit.SetActive(false);
                    interactableItem.GetComponent<BoxCollider2D>().enabled = false;
                    discoverySFX.Play();
                    bgcManager.UpdateBackgroundColor();
                    postProManager.DecreaseVignetteSmoothly();
                }
            }
        }
        else
        {
            MinigameVisual.SetActive(false);
        }
    }
}
