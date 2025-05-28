using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the background color based on moods and smoothly follows the player.
/// Implements singleton pattern for global access.
/// </summary>
public class BackgroundColorManager : MonoBehaviour
{
    public static BackgroundColorManager Instance;

    private SpriteRenderer spriteRenderer;
    private Color targetColor;
    public float lerpSpeed = 2f;

    public float followSpeed = 2f;
    public float yOffset = 1f;
    public Transform target;

    /// <summary>
    /// Initializes the singleton instance, ensures only one instance exists,
    /// and sets the initial target color to the current background color.
    /// </summary>
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

    /// <summary>
    /// Lerps the background color towards the target color each frame and moves background to follow the player with an offset.
    /// </summary>
    void Update()
    {
        spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, lerpSpeed * Time.deltaTime);

        // Follow Player
        target = GameObject.FindWithTag("Player")?.transform;
        if (target != null)
            transform.position = new Vector3(target.position.x, target.position.y + yOffset, transform.position.z);
    }

    /// <summary>
    /// Sets the target background color.
    /// </summary>
    /// <param name="newColor">The new target color to set as the background.</param>
    public void SetBackgroundColor(Color newColor)
    {
        targetColor = newColor;
    }

    /// <summary>
    /// Sets the target background color using RGB values in 0-255 range.
    /// </summary>
    /// <param name="r">Red component (0-255).</param>
    /// <param name="g">Green component (0-255).</param>
    /// <param name="b">Blue component (0-255).</param>
    public void SetBackgroundColor255(int r, int g, int b)
    {
        targetColor = new Color(r / 255f, g / 255f, b / 255f);
    }

    /// <summary>
    /// Updates the background color to a brighter shade if it is below certain RGB thresholds.
    /// Logs a message when the color is sufficiently bright (indicating "happy" mood).
    /// </summary>
    public void UpdateBackgroundColor()
    {
        Color currentColor = GetComponent<SpriteRenderer>().color;
        if (currentColor.r < (250f / 255f) && currentColor.g < (210f / 255f) && currentColor.b < (90f / 255f))
        {
            targetColor = new Color(currentColor.r + (10f / 255f), currentColor.g + (8f / 255f), currentColor.b + (2f / 255f));
        }
        else
        {
            Debug.Log("Protagonist is happy!");
        }
    }
}
