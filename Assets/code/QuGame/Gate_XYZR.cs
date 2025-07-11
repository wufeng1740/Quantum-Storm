using UnityEngine;
using QuantumWorld;
using System.Collections;

public class Gate_XYZR : MonoBehaviour
{
    // function: rotate the ball along X axis 180 degrees in ? second
    // public
    [Tooltip("This is the Quantum World axis.")]
    public Vector3 qubitRotationAxis = new Vector3(1, 0, 0); // X axis
    public float qubitRotationAngle = 180f; // 180 degrees

    // private
    private bool isGateRunning = false; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool isTheRightQubit(Collider other)
    {
        // check if other is a qubit in running
        // Debug.Log(other.gameObject.name + ": is Checking");
        if (QuGameManager.singlton.launchMode == false) return false;
        if (other == null) return false;
        if (!other.gameObject.CompareTag("Qubit")) return false;
        return true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isTheRightQubit(other)) return;
        isGateRunning = false;
    }

    // --------------------------------------------------------------------------
    // Important: gate function is here
    IEnumerator HandleGateOperation(QubitGeneral qubitGeneral, float duration)
    {
        yield return new WaitForSeconds(duration * 0.1f);
        if (qubitGeneral == null) {
            isGateRunning = false;
            yield break;  // check if qubitGeneral is null
        }
        // Debug.Log("before - isEntangled: " + qubitGeneral.isEntangled);
        QuEntanglementManager.singlton.CheckAndRemove(qubitGeneral.gameObject);
        // Debug.Log("after  - isEntangled: " + qubitGeneral.isEntangled);
        qubitGeneral.RotateArrow(QuAxis.QuAxisToUnityAxis(qubitRotationAxis), qubitRotationAngle, duration * 0.8f);
    }
    // --------------------------------------------------------------------------

    void OnTriggerStay(Collider other)
    {
        if (!isTheRightQubit(other)) return;
        if (isGateRunning) return;
        GameObject qubit = other.gameObject;
        QubitGeneral qubitGeneral = qubit.GetComponent<QubitGeneral>();
        // Debug.Log(other.gameObject.name + ": get QubitGeneral");
        float duration = 4/(QuGameManager.singlton.quMovingSpeed*2);    // 4/2 = 2s
        if (this.gameObject.GetComponent<GateGeneral>().isGateReady)
        {
            // Debug.Log(other.gameObject.name + ": is ready to go");
            StartCoroutine(HandleGateOperation(qubitGeneral, duration));
            isGateRunning = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!isTheRightQubit(other)) return;
        isGateRunning = false;
    }
}
