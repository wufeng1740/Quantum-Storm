using UnityEngine;

public class ObjectClickHandler : MonoBehaviour
{
	public string targetTag = "Qubit";
	public MiniCamController miniCamController;

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider.CompareTag(targetTag))
				{
					miniCamController.StartTracking(hit.transform);
				}
			}
		}
	}
}
