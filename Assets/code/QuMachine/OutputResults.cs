using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OutputResults : MonoBehaviour
{   
    public TextMeshPro[] outputTextsObject;   // amount of 0, 0/1, 1
    public GameObject[] resultObjects; // Qubit prefab

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void OnEnable()
    {
        // Debug.Log("InputObject - Onable");
    }
    void SetResultObjects(bool isActive)
    {
        foreach (var resultObject in resultObjects)
        {
            resultObject.SetActive(isActive);
        }
    }

    void Awake()
    {
        
    }
    void Start()
    {
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

        if (QuGameManager.singlton.launchMode == false)
        {
            SetResultObjects(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (QuGameManager.singlton.launchMode == false)
        {
            return;
        }
        if (other.gameObject.CompareTag("Qubit"))
        {   
            // Debug.Log("OutputResults - OnTriggerEnter");
            int qubitStatus = other.gameObject.GetComponent<QubitGeneral>().status;
            if (qubitStatus == 0)
            {
                resultObjects[0].SetActive(true);
                outputTextsObject[0].text = (int.Parse(outputTextsObject[0].text) + 1).ToString();
            }
            else if (qubitStatus == 1)
            {
                resultObjects[2].SetActive(true);
                outputTextsObject[2].text = (int.Parse(outputTextsObject[2].text) + 1).ToString();
            }
            else
            {
                resultObjects[1].SetActive(true);
                outputTextsObject[1].text = (int.Parse(outputTextsObject[1].text) + 1).ToString();
            }
            Destroy(other.gameObject);
            SoundManager.singleton.PlaySFX("qubit_arrived");
        }
    }

    public void OnLaunch()
    {   
        if (QuGameManager.singlton.launchMode == false)
        {
        outputTextsObject[0].text = "0";
        outputTextsObject[1].text = "0";
        outputTextsObject[2].text = "0";
        SetResultObjects(false);
        }
    }
}
