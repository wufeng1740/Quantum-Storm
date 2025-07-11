using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class StartSceneController : MonoBehaviour
{
	[Header("UI")]
	public TextMeshProUGUI pressText;
	public Image fadeOverlay;
	[Header("Setting")]
	public float fadeDuration = 1.5f;
	public string nextSceneName = "MainMenu";

	private bool transitioning = false;

	void Start()
	{
		StartCoroutine(BreathingEffect());

		Color c = fadeOverlay.color;
		c.a = 0;
		fadeOverlay.color = c;
	}

	void Update()
	{
		if (!transitioning && Input.anyKeyDown)
		{
			transitioning = true;
			StartCoroutine(FadeAndLoadScene());
		}
	}

	IEnumerator BreathingEffect()
	{
		float t = 0;
		while (!transitioning)
		{
			t += Time.deltaTime;
			float alpha = Mathf.Abs(Mathf.Sin(t * 2f));
			Color c = pressText.color;
			c.a = alpha;
			pressText.color = c;
			yield return null;
		}
	}

	IEnumerator FadeAndLoadScene()
	{
		float elapsed = 0f;
		Color c = fadeOverlay.color;

		while (elapsed < fadeDuration)
		{
			elapsed += Time.deltaTime;
			c.a = Mathf.Clamp01(elapsed / fadeDuration);
			fadeOverlay.color = c;
			yield return null;
		}

		SceneManager.LoadScene(nextSceneName);
	}
}
