using UnityEngine;

public class AlwaysFaceToCamera : MonoBehaviour
{
    [Header("Qubit settings")]
    [Tooltip("The qubit object it belongs to")]
    [SerializeField]
    private GameObject thisQubitObject;

    // [Header("Camera settings")]
    // [Tooltip("Main camera to look at")]
    // [SerializeField]
    private Camera mainCamera;
    // [Tooltip("Mini camera to look at")]
    // [SerializeField]
    private Camera miniCam;

    void Awake()
    {
        mainCamera = Camera.main;
        miniCam = GameObject.Find("MiniCam")?.GetComponent<Camera>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (miniCam == null)
        {
            Debug.LogWarning("MiniCam not found. Please ensure there is a camera named 'MiniCam' in the scene.");
        }
        if (mainCamera == null)
        {
            Debug.LogWarning("Main camera not found. Please ensure there is a camera tagged as 'MainCamera' in the scene.");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        if (MiniCamController.singleton != null && MiniCamController.singleton.targetObject == thisQubitObject)
        {
            transform.LookAt(miniCam.transform.position);
            transform.Rotate(0, 180, 0); // fix the rotation problem of facing to camera
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward); // face world Z+ direction
        }
        // else if (mainCamera != null)
        // {
        //     transform.LookAt(mainCamera.transform.position);
        //     // transform.Rotate(0, 180, 0);
        // }
        // else
        // {
        //     Debug.LogWarning("Main camera not found. Please ensure there is a camera tagged as 'MainCamera' in the scene.");
        // }
    }
}
