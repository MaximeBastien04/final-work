using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages UI windows, button states, fade effects, and scene transitions in the Home scene.
/// </summary>
public class CutsceneManager : MonoBehaviour
{
    [Header("Home Scene")]
    public GameObject homeSceneWindow;
    public GameObject homeMainMenu;
    public GameObject homeMainMenuButtons;
    public GameObject fadeToBlackSquare;
    public bool windowIsClosed = false;

    /// <summary>
    /// Enables all buttons under homeMainMenuButtons if the active scene is "HomeScene".
    /// Called on scene start.
    /// </summary>
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "HomeScene")
            foreach (Transform child in homeMainMenuButtons.transform)
            {
                child.GetComponentInChildren<Button>().enabled = true;
            }
    }

    /// <summary>
    /// Plays the PlayableDirector attached to the GameObject named "Window".
    /// Used to close a UI window with a cutscene or animation.
    /// </summary>
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

    /// <summary>
    /// Disables all buttons under the homeMainMenuButtons GameObject.
    /// Used to prevent user input on menu buttons.
    /// </summary>
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

    public void LoadWorkScene()
    {
        SceneManager.LoadScene("Work");
    }

    public void CloseWindowBool()
    {
        windowIsClosed = true;
    }
}
