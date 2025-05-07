using TMPro;
using UnityEngine;

public class ObjectiveScript : MonoBehaviour
{
    public int workCounter = 0;

    public void ChangeObjective(string objective)
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = objective;
    }
}
