using UnityEngine;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;
    public string lastExitDoor;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
