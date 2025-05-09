using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    [Header("Light Switch Settings")]
    private SpriteRenderer lightSwitchSprite;
    private GameObject lightChild;
    private bool isLightOn = false;

    private void Start()
    {
        lightSwitchSprite = GetComponent<SpriteRenderer>();
        lightChild = transform.Find("Light").gameObject;
        lightChild.SetActive(false);

    }
    public void ToggleLightSwitch()
    {
        lightSwitchSprite.flipY = !lightSwitchSprite.flipY;
        isLightOn = !isLightOn;
        lightChild.SetActive(isLightOn);
    }
}
