using UnityEngine;

public class LoadingMusicPlayer : MonoBehaviour
{
	[Header("Audio")]
	public AudioSource musicSource;
	public AudioClip loadingMusicClip;

	[Range(0f, 1f)] public float musicVolume = 1f;

	private void Start()
	{
		if (musicSource != null && loadingMusicClip != null)
		{
			musicSource.clip = loadingMusicClip;
			musicSource.loop = true;
			musicSource.volume = musicVolume;
			musicSource.Play();
		}
	}

	private void Update()
	{
		if (musicSource != null)
			musicSource.volume = musicVolume;
	}
}
