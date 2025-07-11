using UnityEngine;
using UnityEngine.UI;

public class LoadingVisuals : MonoBehaviour
{
	public Image rotatingImage;
	public Image blinkingImage;

	public float rotationSpeed = 180f;
	public float blinkInterval = 0.8f;

	private float blinkTimer;

	private void Update()
	{
		if (rotatingImage != null)
		{
			rotatingImage.transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
		}

		if (blinkingImage != null)
		{
			blinkTimer += Time.deltaTime;
			if (blinkTimer >= blinkInterval)
			{
				blinkingImage.enabled = !blinkingImage.enabled;
				blinkTimer = 0f;
			}
		}
	}
}
