using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject quitMenuCanvas;

    [Header("First Selected Options")]
    [SerializeField] private GameObject mainMenuFirst;
    [SerializeField] private GameObject quitMenuFirst;

    private bool isPaused;

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "HomeScene")
        {
            mainMenuCanvas.SetActive(false);
            quitMenuCanvas.SetActive(false);
        }
        else
        {
            mainMenuCanvas.SetActive(true);
            quitMenuCanvas.SetActive(false);
            EventSystem.current.SetSelectedGameObject(mainMenuFirst);
        }
    }

    void Update()
    {
        if (InputManager.Instance.MenuOpenCloseInput && SceneManager.GetActiveScene().name != "HomeScene")
        {
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                Unpause();
            }
        }
    }

    #region Pause/Unpause Functions
    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        GameObject.FindWithTag("Player").GetComponent<PlayerScript>().interactionBlocked = true;

        OpenMainMenu();
    }

    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1f;
        GameObject.FindWithTag("Player").GetComponent<PlayerScript>().interactionBlocked = false;

        CloseAllMenus();
    }

    #endregion

    #region Canvas Activations/Deactivations Functions
    private void OpenMainMenu()
    {
        mainMenuCanvas.SetActive(true);
        quitMenuCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(mainMenuFirst);
    }

    private void OpenQuitMenu()
    {
        mainMenuCanvas.SetActive(false);
        quitMenuCanvas.SetActive(true);

        EventSystem.current.SetSelectedGameObject(quitMenuFirst);
    }

    private void CloseAllMenus()
    {
        mainMenuCanvas.SetActive(false);
        quitMenuCanvas.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
    }
    #endregion

    #region Main Menu Buttons Actions

    public void OnQuitPress()
    {
        OpenQuitMenu();
    }

    public void OnResumePress()
    {
        Unpause();
    }

    #endregion

    #region Quit Menu Button Actions

    public void OnQuitNoPress()
    {
        OpenMainMenu();
    }

    public void OnQuitYesPress()
    {
        Application.Quit();
    }

    #endregion
}
