using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;
    private ObjectiveScript objectiveScript;
    private BackgroundColorManager bgcManager;
    private WorkMinigame workMinigame;

    public int workCounter = 0;

    // Global state variables
    // public bool hasTakenPills = false;
    // public bool hadPhoneArgument = false;
    // public bool triedToJump = false;
    // public bool gotCallAtBridge = false;
    // public bool choseToLive = false;

    [Header("Appartment Plant Sprites")]
    public Sprite plantHealthy;
    public Sprite plantMedium;
    public Sprite plantPoor;
    public Sprite plantDead;
    private GameObject plantObject;
    private SpriteRenderer plantRenderer;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        objectiveScript = GameObject.Find("ObjectiveManager").GetComponent<ObjectiveScript>();
        bgcManager = GameObject.Find("BackgroundColor").GetComponent<BackgroundColorManager>();
    }

    // Increases workCounter and sets backgroundcolor depending on work day
    public void GoToWork()
    {
        workCounter++;

        switch (workCounter)
        {
            case 1:
                Debug.Log("Day 1 of work");
                bgcManager.SetBackgroundColor255(255, 150, 30);
                break;
            case 2:
                Debug.Log("Day 2 of work");
                bgcManager.SetBackgroundColor255(209, 209, 100);
                break;
            case 3:
                Debug.Log("Day 3 of work");
                bgcManager.SetBackgroundColor255(195, 195, 131);
                break;
            case 4:
                bgcManager.SetBackgroundColor255(116, 163, 192);
                break;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Appartment")
        {
            UpdatePlantSprite();
        }
        if (scene.name == "Work")
        {
            workMinigame = GameObject.Find("WorkMinigameManager").GetComponent<WorkMinigame>();
            IncreaseWorkDifficulty();
        }
    }

    private void UpdatePlantSprite()
    {
        plantObject = GameObject.Find("Plant");
        plantRenderer = plantObject.GetComponent<SpriteRenderer>();

        if (plantRenderer == null || objectiveScript == null) return;

        switch (workCounter)
        {
            case 1:
                plantRenderer.sprite = plantMedium;
                break;
            case 2:
                plantRenderer.sprite = plantPoor;
                break;
            case 3:
                plantRenderer.sprite = plantDead;
                break;
            case 4:
                plantRenderer.sprite = plantDead;
                break;
        }
    }

    private void IncreaseWorkDifficulty()
    {
        switch (workCounter)
        {
            case 1:
                workMinigame.fillSpeed = 0.1f;
                break;
            case 2:
                workMinigame.fillSpeed = 0.05f;
                break;
            case 3:
                workMinigame.fillSpeed = 0.01f;
                break;
        }
    }

    //     // Called after player finishes a work day
    //     public void CompleteWorkDay()
    //     {

    //         // if (workCounter < 3)
    //         // {
    //         //     LoadScene("Room_Morning");
    //         // }
    //         // else if (workCounter == 3)
    //         // {
    //         //     LoadScene("ClubParty");
    //         // }
    //         // else if (workCounter == 4)
    //         // {
    //         //     hasTakenPills = true;
    //         //     LoadScene("ClubPills");
    //         // }
    //     }

    //     // Transition to the bridge scene and register player choice
    //     public void ReachBridge()
    //     {
    //         LoadScene("Bridge");
    //     }

    //     public void ChooseToJump()
    //     {
    //         triedToJump = true;
    //         choseToLive = false;
    //         LoadScene("Bridge_Aftermath");
    //     }

    //     public void ChooseToLive()
    //     {
    //         triedToJump = true;
    //         choseToLive = true;
    //         LoadScene("Bridge_Aftermath");
    //     }

    //     // Transition utility
    //     public void LoadScene(string sceneName)
    //     {
    //         SceneManager.LoadScene(sceneName);
    //     }
}
