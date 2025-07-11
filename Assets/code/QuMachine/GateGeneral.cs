using UnityEngine;
using System.Collections;

public class GateGeneral : MonoBehaviour
{
    // public
    public bool isGateReady = false;
    public string gateName = "?";           // use to control gate name
    public TMPro.TextMeshPro[] gateText;    // use to control gate text


    // private
    private GameObject qubitUsed;
    private GameObject qubit;
    private QubitGeneral qubitGeneral;
    private float qubitOriginalSpeed;
    private float qubitOriginalScale;
    private bool isQubitStopped = false; // 防止重复启动协程
    private float qubitScale = 1.5f;
    private float qubitTouchingGateDistance = 4f;    // distance between qubit first and last touch gate
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var text in gateText)
        {
            text.text = gateName;
        }
    }

    private void ResetGateGeneral()
    {
        isGateReady = false;
        qubit = null;
        qubitGeneral = null;
        qubitOriginalSpeed = 0;
        qubitOriginalScale = 0;
        isQubitStopped = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (QuGameManager.singlton.launchMode == false) return;
        if (other.gameObject.tag == "Qubit" && other.gameObject != qubitUsed)
        {
            // isGateReady = false;
            qubit = other.gameObject;
            qubitGeneral = qubit.GetComponent<QubitGeneral>();
            // qubitOriginalScale = qubit.transform.localScale.x;
            qubitOriginalSpeed = qubitGeneral.movingSpeed;
            // qubitGeneral.movingSpeed = 0f;
            // qubitGeneral.movingSpeed = qubitOriginalSpeed * 2;
            isQubitStopped = false;
            // StartCoroutine(ResumeQubitPosition());
            StartQubitSnap();
        }
    }
    
    IEnumerator ResumeQubitPosition()
    {
        // Debug.Log("ResumeQubit");
        float waitingTime = qubitTouchingGateDistance/QuGameManager.singlton.quMovingSpeed;   // 5s, time of qubit in gate
        yield return new WaitForSeconds(waitingTime);
        // 恢复 Qubit 原始的移动速度
        if (qubit == null) {
            ResetGateGeneral();
            yield break; // 如果 Qubit 被销毁，退出协程
        }
        // qubit.transform.localScale = new Vector3(qubitOriginalScale, qubitOriginalScale, qubitOriginalScale);
        qubitGeneral.ForceResetQubitPositionAndSpeed();
        qubitUsed = qubit;
        isGateReady = false;
    }

    public void StartQubitSnap()
    {
        if (!isQubitStopped && qubit != null && qubit != qubitUsed)
        {
            StartCoroutine(SnapAndScaleQubit());
        }
    }

    private IEnumerator SnapAndScaleQubit()
    {
        // if (qubit == null || qubit == qubitUsed) {
        //     ResetGateGeneral();
        //     yield break; // 如果 Qubit 被销毁或已被使用，退出协程
        // }
        // Debug.Log("SnapAndScaleQubit");
        isQubitStopped = true;
        qubitGeneral.movingSpeed = 0;

        float duration = qubitTouchingGateDistance/2/(QuGameManager.singlton.quMovingSpeed*2);  // 动画时长 = 1.25s
        float elapsed = 0f;

        Vector3 startPos = qubit.transform.position;
        Vector3 endPos = transform.position;

        while (elapsed < duration)
        {
            if (qubit == null) {
                ResetGateGeneral();
                yield break; // 如果 Qubit 被销毁，退出协程
            }
            elapsed += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            qubit.transform.position = Vector3.Lerp(startPos, endPos, t);
            // qubit.transform.localScale = Vector3.Lerp(startScale, endScale, t);

            yield return new WaitForFixedUpdate();
        }

        // 确保精确到位
        if (qubit == null) {
            ResetGateGeneral();
            yield break; // 如果 Qubit 被销毁，退出协程
        }
        qubit.transform.position = endPos;
        // qubit.transform.localScale = Vector3.one * (qubitOriginalScale * qubitScale);
        isGateReady = true;     // gate is ready to implement gate function
        StartCoroutine(ResumeQubit());
    }

    IEnumerator ResumeQubit()
    {
        // Debug.Log("ResumeQubit");
        float waitingTime = qubitTouchingGateDistance/(QuGameManager.singlton.quMovingSpeed * 2);   // 2.5s, time of qubit stopping
        yield return new WaitForSeconds(waitingTime);   // waiting for gate function to be implemented
        // 恢复 Qubit 原始的移动速度
        if (qubit == null) {
            ResetGateGeneral();
            yield break; // 如果 Qubit 被销毁，退出协程
        }
        // qubit.transform.localScale = new Vector3(qubitOriginalScale, qubitOriginalScale, qubitOriginalScale);
        StartCoroutine(SendQubitOut());
        // qubitGeneral.movingSpeed = qubitOriginalSpeed*2;
        qubitUsed = qubit;
        isGateReady = false;
    }
    private IEnumerator SendQubitOut()
    {
        float duration = qubitTouchingGateDistance / 2 / (QuGameManager.singlton.quMovingSpeed * 2);  // 动画时长 = 1.25s
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + new Vector3(qubitTouchingGateDistance / 2, 0, 0);
        while (elapsed < duration)
        {
            if (qubit == null)
            {
                ResetGateGeneral();
                yield break; // 如果 Qubit 被销毁，退出协程
            }
            elapsed += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            qubit.transform.position = Vector3.Lerp(startPos, endPos, t);
            // qubit.transform.localScale = Vector3.Lerp(startScale, endScale, t);

            yield return new WaitForFixedUpdate();
        }
        // 确保精确到位
        if (qubit == null)
        {
            ResetGateGeneral();
            yield break; // 如果 Qubit 被销毁，退出协程
        }
        qubit.transform.position = endPos;
        qubitGeneral.movingSpeed = QuGameManager.singlton.quMovingSpeed; // 恢复原始速度
        ResetGateGeneral();
        yield break;
        // qubitGeneral.ForceResetQubitPositionAndSpeed(); // 确保位置和速度正确
    }
    // void OnTriggerExit(Collider other)
    // {
    //     if (QuGameManager.singlton.launchMode == false) return;
    //     if (other.gameObject.tag == "Qubit")
    //         {
    //             other.gameObject.GetComponent<QubitGeneral>().movingSpeed = QuGameManager.singlton.quMovingSpeed;
    //             ResetGateGeneral();
    //         }
    // }
}
