using UnityEngine;

/// <summary>
/// Manages the logic for the player receiving an ice cream object.
/// Moves the player's right arm to a specific position and attaches the ice cream to the hand.
/// </summary>
public class GetIceCream : MonoBehaviour
{
    public Transform handTransform;
    public GameObject playerRightArm;
    public GameObject iceCream;
    public static bool iceCreamGiven;

    /// <summary>
    /// Gives the ice cream to the player by:
    /// - Moving the player's right arm to a predefined position,
    /// - Parenting the ice cream to the player's hand transform,
    /// - Setting the ice cream's local position and rotation to align it properly,
    /// - Setting the static flag 'iceCreamGiven' to true.
    /// </summary>
    public void IceCream()
    {
        playerRightArm.transform.position = new Vector3(-0.259f, 1.395f, 0);
        iceCream.transform.SetParent(handTransform);
        iceCream.transform.localPosition = new Vector3(0, -0.2f, 0);
        iceCream.transform.localRotation = new Quaternion(90, 0, 0, 0);

        PlayerInventory.Instance.hasIceCream = true;
    }
}
