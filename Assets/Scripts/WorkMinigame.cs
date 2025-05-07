using UnityEngine;
using UnityEngine.UI;

public class WorkMinigame : MonoBehaviour
{
    public Image completionCircle;
    public bool playerIsSitting;
    public GameObject MinigameVisual;

    public float fillSpeed = 0.01f;
    public GameObject playerSit;
    private CameraFollow mainCamera;
    private GameObject player;
    [SerializeField] private GameObject interactableItem;

    void Start()
    {
        playerIsSitting = false;
        MinigameVisual.SetActive(false);
        completionCircle.fillAmount = 0;
        mainCamera = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        player = GameObject.FindWithTag("Player");
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
            player.SetActive(false);
            player.GetComponent<PlayerScript>().DisableMovement();
            playerSit.SetActive(true);

            MinigameVisual.SetActive(true);

            if (Input.GetKey(KeyCode.E) && completionCircle.fillAmount < 1)
            {
                completionCircle.fillAmount += fillSpeed * Time.deltaTime;
            }
            else if (completionCircle.fillAmount > 0f && completionCircle.fillAmount != 1)
            {
                completionCircle.fillAmount -= fillSpeed * Time.deltaTime * 2;
                if (completionCircle.fillAmount < 0f)
                    completionCircle.fillAmount = 0f;
            }
            else if (completionCircle.fillAmount == 1)
            {
                mainCamera.target = player.transform;
                player.SetActive(true);
                player.GetComponent<PlayerScript>().EnableMovement();
                player.GetComponent<Animator>().SetTrigger("Idle");
                playerIsSitting = false;
                playerSit.SetActive(false);
                interactableItem.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        else
        {
            MinigameVisual.SetActive(false);
        }
    }
}
