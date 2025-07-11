using UnityEngine;
using QuantumWorld;
using System.Collections;

public class Gate_C : MonoBehaviour
{
    // public
    public GameObject targetGate;   // the target gate linked to the control gate
        // control qubit status
    public GameObject controlQubit;
    public int controlStatus;
    public Vector3 controlDirection;

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
        // yield return new WaitForSeconds(duration * 0.5f);
        if (qubitGeneral == null) {
            isGateRunning = false;
            yield break;  // check if qubitGeneral is null
        }
        controlQubit = qubitGeneral.gameObject;
        controlStatus = qubitGeneral.status;
        controlDirection = qubitGeneral.arrowDirection;
        yield return new WaitForSeconds(duration * 0.9f);
    }
    // --------------------------------------------------------------------------

    void OnTriggerStay(Collider other)
    {
        if (isGateRunning) return;
        if (!isTheRightQubit(other)) return;
        GameObject qubit = other.gameObject;
        QubitGeneral qubitGeneral = qubit.GetComponent<QubitGeneral>();
        // Debug.Log("C gate:" + other.gameObject.name + ": get QubitGeneral");
        float duration = 4/(QuGameManager.singlton.quMovingSpeed*2);    // 4/2 = 2s
        if (this.gameObject.GetComponent<GateGeneral>().isGateReady || QuGameManager.singlton.launchMode == true)
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
