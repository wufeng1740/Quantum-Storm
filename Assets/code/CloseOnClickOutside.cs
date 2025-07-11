using UnityEngine;
using UnityEngine.EventSystems;

public class CloseOnClickInside : MonoBehaviour, IPointerClickHandler
{
	public GameObject panel;
	public GameObject mainMenuPanel;

	public void OnPointerClick(PointerEventData eventData)
	{
		panel.SetActive(false);
		EnableMainMenuButtons();
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
}
