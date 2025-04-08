using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("volume", 1f);
        audioSource.volume = savedVolume;
    }

    public void SetVolume(float value)
    {
        audioSource.volume = value;
    }
}