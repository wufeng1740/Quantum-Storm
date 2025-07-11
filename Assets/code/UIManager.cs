using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection;

public class UIManager : MonoBehaviour
{
	[Header("Panels")]
	public GameObject settingsPanel;

	[Header("Volume Control")]
	public Slider volumeSlider;

	private void Start()
	{
		if (settingsPanel != null)
			settingsPanel.SetActive(false);

		float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);

		if (volumeSlider != null)
		{
			volumeSlider.value = savedVolume;
			volumeSlider.onValueChanged.AddListener(SetVolume);
			SetVolume(savedVolume);
		}
	}

	public void OnHintButtonClicked()
	{
		Debug.Log("Hint button clicked.");
	}

	public void OnSettingsButtonClicked()
	{
		settingsPanel.SetActive(true);
	}

	public void OnCloseSettingsClicked()
	{
		settingsPanel.SetActive(false);
	}

	public void SetVolume(float volume)
	{
		AudioListener.volume = volume;

		if (SoundManager.singleton != null)
		{
			var soundMgrType = typeof(SoundManager);

			var musicField = soundMgrType.GetField("musicSource", BindingFlags.NonPublic | BindingFlags.Instance);
			var sfxField = soundMgrType.GetField("sfxSource", BindingFlags.NonPublic | BindingFlags.Instance);
			var loopField = soundMgrType.GetField("loopSource", BindingFlags.NonPublic | BindingFlags.Instance);

			var musicSource = musicField?.GetValue(SoundManager.singleton) as AudioSource;
			var sfxSource = sfxField?.GetValue(SoundManager.singleton) as AudioSource;
			var loopSource = loopField?.GetValue(SoundManager.singleton) as AudioSource;

			if (musicSource != null) musicSource.volume = volume;
			if (sfxSource != null) sfxSource.volume = volume;
			if (loopSource != null) loopSource.volume = volume;
		}
		else
		{
			Debug.LogWarning("SoundManager.singleton is null");
		}

		PlayerPrefs.SetFloat("MusicVolume", volume);
		PlayerPrefs.Save();
	}

	public static class LoadingData
	{
		public static string targetScene;
	}

	public void OnExitButtonClicked()
	{
		LoadingData.targetScene = "MainMenu";
		SceneManager.LoadScene("LoadingScene");
	}
}
