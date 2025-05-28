using UnityEngine;

/// <summary>
/// Applies a parallax scrolling effect to the background based on the camera's movement,
/// creating a sense of depth.
/// </summary>
public class ParallaxBackground : MonoBehaviour
{
    private Transform cam;
    private Vector3 previousCamPos;

    [Range(0f, 1f)]
    public float parallaxEffect = 0.5f; // Lower value = slower movement = farther away

    /// <summary>
    /// Initializes the camera reference and stores its initial position.
    /// </summary>
    void Start()
    {
        cam = Camera.main.transform;
        previousCamPos = cam.position;
    }

    /// <summary>
    /// Updates the background position based on the camera's movement to create the parallax effect.
    /// </summary>
    void LateUpdate()
    {
        Vector3 deltaMovement = cam.position - previousCamPos;
        transform.position += new Vector3(deltaMovement.x * parallaxEffect, deltaMovement.y * parallaxEffect, 0);
        previousCamPos = cam.position;
    }
}
