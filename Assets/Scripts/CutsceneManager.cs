using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [Header("Home Scene")]
    public GameObject homeSceneWindow;
    public GameObject homeMainMenu;
    public GameObject homeMainMenuButtons;
    public GameObject fadeToBlackSquare;

    void Start()
    {        
        foreach (Transform child in homeMainMenuButtons.transform)
        {
            child.GetComponentInChildren<Button>().enabled = true;
        }
    }

    public void CloseWindow()
    {
        GameObject.Find("Window").GetComponent<PlayableDirector>().Play();
    }

    public void SetActiveWindow()
    {
        homeSceneWindow.SetActive(true);
    }

    public void SetInactiveHomeMenu()
    {
        homeMainMenu.SetActive(false);
        
    }

    public void SetInactiveHomeButtons()
    {
        foreach (Transform child in homeMainMenuButtons.transform)
        {
            child.GetComponentInChildren<Button>().enabled = false;
        }
        
    }

    public void SetActiveFadeToBlackSquare()
    {
        fadeToBlackSquare.SetActive(true);
    }
}
