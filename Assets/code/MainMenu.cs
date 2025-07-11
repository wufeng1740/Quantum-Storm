using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UIManager;

public class MainMenu : MonoBehaviour
{
	[Header("Panels")]
	public GameObject settingsPanel;
	public GameObject mainMenuPanel;
	public GameObject levelSelectPanel;

	[Header("Audio")]
	public AudioSource backgroundMusicSource;
	public AudioClip backgroundMusicClip;
	public AudioSource sfxSource;
	public AudioClip clickSoundClip;
	public Slider musicVolumeSlider;
	public Slider sfxVolumeSlider;

	[Range(0f, 1f)] public float musicVolume = 1f;
	[Range(0f, 1f)] public float sfxVolume = 1f;

	private void DisableMainMenuButtons()
	{
		CanvasGroup canvasGroup = mainMenuPanel.GetComponent<CanvasGroup>();
		if (canvasGroup == null)
			canvasGroup = mainMenuPanel.AddComponent<CanvasGroup>();
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
	}

	private void EnableMainMenuButtons()
	{
		CanvasGroup canvasGroup = mainMenuPanel.GetComponent<CanvasGroup>();
		if (canvasGroup != null)
		{
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;
		}
	}

	private void Start()
	{
		settingsPanel.SetActive(false);

		musicVolume = PlayerPrefs.GetFloat("MusicVolume", musicVolume);
		sfxVolume = PlayerPrefs.GetFloat("SFXVolume", sfxVolume);

		if (musicVolumeSlider != null)
		{
			musicVolumeSlider.minValue = 0f;
			musicVolumeSlider.maxValue = 1f;
			musicVolumeSlider.wholeNumbers = false;

			musicVolumeSlider.onValueChanged.RemoveAllListeners();
			musicVolumeSlider.value = musicVolume;
			musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
		}

		if (sfxVolumeSlider != null)
		{
			sfxVolumeSlider.minValue = 0f;
			sfxVolumeSlider.maxValue = 1f;
			sfxVolumeSlider.wholeNumbers = false;

			sfxVolumeSlider.onValueChanged.RemoveAllListeners();
			sfxVolumeSlider.value = sfxVolume;
			sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
		}

		ApplyVolumes();

		if (backgroundMusicSource != null && backgroundMusicClip != null)
		{
			backgroundMusicSource.clip = backgroundMusicClip;
			backgroundMusicSource.loop = true;
			backgroundMusicSource.Play();
		}
	}
	private void ApplyVolumes()
	{
		if (backgroundMusicSource != null)
			backgroundMusicSource.volume = musicVolume;
		if (sfxSource != null)
			sfxSource.volume = sfxVolume;

		PlayerPrefs.SetFloat("MusicVolume", musicVolume);
		PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
		PlayerPrefs.Save();
	}

	public void PlayClickSound()
	{
		if (sfxSource != null && clickSoundClip != null)
		{
			sfxSource.PlayOneShot(clickSoundClip);
		}
	}

	public void StartGame()
	{
		PlayClickSound();
		SaveManager.Delete();
		SceneManager.LoadScene("LoadingScene");
	}

	public void ContinueGame()
	{
		PlayClickSound();

		SaveData data = SaveManager.Load();

		if (data != null && !string.IsNullOrEmpty(data.lastSceneName))
		{
			string targetScene = MapContinueScene(data.lastSceneName);
			LoadingData.targetScene = targetScene;
			SceneManager.LoadScene("LoadingScene");
		}
		else
		{
			Debug.Log("No valid save file, please start a new game first");
		}
	}

	private string MapContinueScene(string savedScene)
	{
		switch (savedScene)
		{
			case "QuMachine": return "QuMachine";
			case "Level_01": return "story2";
			case "Level_02": return "story3";
			case "Level_03": return "Level_04";
			case "Level_04": return "story4";
			case "Level_05": return "story5";
			case "Level_06": return "story6";
			case "Level_07": return "story7";
			default: return "MainMenu";
		}
	}

	public void OpenLab()
	{
		PlayClickSound();
		SceneManager.LoadScene("QuMachine");
	}

	public void OpenLevelSelect()
	{
		PlayClickSound();
		levelSelectPanel.SetActive(true);
		DisableMainMenuButtons();
	}

	public void CloseLevelSelect()
	{
		PlayClickSound();
		levelSelectPanel.SetActive(false);
		EnableMainMenuButtons();
	}

	public void LoadLevel(string levelName)
	{
		PlayClickSound();
		LoadingData.targetScene = levelName;
		SceneManager.LoadScene("LoadingScene");
	}

	public void OpenSettings()
	{
		PlayClickSound();
		settingsPanel.SetActive(true);
		DisableMainMenuButtons();
	}

	public void CloseSettings()
	{
		PlayClickSound();
		settingsPanel.SetActive(false);
		EnableMainMenuButtons();
	}

	public void OnMusicVolumeChanged(float value)
	{
		musicVolume = value;
		ApplyVolumes();
	}

	public void OnSFXVolumeChanged(float value)
	{
		sfxVolume = value;
		ApplyVolumes();
	}


	public void QuitGame()
	{
		PlayClickSound();
		Application.Quit();
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
	}
}
