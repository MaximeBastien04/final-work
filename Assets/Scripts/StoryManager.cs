using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;

    public bool hasInteractedWithMeditationLady = false;

    public HashSet<string> completedInteractions = new HashSet<string>();

    private void Awake()
    {
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

    public bool HasCompleted(string interactionId)
    {
        return completedInteractions.Contains(interactionId);
    }

    public void MarkCompleted(string interactionId)
    {
        completedInteractions.Add(interactionId);
    }
}
