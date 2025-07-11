using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolDragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public RectTransform canvasRect;
	public Canvas canvas;
	public RectTransform dragTarget;

	private GameObject dragObject;
	private bool fromTargetArea = false;

	void Awake()
	{
		if (canvas == null)
			canvas = GetComponentInParent<Canvas>();
		if (canvasRect == null && canvas != null)
			canvasRect = canvas.GetComponent<RectTransform>();
		if (dragTarget == null)
		{
			GameObject found = GameObject.Find("TargetArea");
			if (found != null)
				dragTarget = found.GetComponent<RectTransform>();
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		dragObject = Instantiate(gameObject, canvas.transform);
		dragObject.name = "DraggedTool";
		dragObject.transform.SetAsLastSibling();

		RectTransform rt = dragObject.GetComponent<RectTransform>();
		rt.sizeDelta = GetComponent<RectTransform>().sizeDelta;
		rt.pivot = new Vector2(0.5f, 0.5f);
		rt.anchorMin = new Vector2(0.5f, 0.5f);
		rt.anchorMax = new Vector2(0.5f, 0.5f);
		rt.anchoredPosition = Vector2.zero;

		Destroy(dragObject.GetComponent<Button>());

		fromTargetArea = transform.parent == dragTarget;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (dragObject == null) return;

		Vector2 localPos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, eventData.pressEventCamera, out localPos);
		dragObject.transform.localPosition = localPos;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (dragObject == null) return;

		bool isOverTarget = RectTransformUtility.RectangleContainsScreenPoint(dragTarget, eventData.position, eventData.pressEventCamera);
		float maxOffset = 80f;

		Vector2 localPos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(dragTarget, eventData.position, eventData.pressEventCamera, out localPos);
		bool withinOffset = Vector2.Distance(localPos, Vector2.zero) <= maxOffset;

		if (isOverTarget || withinOffset)
		{
			Transform existing = dragTarget.Find("ActiveTool");
			if (existing != null) Destroy(existing.gameObject);

			dragObject.name = "ActiveTool";
			dragObject.transform.SetParent(dragTarget);
			dragObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

			ToolDragItem item = dragObject.GetComponent<ToolDragItem>();
			if (item != null)
			{
				item.canvas = canvas;
				item.canvasRect = canvasRect;
				item.dragTarget = dragTarget;
			}
		}
		else
		{
			if (fromTargetArea)
			{
				Transform existing = dragTarget.Find("ActiveTool");
				if (existing != null)
					Destroy(existing.gameObject);
			}

			Destroy(dragObject);
		}

		dragObject = null;
		fromTargetArea = false;
	}
}
