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

    /// <summary>
    /// Initializes the vignette reference from the Volume component on this GameObject.
    /// </summary>
    void Start()
    {
        Volume volume = GetComponent<Volume>();
    }

    /// <summary>
    /// Smoothly decreases the intensity of the vignette by a fixed amount over time.
    /// </summary>
    public void DecreaseVignetteSmoothly()
    {
        if (vignette == null) return;

        if (vignetteRoutine != null)
        {
            StopCoroutine(vignetteRoutine);
        }

        vignetteRoutine = StartCoroutine(FadeVignetteTo(vignette.intensity.value - 0.03f, 1f));
    }


    /// <summary>
    /// Coroutine to gradually interpolate the vignette intensity toward a target value.
    /// </summary>
    /// <param name="target">The target vignette intensity value.</param>
    /// <param name="duration">The duration over which to fade the intensity.</param>
    /// <returns>IEnumerator used for coroutine execution.</returns>
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
