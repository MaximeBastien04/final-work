using UnityEngine;

public class GlassPickup : MonoBehaviour
{
    public Transform handTransform;

    public Vector3 originalPosition;
    public Quaternion originalRotation;

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }

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

    public void PutDown()
    {
        transform.SetParent(null);
        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }
}
