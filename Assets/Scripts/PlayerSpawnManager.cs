using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : MonoBehaviour
{
    private GameObject player;

    public Transform fromAppartmentSpawn;
    public Transform fromWorkSpawn;
    public Transform fromOutsideToAppartment;

    private CameraFollow cameraFollow;

    void Start()
    {
        cameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();

        string lastExit = SceneTransitionManager.Instance.lastExitDoor;

        if (player == null)
            player = GameObject.FindWithTag("Player");

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Outside")
        {
            if (lastExit == "WorkDoor")
            {
                player.transform.position = fromWorkSpawn.position;
            }
            else if (lastExit == "AppartmentDoor")
            {
                player.transform.position = fromAppartmentSpawn.position;
            }
        }
        else if (sceneName == "Appartment")
        {
            if (lastExit == "AppartmentDoorOutside")
            {
                player.transform.position = fromOutsideToAppartment.position;
            }
        }
        cameraFollow.SetCameraPosition();
    }
}
