using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class PostProcessingManager : MonoBehaviour
{
    private Vignette vignette;
    private Coroutine vignetteRoutine;

    public static PostProcessingManager Instance;

    
    void Awake()
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

    void Start()
    {
        Volume volume = GameObject.Find("Global Volume").GetComponent<Volume>();

        if (volume.profile.TryGet(out vignette))
        {
            // Vignette found
        }
        else
        {
            Debug.LogWarning("Vignette not found in Volume Profile.");
        }
    }

    public void DecreaseVignetteSmoothly()
    {
        if (vignette == null) return;

        if (vignetteRoutine != null)
        {
            StopCoroutine(vignetteRoutine);
        }

        vignetteRoutine = StartCoroutine(FadeVignetteTo(vignette.intensity.value - 0.015f, 1f));
    }

    private IEnumerator FadeVignetteTo(float target, float duration)
    {
        float start = vignette.intensity.value;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(start, target, elapsed / duration);
            yield return null;
        }

        vignette.intensity.value = target;
    }
}
