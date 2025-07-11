using UnityEngine;
using System.Linq;

public class SoundManager : MonoBehaviour
{
    public static SoundManager singleton;

    [Header("Sources")]
    [SerializeField] AudioSource musicSource;       // Source for background music
    [SerializeField] AudioSource sfxSource;         // Source for sound effects
    [SerializeField] private AudioSource loopSource;// Source for looping sound effects

    [Header("Clips")]
    public AudioClip musicClip;
    public AudioClip[] musicClips; // Optional: array of music clips for variety
    public AudioClip[] sfxClips;

    void Awake()
    {
        if (singleton == null) { singleton = this; DontDestroyOnLoad(gameObject); }
        PlayDefaultMusic();
        // Debug.Log("SoundManager initialized with music: " + musicClip.name + ", Time.timeScale: " + Time.timeScale + ", AudioSource.pitch: " + musicSource.pitch);
    }

    public void PlayDefaultMusic()  // Plays the default music clip on loop
    {
        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    /// <summary>
    /// Plays the specified music clip on loop.
    /// </summary>
    public void PlayMusic(string clipName)
    {
        var clip = musicClips.FirstOrDefault(c => c.name == clipName);
        if (clip != null)
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"Music clip '{clipName}' not found.");
        }
    }

    /// <summary>
    /// Plays the specified sound effect once.
    /// </summary>
    public void PlaySFX(string clipName)
    {
        var clip = sfxClips.FirstOrDefault(c => c.name == clipName);
        if (clip != null) sfxSource.PlayOneShot(clip);
    }


    /// <summary>
    /// Plays the specified clip on loop until stopped.
    /// </summary>
    public void PlayLoopSFX(string clipName)
    {
        var clip = sfxClips.FirstOrDefault(c => c.name == clipName);
        if (clip != null)
        {
            loopSource.clip = clip;
            loopSource.loop = true;
            loopSource.Play();
        }
    }

    /// <summary>
    /// Stops the looping sound effect.
    /// </summary>
    public void StopLoopSFX()
    {
        if (loopSource.isPlaying)
            loopSource.Stop();
    }
    
    // void Update()
    // {
    //     AudioSource[] sources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
    //     foreach(var src in sources)
    //     {
    //         if (src.isPlaying)
    //         {
    //             Debug.Log($"[AudioDebug] AudioSource {src.gameObject.name} pitch={src.pitch}, clip={src.clip?.name}");
    //         }
    //     }
    // }
}
