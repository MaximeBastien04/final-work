using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;

    // Global state variables
    public int workCounter = 0;
    public bool hasTakenPills = false;
    public bool hadPhoneArgument = false;
    public bool triedToJump = false;
    public bool gotCallAtBridge = false;
    public bool choseToLive = false;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Called after player finishes a work day
    public void CompleteWorkDay()
    {

        // if (workCounter < 3)
        // {
        //     LoadScene("Room_Morning");
        // }
        // else if (workCounter == 3)
        // {
        //     LoadScene("ClubParty");
        // }
        // else if (workCounter == 4)
        // {
        //     hasTakenPills = true;
        //     LoadScene("ClubPills");
        // }
    }

    // Transition to the bridge scene and register player choice
    public void ReachBridge()
    {
        LoadScene("Bridge");
    }

    public void ChooseToJump()
    {
        triedToJump = true;
        choseToLive = false;
        LoadScene("Bridge_Aftermath");
    }

    public void ChooseToLive()
    {
        triedToJump = true;
        choseToLive = true;
        LoadScene("Bridge_Aftermath");
    }

    // Transition utility
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
