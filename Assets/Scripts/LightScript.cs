using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    [Header("Light Switch Settings")]
    public SpriteRenderer lightSwitchSprite;  // Reference to the Sprite Renderer of the lightswitch
    public GameObject lightChild;            // Reference to the child light GameObject
    public float interactionDistance = 2f;   // Distance required to interact

    private Transform player;                // Reference to the player's transform
    private bool isLightOn = false;          // Keeps track of the light state

    private void Start()
    {
        // Find the player in the scene (assuming they have a "Player" tag)
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (lightChild != null)
        {
            Debug.Log("child found");
            lightChild.SetActive(false); // Ensure the light is off at the start
        }
        else
            Debug.Log("child not found");
    }

    private void Update()
    {
        if (player != null && Vector2.Distance(player.position, transform.position) <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleLightSwitch();
            }
        }
    }

    private void ToggleLightSwitch()
    {
        if (lightSwitchSprite != null)
        {
            lightSwitchSprite.flipY = !lightSwitchSprite.flipY;
        }

        // Toggle the light child's active state
        if (lightChild != null)
        {
            isLightOn = !isLightOn;
            lightChild.SetActive(isLightOn);
        }
    }
}
