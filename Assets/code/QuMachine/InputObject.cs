using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputObject : MonoBehaviour
{   
    private float timeToCopy = 10f;   // time of one gate = 5s
    private GameObject theQubit;
    // private GameObject theQubitToMove;
    private List<GameObject> theQubitToMoveList = new List<GameObject>();
    private int qubitCount = 0; // count the number of qubits in the machine

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        theQubit = null;
        timeToCopy = QuGameManager.singlton.timeToCopy;
        // Debug.Log("InputObject - Onable");
        Button launchButton = QuGameManager.singlton.launchButton;
        if (launchButton != null)
        {
            var buttonComponent = launchButton.GetComponent<Button>();
            if (buttonComponent != null)
            {
                // Debug.Log("InputObject - Onable - launchButton");
                buttonComponent.onClick.AddListener(OnLaunch);
            }
            else
            {
                Debug.LogError("LaunchButton does not have a Button component.");
            }
        }
        else
        {
            Debug.LogError("LaunchButton not found in the scene.");
        }
    }

    // QubitGeneral[] qubitsInMachine = new QubitGeneral[0];


    // Update is called once per frame
    void Update()
    {

    }


    public void OnLaunch()
    {
        // !launchMode, reset theQubit, theQubitToMoveList
        // Debug.Log("InputOBject - OnLaunch" + gameMode);
        if (QuGameManager.singlton.launchMode == false)
        {    // edit mode
            if (theQubit != null)
            {
                // theQubit.SetActive(true);
                // Debug.Log(theQubitToMoveList);
                foreach (var qubitToMove in theQubitToMoveList)
                {
                    if (qubitToMove != null)
                    {
                        // Debug.Log("Destroying " + qubitToMove.name);
                        Destroy(qubitToMove);
                    }
                }
                theQubitToMoveList = new List<GameObject>();
                theQubit = null;
            }
            return;
        }

        // launchMode, copy theQubit every 5 seconds
        // 直接查找并获取单个带有"Qubit"标签的游戏对象
        GameObject qubit = null;
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Qubit") && theQubitToMoveList.Contains(child.gameObject) == false)
            {
                qubit = child.gameObject;
                break;
            }
        }
        if (qubit != null)
        {
            theQubit = qubit;
            StartCoroutine(CopyTheQubitPeriodically(theQubit));
        }
        qubitCount = 0;

        // Collider[] hitColliders = Physics.OverlapSphere(transform.position, this.GetComponent<SphereCollider>().radius);
        // foreach (var hitCollider in hitColliders)
        // {   
        //     // Debug.Log(hitCollider.gameObject.name);
        //     if (hitCollider.gameObject.CompareTag("Qubit"))
        //     {   
        //         // Debug.Log("in ---> " + hitCollider.gameObject.name);
        //         theQubit = hitCollider.gameObject;
        //         // CopyTheQubit(theQubit);
        //         StartCoroutine(CopyTheQubitPeriodically(theQubit));
        //     }
        // }
    }

    private System.Collections.IEnumerator CopyTheQubitPeriodically(GameObject qubit)
    {
        // Debug.Log("CallCopyTheQubitPeriodically");
        qubit.name = "Qubit";
        while (QuGameManager.singlton.launchMode == true)
        {
            // Debug.Log("CallCopyTheQubitPeriodically - " + qubit.name);
            CopyTheQubit(qubit);
            yield return new WaitForSeconds(timeToCopy/QuGameManager.singlton.quMovingSpeed);
        }
    }

    // copy the qubit -> to move
    private void CopyTheQubit(GameObject qubit)
    {
        // theQubit.SetActive(true);
        GameObject theQubitToMove = Instantiate(qubit, qubit.transform.position, qubit.transform.rotation);
        theQubitToMove.transform.SetParent(this.transform);
        theQubitToMove.name = qubit.name + "_toMove" + qubitCount;
        qubitCount++;
        theQubitToMove.transform.localScale = qubit.transform.localScale * this.transform.localScale.x;
        theQubitToMove.GetComponent<QubitGeneral>().movingSpeed = QuGameManager.singlton.quMovingSpeed;
        theQubitToMove.GetComponent<QubitGeneral>().SetLaunchTimeAndPosition();
        theQubitToMoveList.Add(theQubitToMove);
        // theQubitToMove.SetActive(true);
        // theQubit.SetActive(false);
    }
}
