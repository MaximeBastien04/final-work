using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;
    public bool hasInteractedWithOldLady = false;
    public bool hasInteractedWithMeditationLady = false;
    public bool hasDrunkCoffee = false;
    public bool hasDrunkWater = false;
    public bool hasGivenChildIceCream = false;
    public bool hasGottenIceCream = false;
    public bool hasWorked = false;
    public bool hasPetDog = false;
    public bool hasLookedAtFlowers = false;


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
        
    }

}
