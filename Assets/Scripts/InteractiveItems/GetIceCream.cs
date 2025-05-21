using Unity.VisualScripting;
using UnityEngine;

public class GetIceCream : MonoBehaviour
{
    public Transform handTransform;
    public GameObject playerRightArm;
    public GameObject iceCream;
    public static bool iceCreamGiven;

    public void IceCream()
    {
        playerRightArm.transform.position = new Vector3(-0.259f, 1.395f, 0);

        iceCream.transform.SetParent(handTransform);
        iceCream.transform.localPosition = new Vector3(0, -0.2f, 0);
        iceCream.transform.localRotation = new Quaternion(90, 0, 0, 0);
        iceCreamGiven = true;
}
}
