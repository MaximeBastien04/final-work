using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;

    public bool hasInteractedWithMeditationLady = false;

    public HashSet<string> completedInteractions = new HashSet<string>();

    public GameObject EndGamePrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool HasCompleted(string interactionId)
    {
        return completedInteractions.Contains(interactionId);
    }

    public void MarkCompleted(string interactionId)
    {
        completedInteractions.Add(interactionId);
    }
    
    public void StoryFinished()
    {
        Color bgColor = BackgroundColorManager.Instance.GetComponent<SpriteRenderer>().color;
        float vignetteValue = PostProcessingManager.Instance.vignette.intensity.value;

        if (vignetteValue <= 0.1f)
        {
            GameObject fadeInSquare = Instantiate(EndGamePrefab, Vector3.zero, Quaternion.identity);
            Image blackSquareImage = fadeInSquare.transform.Find("BlackSquare").GetComponent<Image>();

            blackSquareImage.color = new Color(bgColor.r, bgColor.g, bgColor.b, 0f);


            GameObject.FindWithTag("Player").GetComponent<PlayerScript>().DisableMovement();
            StartCoroutine(FadeIn(blackSquareImage, 15f));
        }
    }

    private IEnumerator FadeIn(Image image, float duration)
    {
        Color startColor = new Color(1f, 0.8f, 0.3176471f, 0f);

        Color endColor = startColor;
        endColor.a = 1f;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            image.color = Color.Lerp(startColor, endColor, elapsed / duration);
            yield return null;
        }

        image.color = endColor;
    }
}
