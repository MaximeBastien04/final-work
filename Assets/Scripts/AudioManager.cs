using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("AudioClips")]
    public AudioClip lightSwitch;
    public AudioClip doorOpen;
    public AudioClip doorClose;
    public AudioClip drinkGlass;
    public AudioClip printer;
    public AudioClip alarmClock;

    private AudioManager Instance;
    void Awake()
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

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
