using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractiveItem : MonoBehaviour
{
    private SpriteRenderer interactSprite;
    private Animator playerAnimator;
    private bool inRange = false;

    // Interact Button Animations
    private Coroutine fadeCoroutine;
    private Coroutine pressResetCoroutine;
    private float pressedButtonSpriteDuration = 0.25f;


    public Sprite eKeyIdle;
    public Sprite eKeyPressed;

    void Start()
    {
        playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();

        interactSprite = gameObject.transform.Find("InteractButton").GetComponent<SpriteRenderer>();
        interactSprite.color = new Color(1f, 1f, 1f, 0f);
        interactSprite.sprite = eKeyIdle;
    }

    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            interactSprite.sprite = eKeyPressed;

            if (pressResetCoroutine != null) StopCoroutine(pressResetCoroutine);
            pressResetCoroutine = StartCoroutine(ResetSpriteAfterDelay(pressedButtonSpriteDuration));

            if (gameObject.name == "Glass")
            {
                Debug.Log("Drink glass animation");
                // playerAnimator.SetTrigger("DrinkWater");
            }
            else if (gameObject.name == "AppartmentDoor")
            {
                Debug.Log("Go outside");
                SceneManager.LoadScene("Outside");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered trigger with: " + other.name);
        if (other.name == "Player")
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeSprite(interactSprite, 0f, 1f, 0.1f));
            inRange = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeSprite(interactSprite, 1f, 0f, 0.1f));
            inRange = false;
        }
    }


    // Interact button fade animation
    private IEnumerator FadeSprite(SpriteRenderer spriteRenderer, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color color = spriteRenderer.color;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            spriteRenderer.color = color;
            elapsed += Time.deltaTime;
            yield return null;
        }

        color.a = endAlpha;
        spriteRenderer.color = color;
    }


    // Interact button pressed sprite animation
    private IEnumerator ResetSpriteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        interactSprite.sprite = eKeyIdle;
    }

}
