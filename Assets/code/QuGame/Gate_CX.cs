using UnityEngine;
using QuantumWorld;
using System.Collections;

public class Gate_CX : MonoBehaviour
{
    // public
    public GameObject controlGate;

    // private
    private bool isGateRunning = false; 
    // private int targetStatus;
    // private Vector3 targetDirection;

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
        yield return new WaitForSeconds(duration * 0.2f);
        if (qubitGeneral == null) {
            isGateRunning = false;
            yield break;  // check if qubitGeneral is null
        }
        int controlStatus = controlGate.GetComponent<Gate_C>().controlStatus;
        Vector3 controlDirection = controlGate.GetComponent<Gate_C>().controlDirection;
        int targetStatus = qubitGeneral.status;
        // Vector3 targetDirection = qubitGeneral.arrowDirection;
        // Debug.Log("Gate_CX: control: " + controlGate.GetComponent<Gate_C>().controlQubit.name + "=" +controlStatus + ", target: " + qubitGeneral.name + "=" + targetStatus);
        // Debug.Log("controlStatus: " + controlStatus + ", targetStatus: " + targetStatus);
        if (controlStatus == 0)
        {
            switch (targetStatus)
            {
                case -2:
                    targetStatus = -2;
                    break;
                case -1:
                    targetStatus = -1;
                    break;
                case 0:
                    targetStatus = 0;
                    break;
                case 1:
                    targetStatus = 1;
                    break;
                default:
                    break;
            }
        }
        else if (controlStatus == 1)
        {
            // Debug.Log("controlStatus: " + controlStatus + ", targetStatus: " + targetStatus);
            switch (targetStatus)
            {
                case -2:
                    targetStatus = -2;
                    break;
                case -1:
                    qubitGeneral.RotateArrow(QuAxis.X, 180, duration * 0.8f);
                    qubitGeneral.SetFromArrowObject();
                    break;
                case 0:
                    targetStatus = 1;
                    qubitGeneral.RotateArrow(QuAxis.X, 180, duration * 0.8f);
                    qubitGeneral.SetFromStatusAndArrowDirection(targetStatus);
                    break;
                case 1:
                    targetStatus = 0;
                    qubitGeneral.RotateArrow(QuAxis.X, 180, duration * 0.8f);
                    qubitGeneral.SetFromStatusAndArrowDirection(targetStatus);
                    break;
                default:
                    break;
            }

        }
        else if (controlStatus == -1)
        {
            switch (targetStatus)
            {
                case -2:
                    targetStatus = -2;
                    break;
                case -1:
                    targetStatus = -2;  // to be updated
                    qubitGeneral.SetFromStatusAndArrowDirection(targetStatus);
                    break;
                case 0:
                    targetStatus = -1;
                    qubitGeneral.SetFromStatusAndArrowDirection(targetStatus, controlDirection);
                    QuEntanglementManager.singlton.AddEntanglement(controlGate.GetComponent<Gate_C>().controlQubit, qubitGeneral.gameObject);
                    break;
                case 1:
                    targetStatus = -1;
                    qubitGeneral.SetFromStatusAndArrowDirection(targetStatus, -controlDirection);
                    QuEntanglementManager.singlton.AddEntanglement(controlGate.GetComponent<Gate_C>().controlQubit, qubitGeneral.gameObject);
                    break;
                default:    // 0 or 1
                    targetStatus = -1;
                    float rotationAngle = Vector3.SignedAngle(QuAxis.Z, controlDirection, QuAxis.X);
                    qubitGeneral.RotateArrow(QuAxis.X, rotationAngle, duration * 0.8f);
                    qubitGeneral.SetFromArrowObject();
                    QuEntanglementManager.singlton.AddEntanglement(controlGate.GetComponent<Gate_C>().controlQubit, qubitGeneral.gameObject);   // only consider classical entanglement
                    break;
            }
        }
        else    // controlStatus == -2
        {
            targetStatus = -2;
            qubitGeneral.SetFromStatusAndArrowDirection(targetStatus);
            // QuEntanglementManager.singlton.AddEntanglement(controlGate.GetComponent<Gate_C>().controlQubit, qubitGeneral.gameObject);
        }
    }

    private int GetControlGateStatus()
    {
    int controlStatus = 0;
    // check if all control gates are in the status
    try
    {
        controlStatus = controlGate.GetComponent<Gate_C>().controlStatus;
    }
    catch (System.Exception)
    {
        controlStatus = 0;
        throw;
    }
    return controlStatus;
    }
    // --------------------------------------------------------------------------

    void OnTriggerStay(Collider other)
    {
        if (isGateRunning) return;
        if (!isTheRightQubit(other)) return;
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
