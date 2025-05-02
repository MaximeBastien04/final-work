using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class UIButtonScript : MonoBehaviour
{

    [Header("Menu's")]
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject IntroCutscene;
    
    public void StartIntroCutscene()
    {
        IntroCutscene.SetActive(true);
        IntroCutscene.GetComponent<PlayableDirector>().Play();
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene("Appartment");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenSettingsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void BackToMainMenu()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }
}
