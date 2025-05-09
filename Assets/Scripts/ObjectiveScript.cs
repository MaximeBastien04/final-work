using TMPro;
using UnityEngine;

public class ObjectiveScript : MonoBehaviour
{
    public static ObjectiveScript Instance;
    public int workCounter = 0;
    public BackgroundColorManager bgcManager;
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
        bgcManager = GameObject.Find("BackgroundColor").GetComponent<BackgroundColorManager>();
    }

    public void ChangeObjective(string objective)
    {
        gameObject.transform.Find("Objective").GetComponent<TextMeshProUGUI>().text = objective;
    }


    public void GoToWork()
    {
        workCounter++;
        if (workCounter == 1)
        {
            Debug.Log("Day 1 of work");
            bgcManager.SetBackgroundColor255(255, 150, 30);
        }
        else if (workCounter == 2)
        {
            Debug.Log("Day 2 of work");
            bgcManager.SetBackgroundColor255(209, 209, 100);
        }

        else if (workCounter == 3)
        {
            Debug.Log("Day 3 of work");
            bgcManager.SetBackgroundColor255(195, 195, 131);
        }
    }
}
