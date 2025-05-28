using UnityEngine;

/// <summary>
/// Controls a light switch that toggles the visual state of the switch and
/// activates or deactivates a child light GameObject.
/// </summary>
public class LightSwitch : MonoBehaviour
{
    [Header("Light Switch Settings")]
    private SpriteRenderer lightSwitchSprite;
    private GameObject lightChild;
    private bool isLightOn = false;

    /// <summary>
    /// Initializes references: gets the SpriteRenderer component and finds the child "Light" GameObject,
    /// which is initially disabled.
    /// </summary>
    private void Start()
    {
        lightSwitchSprite = GetComponent<SpriteRenderer>();
        lightChild = transform.Find("Light").gameObject;
        lightChild.SetActive(false);

    }

    
    /// <summary>
    /// Toggles the light switch:
    /// - flips the switch sprite vertically,
    /// - toggles the light on/off state,
    /// - activates or deactivates the light child GameObject accordingly.
    /// </summary>
    public void ToggleLightSwitch()
    {
        lightSwitchSprite.flipY = !lightSwitchSprite.flipY;
        isLightOn = !isLightOn;
        lightChild.SetActive(isLightOn);
    }
}
