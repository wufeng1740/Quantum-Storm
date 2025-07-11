using UnityEngine;
using TMPro;

public class TestCube : MonoBehaviour
{
    public TextMeshPro[] outputTextsObject;   // amount of 0, 0/1, 1
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // outputTextsObject[0].text = "5";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (QuGameManager.singlton.launchMode == false)
        {
            return;
        }
        if (other.gameObject.CompareTag("Qubit")) {
            int qubitStatus = other.gameObject.GetComponent<QubitGeneral>().status;
            if (qubitStatus == 0)
            {
                outputTextsObject[0].text = (int.Parse(outputTextsObject[0].text) + 1).ToString();
            }
            else if (qubitStatus == 1)
            {
                outputTextsObject[2].text = (int.Parse(outputTextsObject[2].text) + 1).ToString();
            }
            else
            {
                outputTextsObject[1].text = (int.Parse(outputTextsObject[1].text) + 1).ToString();
            }
            // Debug.Log(other.gameObject.GetComponent<QubitGeneral>().status);
            Destroy(other.gameObject);
        }
    }

    public void onLaunch()
    {
        if (!QuGameManager.singlton.launchMode)
        {
            outputTextsObject[0].text = "0";
            outputTextsObject[1].text = "0";
            outputTextsObject[2].text = "0";
        }
    }
}
