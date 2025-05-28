using UnityEngine;

/// <summary>
/// Handles the logic for picking up and putting down a glass object.
/// Moves the glass to the player's hand and triggers related animations and movement disabling.
/// </summary>
public class GlassPickup : MonoBehaviour
{
    public Transform handTransform;

    public Vector3 originalPosition;
    public Quaternion originalRotation;

    /// <summary>
    /// Saves the original position and rotation of the glass on start.
    /// </summary>
    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

    /// <summary>
    /// Picks up the glass:
    /// - Finds the player's right hand transform,
    /// - Disables player movement,
    /// - Parents the glass to the hand and positions it correctly,
    /// - Triggers the player's drinking animation.
    /// </summary>
    public void PickUp()
    {
        handTransform = GameObject.Find("rightHand").transform;
        SpriteRenderer playerSprite = GameObject.FindWithTag("Player").GetComponent<SpriteRenderer>();
        GameObject.FindWithTag("Player").GetComponent<PlayerScript>().DisableMovement();

        transform.SetParent(handTransform);
        transform.localPosition = new Vector3(0.3f, -0.35f, 0);
        transform.localRotation = new Quaternion(0, 0, 180, 0);

        GetComponent<InteractiveItem>().playerAnimator.SetTrigger("drinking");
    }

    /// <summary>
    /// Puts down the glass:
    /// - Unparents the glass,
    /// - Returns it to its original position and rotation in the world.
    /// </summary>
    public void PutDown()
    {
        transform.SetParent(null);
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }
}
