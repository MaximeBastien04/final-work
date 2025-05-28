using UnityEngine;

/// <summary>
/// Manages playing audio clips using a single AudioSource.
/// Implements a singleton pattern to persist across scenes.
/// </summary>
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

    /// <summary>
    /// Plays the given audio clip once on the AudioSource.
    /// </summary>
    /// <param name="clip">The AudioClip to be played.</param>
    public void PlayClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
