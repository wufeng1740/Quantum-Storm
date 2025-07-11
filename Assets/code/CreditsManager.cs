using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsManager : MonoBehaviour
{
	[Header("UI References")]
	public ScrollRect scrollRect;
	public GameObject thanksText;
	public Image blackOverlay;
	public CanvasGroup scrollGroup;
	public CanvasGroup thanksGroup;
	public AudioSource audioSource;

	[Header("Timings")]
	public float fadeInDuration = 2f;
	public float scrollDelay = 1f;
	public float scrollDuration = 10f;
	public float fadeOutScrollDuration = 1.5f;
	public float fadeInThanksDuration = 1.5f;
	public float finalHoldDuration = 3f;
	public float finalFadeOutDuration = 2f;

	private void Start()
	{
		scrollRect.verticalNormalizedPosition = 1f;
		scrollGroup.alpha = 0f;
		scrollGroup.gameObject.SetActive(false);
		thanksGroup.alpha = 0f;
		thanksGroup.gameObject.SetActive(false);
		blackOverlay.color = new Color(0, 0, 0, 1);

		StartCoroutine(RunCreditsSequence());
	}

	IEnumerator RunCreditsSequence()
	{
		scrollGroup.gameObject.SetActive(true);
		yield return FadeImage(blackOverlay, 1f, 0f, fadeInDuration);

		yield return FadeCanvasGroup(scrollGroup, 0f, 1f, 1f);
		yield return new WaitForSeconds(scrollDelay);

		if (audioSource != null) audioSource.Play();
		yield return ScrollCredits();

		yield return FadeCanvasGroup(scrollGroup, 1f, 0f, fadeOutScrollDuration);
		scrollGroup.gameObject.SetActive(false);

		thanksGroup.gameObject.SetActive(true);
		yield return FadeCanvasGroup(thanksGroup, 0f, 1f, fadeInThanksDuration);
		yield return new WaitForSeconds(finalHoldDuration);

		yield return FadeImage(blackOverlay, 0f, 1f, finalFadeOutDuration);
		SceneManager.LoadScene("MainMenu");
	}

	IEnumerator ScrollCredits()
	{
		float elapsed = 0f;
		while (elapsed < scrollDuration)
		{
			elapsed += Time.deltaTime;
			float t = Mathf.Clamp01(elapsed / scrollDuration);
			scrollRect.verticalNormalizedPosition = 1f - t;
			yield return null;
		}
		scrollRect.verticalNormalizedPosition = 0f;
	}

	IEnumerator FadeCanvasGroup(CanvasGroup group, float from, float to, float duration)
	{
		float t = 0f;
		group.alpha = from;
		while (t < duration)
		{
			t += Time.deltaTime;
			group.alpha = Mathf.Lerp(from, to, t / duration);
			yield return null;
		}
		group.alpha = to;
	}

	IEnumerator FadeImage(Image image, float from, float to, float duration)
	{
		float t = 0f;
		Color color = image.color;
		while (t < duration)
		{
			t += Time.deltaTime;
			float a = Mathf.Lerp(from, to, t / duration);
			image.color = new Color(color.r, color.g, color.b, a);
			yield return null;
		}
		image.color = new Color(color.r, color.g, color.b, to);
	}
}
