using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;
    private BackgroundColorManager bgcManager;
    private WorkMinigame workMinigame;

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

    void Start()
    {
        bgcManager = GameObject.Find("BackgroundColor").GetComponent<BackgroundColorManager>();
    }

}
