using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Transform cam;
    private Vector3 previousCamPos;

    [Range(0f, 1f)]
    public float parallaxEffect = 0.5f; // Lower value = slower movement = farther away

    void Start()
    {
        cam = Camera.main.transform;
        previousCamPos = cam.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cam.position - previousCamPos;
        transform.position += new Vector3(deltaMovement.x * parallaxEffect, deltaMovement.y * parallaxEffect, 0);
        previousCamPos = cam.position;
    }
}
