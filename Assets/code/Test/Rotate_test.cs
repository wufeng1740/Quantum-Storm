using UnityEngine;
using QuantumWorld;

public class RotateX : MonoBehaviour
{
    public QubitGeneral qubit;
    public Vector3 qubitRotationAxis = new Vector3(1, 0, 0); // X axis
    public float qubitRotationAngle = 180f; // 180 degrees
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateAlongX()
    {
        qubit.RotateArrow(new Vector3(1, 0, 0), 180f, 3f);
    }

    public void RotateAlongOtherAxis()
    {
        Debug.Log("Start: " + qubit.status + ", " + qubit.arrowDirection);
        qubit.RotateArrow(QuAxis.QuAxisToUnityAxis(qubitRotationAxis), qubitRotationAngle, 3f);
        // qubit.SetFromArrowObject();
        // Debug.Log("End: " + qubit.status + ", " + qubit.arrowDirection);
    }

    public void LogQubitInfo()
    {
        Debug.Log("Qubit: " + qubit.status + ", " + qubit.arrowDirection);
    }

}