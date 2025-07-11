using UnityEngine;
using QuantumWorld;
using System.Collections;

public class Gate_M : MonoBehaviour
{
    // function: rotate the ball along X axis 180 degrees in ? second
    // public
    // [Tooltip("This is the Quantum World axis.")]
    // public Vector3 qubitRotationAxis = new Vector3(1, 0, 0); // X axis
    // public float qubitRotationAngle = 180f; // 180 degrees

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
        yield return new WaitForSeconds(duration * 0.5f);
        // qubitGeneral.RotateArrow(QuAxis.QuAxisToUnityAxis(qubitRotationAxis), qubitRotationAngle, duration * 0.8f);
        // Debug.Log(qubitGeneral.status + ": " + qubitGeneral.arrowDirection);
        if (qubitGeneral == null) {
            isGateRunning = false;
            yield break;  // check if qubitGeneral is null
        }
        int result = qubitGeneral.status;
        Random.InitState((int)(Time.time * 1000 + this.GetInstanceID()));
        // if status == -2, 0.5 for 0/1
        if (result == -2)
        {
            result = Random.value < 0.5f ? 0 : 1;
        }
        else if (result == -1)
        {
            float p0 = (qubitGeneral.arrowDirection.y+1f) / 2f;
            // Debug.Log("p0: " + p0);
            result = Random.value < p0 ? 0 : 1;
            MeasureEntangledQubit(qubitGeneral, result);
            QuEntanglementManager.singlton.Remove(qubitGeneral.gameObject);
        }
        qubitGeneral.SetFromStatusAndArrowDirection(result);
    }

    // deal with entanglement
        // if it's entangled, based on the control qubit's status, check arrow angle first, 
        // if arrow agnle the same, result is the same
        // if arrow agnle different, result is the opposite
    void MeasureEntangledQubit(QubitGeneral qubitGeneral, int result){
        // // check if target is entangled
        if (!qubitGeneral.isEntangled)
        {
            return;
        }

        // based on arrow angles, determine other entangled qubit's status
        int entanglementNumber = QuEntanglementManager.singlton.CheckQubitEntanglement(qubitGeneral.gameObject);
        // Debug.Log("Entangled qubit: " + qubitGeneral.gameObject.name + ", entangled with: " + string.Join(", ", QuEntanglementManager.singlton.entanglementList[entanglementNumber]));
        if (entanglementNumber == -1)
        {
            Debug.Log("Entangled qubit is not recorded");
            return;
        }
        foreach (GameObject q in QuEntanglementManager.singlton.entanglementList[entanglementNumber])
        {
            QubitGeneral otherQubitGeneral = q.GetComponent<QubitGeneral>();
            if (otherQubitGeneral == null) continue;
            if (otherQubitGeneral.gameObject == qubitGeneral.gameObject) continue;
            // if it's modified by other gate, removed it from entanglement
            // if (otherQubitGeneral.movingSpeed == 0) // temp
            // {
            //     QuEntanglementManager.singlton.CheckAndRemove(otherQubitGeneral.gameObject);
            // }
            
            if (qubitGeneral.arrowDirection == otherQubitGeneral.arrowDirection)
            {
                // Debug.Log("Entangled qubit: " + otherQubitGeneral.gameObject.name + ", status: " + result);
                otherQubitGeneral.SetFromStatusAndArrowDirection(result);
            }
            else
            {
                // Debug.Log("Entangled qubit: " + otherQubitGeneral.gameObject.name + ", status: " + (result == 0 ? 1 : 0));
                otherQubitGeneral.SetFromStatusAndArrowDirection(result == 0 ? 1 : 0);
            }
        }

        // remove entanglement
        QuEntanglementManager.singlton.RemoveEntanglementByNumber(entanglementNumber);
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
