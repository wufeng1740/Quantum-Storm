using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using System.Collections;

[System.Serializable]
public class TargetInfo
{
	public string targetName;
	public string title;
	[TextArea(3, 10)]
	public string description;
	public Texture2D image;
	public VideoClip videoClip;
}

public class InfoPanelManager : MonoBehaviour
{
	[Header("Configure the target information list")]
	public List<TargetInfo> targetInfoList;

	[Header("UI")]
	public GameObject infoPanel;
	public CanvasGroup infoPanelGroup;
	public RawImage imageArea;
	public VideoPlayer videoPlayer;
	public TextMeshProUGUI titleText;
	public TextMeshProUGUI descriptionText;
	public Canvas canvas;

	private Dictionary<string, TargetInfo> infoMap;
	private float holdTime = 0f;
	private float threshold = 0.2f;
	private bool infoPanelShown = false;
	private GameObject currentTarget;
	private Coroutine fadeCoroutine;
	private string currentName;

	void Start()
	{
		infoMap = targetInfoList.ToDictionary(info => info.targetName);
		infoPanel.SetActive(false);
		infoPanelGroup.alpha = 0f;
		infoPanelGroup.interactable = false;
		infoPanelGroup.blocksRaycasts = false;

		if (videoPlayer.targetTexture != null)
			videoPlayer.targetTexture.Release();

		videoPlayer.Stop();
	}

	void Update()
	{
		if (Input.GetMouseButton(1))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				GameObject obj = hit.collider.gameObject;
				// Debug.Log("Hit Object: " + obj.name);
				if (obj.GetComponent<GateGeneral>() == null) return;

				currentName = obj.GetComponent<GateGeneral>().gateName;
				// Debug.Log($"Current Name: {currentName}");
				
				if (currentTarget == obj)
				{
					holdTime += Time.deltaTime;
				}
				else
				{
					holdTime = 0f;
					currentTarget = obj;
					infoPanelShown = false;
				}

				if (holdTime >= threshold && !infoPanelShown && infoMap.ContainsKey(currentName))
				{
					ShowInfoPanelForName(currentName, obj.transform);
					infoPanelShown = true;
				}
			}
			else
			{
				holdTime = 0f;
				currentTarget = null;
			}
		}

		if (Input.GetMouseButtonUp(1))
		{
			holdTime = 0f;
			infoPanelShown = false;
			HideInfoPanel();
		}
	}

	void ShowInfoPanelForName(string name, Transform targetTransform)
	{
		if (!infoMap.ContainsKey(name))
		{
			Debug.LogWarning($"No TargetInfo found for object name: {name}");
			return;
		}

		// set the info panel
		TargetInfo info = infoMap[name];
		infoPanel.SetActive(true);
		if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
		fadeCoroutine = StartCoroutine(FadePanel(true));
		if (info.videoClip != null)
		{
			videoPlayer.clip = info.videoClip;
			videoPlayer.Play();
			imageArea.texture = videoPlayer.targetTexture;
		}
		else
		{
			if (videoPlayer.isPlaying)
				videoPlayer.Stop();
			imageArea.texture = info.image;
		}
		titleText.text = info.title;
		descriptionText.text = info.description;

		// set the position
		RectTransform panelRect = infoPanel.GetComponent<RectTransform>();
		LayoutRebuilder.ForceRebuildLayoutImmediate(panelRect);
		Vector3 screenPos = Camera.main.WorldToScreenPoint(targetTransform.position);
		if (screenPos.z < 0)
		{
			infoPanel.SetActive(false);
			return;
		}

		float panelWidth = panelRect.rect.width;
		float panelHeight = panelRect.rect.height;
		float screenW = Screen.width;
		float screenH = Screen.height;

		Vector2 offset = new Vector2(panelWidth+ 30f, panelHeight + 30f);
		bool showAbove = screenPos.y + panelHeight + offset.y > screenH;

		// 动态 pivot 设置
		Vector2 pivot = panelRect.pivot;
		pivot.y = showAbove ? 0f : 1f;
		panelRect.pivot = pivot;

		screenPos.y += showAbove ? -offset.y : offset.y;

		// 限制在屏幕内
		float clampedX = Mathf.Clamp(screenPos.x, panelWidth * panelRect.pivot.x, screenW - panelWidth * (1 - panelRect.pivot.x));
		float clampedY = Mathf.Clamp(screenPos.y, panelHeight * panelRect.pivot.y, screenH - panelHeight * (1 - panelRect.pivot.y));

		panelRect.position = new Vector3(clampedX, clampedY, 0f);
	}



	void HideInfoPanel()
	{
		if (videoPlayer.isPlaying)
			videoPlayer.Stop();
		if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
		fadeCoroutine = StartCoroutine(FadePanel(false));
	}

	IEnumerator FadePanel(bool fadeIn)
	{
		float duration = 0.25f;
		float elapsed = 0f;
		float start = infoPanelGroup.alpha;
		float end = fadeIn ? 1f : 0f;

		infoPanelGroup.interactable = fadeIn;
		infoPanelGroup.blocksRaycasts = fadeIn;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			infoPanelGroup.alpha = Mathf.Lerp(start, end, elapsed / duration);
			yield return null;
		}

		infoPanelGroup.alpha = end;
		if (!fadeIn) infoPanel.SetActive(false);
	}
}