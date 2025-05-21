using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance;

    public Sprite eKeyIdle;
    public Sprite eKeyPressed;
    private GameObject spriteHolder;
    private Animator buttonPress;
    private SpriteRenderer interactSprite;
    [SerializeField] private AnimatorController buttonPressController;
    private Coroutine fadeCoroutine;
    private Coroutine pressResetCoroutine;
    private GameObject currentTarget;
    public int interactionCounter = 0;

    private float pressedDuration = 0.25f;

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
        spriteHolder = new GameObject("GlobalInteractButton");
        spriteHolder.transform.SetParent(transform);
        interactSprite = spriteHolder.AddComponent<SpriteRenderer>();
        interactSprite.sortingOrder = 999;
        interactSprite.sprite = eKeyIdle;
        interactSprite.color = new Color(1f, 1f, 1f, 0f);
        interactSprite.transform.localScale = new Vector3(0.44f, 0.44f, 1f);

        buttonPress = spriteHolder.AddComponent<Animator>();
        buttonPress.runtimeAnimatorController = buttonPressController;
    }

    void Update()
    {
        if (currentTarget != null && Input.GetKeyDown(KeyCode.E))
        {
            buttonPress.SetTrigger("press");
            if (pressResetCoroutine != null) StopCoroutine(pressResetCoroutine);
            pressResetCoroutine = StartCoroutine(ResetSpriteAfterDelay(pressedDuration));

            currentTarget.GetComponent<InteractiveItem>()?.TriggerInteraction();
        }

        // Position of Interact Button

        if (currentTarget != null)
        {
            SpriteRenderer currentTargetSprite = currentTarget.GetComponent<SpriteRenderer>();
            Vector3 currentTargetPos = currentTarget.transform.position;

            if (currentTarget.name == "AppartmentDoor")
            {
                interactSprite.transform.position = new Vector3(currentTargetSprite.bounds.min.x - 0.5f, currentTargetPos.y + 1f, currentTargetPos.z);
            }
            else if (currentTarget.name == "WorkDoor" || currentTarget.name == "AppartmentDoorOutside" || currentTarget.name == "IceCreamVendor")
            {
                interactSprite.transform.position = new Vector3(currentTargetSprite.bounds.max.x + 0.5f, currentTargetPos.y + 1f, currentTargetPos.z);
            }
            else
            {
                interactSprite.transform.position = new Vector3(currentTargetPos.x, currentTargetSprite.bounds.max.y + 1, currentTargetPos.z);
            }
        }
    }

    public void ShowButton(GameObject target)
    {
        currentTarget = target;
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeSprite(0f, 1f, 0.1f));
    }

    public void HideButton()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeSprite(1f, 0f, 0.1f));
        currentTarget = null;
    }

    private IEnumerator FadeSprite(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color color = interactSprite.color;

        while (elapsed < duration)
        {
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            interactSprite.color = color;
            elapsed += Time.deltaTime;
            yield return null;
        }

        color.a = endAlpha;
        interactSprite.color = color;
    }

    private IEnumerator ResetSpriteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        interactSprite.sprite = eKeyIdle;
    }

    public void IncreaseCounter()
    {
        interactionCounter++;
        Debug.Log("Interaction amount: " + interactionCounter);
    }
}