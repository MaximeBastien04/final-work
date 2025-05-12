using TMPro;
using UnityEngine;

public class ObjectiveScript : MonoBehaviour
{
    public static ObjectiveScript Instance;
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

    public void ChangeObjective(string objective)
    {
        gameObject.transform.Find("Objective").GetComponent<TextMeshProUGUI>().text = objective;
    }
}
