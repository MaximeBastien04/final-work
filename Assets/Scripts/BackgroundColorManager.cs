using System.Collections.Generic;
using UnityEngine;

public class BackgroundColorManager : MonoBehaviour
{
    public static BackgroundColorManager Instance;

    private SpriteRenderer spriteRenderer;
    private Color targetColor;
    public float lerpSpeed = 2f;

    private Dictionary<string, Color> moodColors = new Dictionary<string, Color>()
    {
        { "Happy", new Color(1f, 0.8f, 0.3f) },       // bright yellow-orange
        { "Sad", new Color(0.2f, 0.3f, 0.6f) },        // desaturated blue
        { "Anxious", new Color(0.5f, 0.4f, 0.6f) },    // muted purple
        { "Neutral", new Color(0.6f, 0.6f, 0.6f) },    // gray
        { "Hopeful", new Color(0.6f, 0.85f, 1f) }      // sky blue
    };

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

        spriteRenderer = GetComponent<SpriteRenderer>();
        targetColor = spriteRenderer.color;
    }

    void Update()
    {
        spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, lerpSpeed * Time.deltaTime);
    }

    /// Set background color using a Color
    public void SetBackgroundColor(Color newColor)
    {
        targetColor = newColor;
    }

    /// Set background color using RGB in 0-255 range
    public void SetBackgroundColor255(int r, int g, int b)
    {
        targetColor = new Color(r / 255f, g / 255f, b / 255f);
    }


    public void SetMood(string mood)
    {
        if (moodColors.TryGetValue(mood, out Color color))
        {
            SetBackgroundColor(color);
        }
        else
        {
            Debug.LogWarning("Mood not found: " + mood);
        }
    }
}
