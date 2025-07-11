using UnityEngine;
using UnityEngine.UI;

public class MiniCamController : MonoBehaviour
{
	public static MiniCamController singleton;

	[Header("Tracking settings")]
	public Vector3 offset = Vector3.zero;
	public float distanceToTarget = 5f;
	public GameObject targetObject;		// to track the target object to outside use

	[Header("Zoom settings")]
	public float zoomSpeed = 2f;
	public float minDistance = 2f;
	public float maxDistance = 20f;

	[Header("UI control")]
	public Slider horizontalSlider;
	public Slider verticalSlider;
	public GameObject slidersGroup;
	public RawImage miniCamRawImage;

	[Header("Layer control")]
	public string renderLayer = "MiniCamLayer";

	private Transform target;
	private bool isTracking = false;
	private float horizontalAngle = 0f;
	private float verticalAngle = 0f;
	private Camera miniCam;

	void Awake()
	{
		singleton = this;
		DontDestroyOnLoad(gameObject);

	}
	void Start()
	{
		miniCam = GetComponent<Camera>();
		miniCam.enabled = false;

		if (miniCamRawImage != null)
			miniCamRawImage.enabled = false;

		if (slidersGroup != null)
			slidersGroup.SetActive(false);
	}

	void Update()
	{
		if (isTracking && target != null)
		{
			targetObject = target.gameObject;
			float scrollInput = Input.GetAxis("Mouse ScrollWheel");
			if (Mathf.Abs(scrollInput) > 0.01f)
			{
				distanceToTarget -= scrollInput * zoomSpeed;
				distanceToTarget = Mathf.Clamp(distanceToTarget, minDistance, maxDistance);
			}

			horizontalAngle = horizontalSlider.value * 180f;
			verticalAngle = verticalSlider.value * 90f;
			verticalAngle = Mathf.Clamp(verticalAngle, -80f, 80f);

			Quaternion rotation = Quaternion.Euler(verticalAngle, horizontalAngle, 0);
			Vector3 rotatedOffset = rotation * new Vector3(0, 0, -distanceToTarget);

			transform.position = target.position + offset + rotatedOffset;
			transform.LookAt(target.position + offset);
		}

		if (isTracking && (!target || target.gameObject == null))
		{
			StopTracking();
		}
	}

	public void StartTracking(Transform newTarget)
	{
		target = newTarget;
		isTracking = true;

		miniCam.enabled = true;

		if (miniCamRawImage != null)
			miniCamRawImage.enabled = true;

		if (slidersGroup != null)
			slidersGroup.SetActive(true);

		miniCam.cullingMask = LayerMask.GetMask(renderLayer);

		ResetSliders();
	}

	public void StopTracking()
	{
		isTracking = false;
		target = null;
		targetObject = null;

		miniCam.enabled = false;

		if (miniCamRawImage != null)
			miniCamRawImage.enabled = false;

		if (slidersGroup != null)
			slidersGroup.SetActive(false);
	}

	private void ResetSliders()
	{
		if (horizontalSlider != null) horizontalSlider.value = 0f;
		if (verticalSlider != null) verticalSlider.value = 0f;
	}
}