using UnityEngine;

/// <summary>
/// Smoothly follows a target transform with adjustable speed and vertical offset.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 2f;
    public float yOffset = 1f;
    public Transform target;

    /// <summary>
    /// Updates the camera position each frame to smoothly follow the target with a vertical offset.
    /// Uses spherical linear interpolation for smooth movement.
    /// </summary>
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Immediately sets the camera's position to the target's position plus vertical offset.
    /// Does not use smoothing.
    /// </summary>
    public void SetCameraPosition()
    {
        transform.position = new Vector3(target.position.x, target.position.y + yOffset, -10f);
    }
}
