using UnityEngine;

public class IceCreamRestorer : MonoBehaviour
{
    public GameObject iceCreamPrefab;
    public Transform handTransform;
    public GameObject playerRightArm;

    void Start()
    {
        if (PlayerInventory.Instance.hasIceCream)
        {
            if (!handTransform.Find("IceCream"))
            {
                GameObject iceCream = Instantiate(iceCreamPrefab, handTransform);
                iceCream.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                iceCream.name = "IceCream";
            }
        }
    }

}
