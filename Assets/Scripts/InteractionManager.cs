using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance;

    private InteractiveItem currentInteractiveItem;

    public GameObject buttonPrefab;
    private GameObject interactionButton;
    private SpriteRenderer buttonSpriteRenderer;
    private Animator buttonAnimator;
    private Light2D buttonGlow;
    [SerializeField] private AnimatorController buttonPressController;
    private Coroutine fadeCoroutine;
    private GameObject currentTarget;
    public int interactionCounter = 0;

    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        interactionButton = Instantiate(buttonPrefab, transform);

        buttonAnimator = interactionButton.GetComponent<Animator>();
        buttonSpriteRenderer = interactionButton.GetComponent<SpriteRenderer>();
        buttonGlow = interactionButton.GetComponent<Light2D>();
    }

    /// <summary>
    /// Updates the position of the interaction button based on the current target's position.
    /// </summary>
    void Update()
    {
        if (currentTarget != null)
        {
            SpriteRenderer currentTargetSprite = currentTarget.GetComponent<SpriteRenderer>();
            Vector3 currentTargetPos = currentTarget.transform.position;

            if (currentTarget.name == "AppartmentDoor" || currentTarget.name == "WorkDoorOutside")
            {
                interactionButton.transform.position = new Vector3(currentTargetSprite.bounds.min.x - 0.5f, currentTargetPos.y + 1f, currentTargetPos.z);
            }
            else if (currentTarget.name == "WorkDoor" || currentTarget.name == "AppartmentDoorOutside")
            {
                interactionButton.transform.position = new Vector3(currentTargetSprite.bounds.max.x + 0.5f, currentTargetPos.y + 1f, currentTargetPos.z);
            }
            else if (currentTarget.name == "IceCreamVendor")
            {
                interactionButton.transform.position = new Vector3(currentTargetPos.x, currentTargetPos.y + 3f, currentTargetPos.z);
            }
            else
            {
                interactionButton.transform.position = new Vector3(currentTargetPos.x, currentTargetSprite.bounds.max.y + 1, currentTargetPos.z);
            }
        }
    }

    /// <summary>
    /// Triggers the interaction logic of the current interactive item.
    /// </summary>
    public void Interact()
    {
        if (currentInteractiveItem != null)
        {
            currentInteractiveItem.TriggerInteraction();
        }
    }

    /// <summary>
    /// Displays the interaction button near the specified target and starts its fade-in animation.
    /// </summary>
    /// <param name="target">The GameObject the player can interact with.</param>
    public void ShowButton(GameObject target)
    {
        currentTarget = target;
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeSprite(0f, 1f, 0f, 3f, 0.3f));

        currentInteractiveItem = target.GetComponent<InteractiveItem>();
    }

    /// <summary>
    /// Hides the interaction button and starts its fade-out animation.
    /// </summary>
    public void HideButton()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeSprite(1f, 0f, 3f, 0f, 0.3f));
        currentTarget = null;

        currentInteractiveItem = null;
    }

    /// <summary>
    /// Coroutine that fades the interaction button's visibility and glow over time.
    /// </summary>
    /// <param name="startAlpha">The initial alpha (transparency) of the button sprite.</param>
    /// <param name="endAlpha">The final alpha of the button sprite.</param>
    /// <param name="startIntensity">The initial intensity of the button's light glow.</param>
    /// <param name="endIntensity">The final intensity of the button's light glow.</param>
    /// <param name="duration">The duration over which the fade occurs.</param>
    /// <returns>IEnumerator used for coroutine execution.</returns>
    private IEnumerator FadeSprite(float startAlpha, float endAlpha, float startIntensity, float endIntensity, float duration)
    {
        float elapsed = 0f;
        Color color = buttonSpriteRenderer.color;

        while (elapsed < duration)
        {
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            buttonGlow.intensity = Mathf.Lerp(startIntensity, endIntensity, elapsed / duration);
            buttonSpriteRenderer.color = color;
            elapsed += Time.deltaTime;
            yield return null;
        }

        color.a = endAlpha;
        buttonSpriteRenderer.color = color;
    }
}