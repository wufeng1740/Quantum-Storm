using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using System.Linq;
using QuantumWorld;

public class LevelManager : MonoBehaviour
{
    [Header("Game Level Settings")]
    [Tooltip("current level number")]
    public int currentLevel = 1;
    [Tooltip("next scene to jump to")]
    public String nextScene = "";
    [Tooltip("time to wait before jumping to next scene")]
    // public float waitBeforeNextScene = 0f;

    [Header("Game API (don't change)")]
    public TMPro.TextMeshProUGUI winText;
    public TextMeshPro[] outputTextObjects;   // amount of 0, 0/1, 1

    public List<TextMeshPro[]> AllOutputTextObjects = new List<TextMeshPro[]>();
    public bool isAchieved;
    public GameObject continueButton;

    // private
    private Dictionary<int, Func<bool>> levelActions;
    private Dictionary<int, string> levelHints;
    private bool isResultChanged = false;
    private bool isFailed = false;
    private string resultString = "";
    private int resultSum = 0;
    private GameObject hintPanel;
    private TMPro.TextMeshProUGUI hintText;
    private string galobalInstruction;
    private GameObject quGameManagerObject;
    private List<GameObject> allGates = new List<GameObject>();

    // private

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isAchieved = false;
        // define the level actions
        levelActions = new Dictionary<int, Func<bool>>
        {
            { 0, Level_00 },
            { 1, Level_01 },
            { 2, Level_02 },
            { 3, Level_03 },
            { 4, Level_04 },
            { 5, Level_05 },
            { 6, Level_06 },
            { 7, Level_07 },
        };

        // Find the "AchievedText" GameObject
        Transform parent = GameObject.Find("Canvas_Main").transform;
        Transform achievedText = parent.Find("AchievedText");  // this way to find inactive object
        if (achievedText != null)
        {
            // Get the TextMeshProUGUI component
            winText = achievedText.GetComponent<TMPro.TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("AchievedText GameObject not found!");
        }

        // Initialize the textWatcher
        for (int i = 0; i < QuGameManager.singlton.qubitMachineNumnber; i++)
        {
            outputTextObjects = QuGameManager.singlton.qubitMachines[i].GetComponent<QuMachine>().outputTextObjects;
            AllOutputTextObjects.Add(outputTextObjects);
            foreach (var textObj in outputTextObjects)
            {
                var watcher = textObj.gameObject.AddComponent<TextWatcher>();
                watcher.OnTextChanged += () => { isResultChanged = true; };
            }
        }

        // Initialize the hintText
        Transform hintPanelTransform = parent.Find("HintPanel");
        if (hintPanelTransform != null)
        {
            hintPanel = hintPanelTransform.gameObject;
            Transform hintTextTransform = hintPanelTransform.Find("HintDescription");
            if (hintTextTransform == null)
            {
                Debug.LogError("HintDescription GameObject not found in HintPanel.");
                return;
            }
            hintText = hintTextTransform.GetComponent<TMPro.TextMeshProUGUI>();
            if (hintText == null)
            {
                Debug.LogError("HintDescription does not have a TextMeshProUGUI component.");
            }
        }
        else
        {
            Debug.LogError("HintDescription GameObject not found!");
        }

        // Find the QuGameManager object
        quGameManagerObject = GameObject.Find("QuGameManager");
        if (quGameManagerObject == null)
        {
            Debug.LogError("QuGameManager object not found in the scene.");
        }

        // Add listener to the launch button
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

        // Initialize the level hints
        levelHints = new Dictionary<int, string>
        {
            { 0, " - Goal: this is for your testing."},
            { 1, " - Goal: get 3 qubits of 0 and 3 qubits of 1, no qubit of 0/1." },
            { 2, " - Goal: get 3 qubits of 1, no qubit of 0 or 0/1" },
            { 3, " - Goal: get 3 qubits of 0 and 3 qubits of 1. no qubit of 0/1" },
            { 4, " - Goal: get 3 qubits of 1, no qubit of 0 or 0/1" },
            { 5, " - Requirement: the terminal only accepts the same qubit of 0 or 1 each time, like [00] or [11]\n - Goal: get 3 qubits of 0 and 3 qubits of 1 in both output, no qubit of 0/1;" },
            { 6, " - Requirement: use 4 X gates & 4 CX gates\n - Goal: output [1011] each time." },
            { 7, " - Requirement: use all gates for at least 1 time\n - Gaol: output [1111] each time." },
        };
        galobalInstruction = "\n\nGlobal Instruction\n - Right click and hold on any Gate (square) to get instruction of the Gate.\n - Drag and drop the Qubit or Gate to change its position.\n - Click on the Launch button to run the quantum circuit.";
    }

    // Update is called once per frame
    void Update()
    {
        if (QuGameManager.singlton.launchMode == false) return;
        if (!isResultChanged) return;
        // Debug.Log("LevelManager - Update: isResultChanged = " + isResultChanged);
        // int[][] result = GetOutputResults();
        // Debug.Log("Level_06: result = " + string.Join(", ", result.Select(r => string.Join(",", r))));
        if (isFailed) return;
        if (isAchieved) return;

        // check if the level is achieved
        if (levelActions.ContainsKey(currentLevel))
        {
            isAchieved = levelActions[currentLevel].Invoke();
        }
        else
        {
            isAchieved = levelActions[1].Invoke();
        }
        if (!isFailed && isAchieved)
        {
            AfterAchieved();
        }

        isResultChanged = false;
    }

    public void OnLaunch()
    {
        // Reset all
        if (QuGameManager.singlton.launchMode == false)
        {
            winText.gameObject.SetActive(false);
            return;
        }
        else  // launchMode == true
        {
            allGates = GetAllGates();
        }
        ResetAll();
    }

    public void OnContinue()
    {
        // go to next scene
        if (nextScene != "")
        {
            // StartCoroutine(WaitAndLoadScene());
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
        }
        else
        {
            Debug.Log("No next scene specified.");
        }
    }

    private int[][] GetOutputResults()
    {
        resultSum = 0;
        int[][] result = new int[QuGameManager.singlton.qubitMachineNumnber][];
        for (int i = 0; i < QuGameManager.singlton.qubitMachineNumnber; i++)
        {
            result[i] = new int[3];
            result[i][0] = int.Parse(AllOutputTextObjects[i][0].text);
            result[i][1] = int.Parse(AllOutputTextObjects[i][1].text);
            result[i][2] = int.Parse(AllOutputTextObjects[i][2].text);
            resultSum += result[i][0] + result[i][1] + result[i][2];
        }
        return result;
    }

    private List<GameObject> GetAllGates()
    {   
        List<GameObject> foundObjects = new List<GameObject>();
        // 获取父物体下所有子物体（包括自己）
        Transform[] allChildren = quGameManagerObject.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in allChildren)
        {
            if (child.gameObject.CompareTag("Gate"))
            {
                foundObjects.Add(child.gameObject);
            }
        }
        return foundObjects;
    }

    private Dictionary<string, int> GetGateCount()
    {
        if (allGates.Count == 0)
        {
            return null;
        }
        Dictionary<string, int> gateCount = new Dictionary<string, int>();
        string[] gateList = QuGate.gateList;
        foreach (string gateName in gateList)
        {
            gateCount[gateName] = 0; // Initialize all gates to 0
        }

        foreach (GameObject gate in allGates)
        {
            string gateName = gate.GetComponent<GateGeneral>().gateName;
            if (gateCount.ContainsKey(gateName))
            {
                gateCount[gateName]++;
            }
        }
        return gateCount;
    }

    private void ResetAll()
    {
        isFailed = false;
        isAchieved = false;
        resultSum = 0;
        winText.gameObject.SetActive(false);
        resultString = "";
    }

    // --------------------------------------------------------
    // write all achievement conditions here
    // result[i][0] = amount of 0
    // result[i][1] = amount of 0/1
    // result[i][2] = amount of 1

    private bool Level_00()
    {
        // this one is just for testing
        return false;
    }
    private bool Level_01()
    {
        int[][] result = GetOutputResults();
        for (int i = 0; i < result.Length; i++)
        {
            if (result[i][0] >= 3 && result[i][1] == 0 && result[i][2] >= 3)
            {
                return true;
            }
        }
        return false;
    }

    private bool Level_02()
    {
        int[][] result = GetOutputResults();
        for (int i = 0; i < result.Length; i++)
        {
            if (result[i][0] == 0 && result[i][1] == 0 && result[i][2] >= 3)
            {
                return true;
            }
        }
        return false;
    }

    private bool Level_03()
    {
        int[][] result = GetOutputResults();
        for (int i = 0; i < result.Length; i++)
        {
            if (result[i][0] >= 3 && result[i][1] == 0 && result[i][2] >= 3)
            {
                return true;
            }
        }
        return false;
    }

    private bool Level_04()
    {
        int[][] result = GetOutputResults();
        for (int i = 0; i < result.Length; i++)
        {
            if (result[i][0] == 0 && result[i][1] == 0 && result[i][2] >= 3)
            {
                return true;
            }
        }
        return false;
    }

    private bool Level_05()
    {
        if (isFailed) return false;
        int[][] result = GetOutputResults();
        if (resultSum % 2 == 0)
        {
            if (result[0][0] != result[1][0] || result[0][1] != result[1][1] || result[0][2] != result[1][2] || result[0][1] > 0 || result[1][1] > 0)
            {
                // Debug.Log("Level_05: resultSum = " + resultSum + "-> isFailed = true");
                // Debug.Log("Level_05: result[0] = " + result[0][0] + ", " + result[0][1] + ", " + result[0][2]);
                // Debug.Log("Level_05: result[1] = " + result[1][0] + ", " + result[1][1] + ", " + result[1][2]);
                if (result[0][0] != result[1][0] || result[0][1] != result[1][1] || result[0][2] != result[1][2])
                {
                    SetFail("[OUTPUT] only accept the same 0 or 1 qubit.");
                }
                else
                {
                    SetFail("[OUTPUT] doesn't accept the 0/1 qubit.");
                }
                return false;
            }
            else if (result[0][0] >= 3 && result[0][1] == 0 && result[0][2] >= 3)
            {
                return true;
            }
        }
        return false;
    }

    private bool Level_06()
    {
        if (isFailed) return false;
        if (allGates.Count < 8)
        {
            SetFail("[GATE] Please use 4 X gates and 4 CX gates.");
            return false;
        }
        else
        {
            Dictionary<string, int> gateCount = GetGateCount();
            if (gateCount["X"] != 4 || gateCount["CX"] != 4)
            {
                SetFail("[GATE] Please use 4 X gates and 4 CX gates.");
                return false;
            }
        }
        int[][] result = GetOutputResults();
        // Debug.Log("Level_06: result = " + string.Join(", ", result.Select(r => string.Join(",", r))));
        if (resultSum >= 4)
        {
            if (result[0][0] == 0 && result[0][1] == 0 && result[0][2] >= 1 &&
                result[1][0] >= 1 && result[1][1] == 0 && result[1][2] == 0 &&
                result[2][0] == 0 && result[2][1] == 0 && result[2][2] >= 1 &&
                result[3][0] == 0 && result[3][1] == 0 && result[3][2] >= 1)
            {
                // Debug.Log("Level_05: resultSum = " + resultSum + "-> isFailed = true");
                // Debug.Log("Level_05: result[0] = " + result[0][0] + ", " + result[0][1] + ", " + result[0][2]);
                // Debug.Log("Level_05: result[1] = " + result[1][0] + ", " + result[1][1] + ", " + result[1][2]);
                return true;
            }
            else
            {
                SetFail("[OUTPUT] Output qubit sould be 1111.");
                return false;
            }
        }
        return false;
    }

    private bool Level_07()
    {
        if (isFailed) return false;
        if (allGates.Count < 9)
        {
            SetFail("[GATE] Please use all gates for at least 1 time.");
            return false;
        }
        else
        {
            Dictionary<string, int> gateCount = GetGateCount();
            // Debug.Log("Level_07: gateCount = " + string.Join(", ", gateCount.Select(kv => kv.Key + ": " + kv.Value)));
            if (
                gateCount["X"] < 1 ||
                gateCount["Y"] < 1 ||
                gateCount["Z"] < 1 ||
                gateCount["X/2"] < 1 ||
                gateCount["Y/2"] < 1 ||
                gateCount["Z/2"] < 1 ||
                gateCount["H"] < 1 ||
                gateCount["🔍"] < 1 ||
                gateCount["CX"] < 1
                )
            {
                SetFail("[GATE] Please use all gates for at least 1 time.");
                return false;
            }
        }
        int[][] result = GetOutputResults();
        if (resultSum >= 4)
        {
            if (result[0][0] == 0 && result[0][1] == 0 && result[0][2] >= 1 &&
                result[1][0] == 0 && result[1][1] == 0 && result[1][2] >= 1 &&
                result[2][0] == 0 && result[2][1] == 0 && result[2][2] >= 1 &&
                result[3][0] == 0 && result[3][1] == 0 && result[3][2] >= 1)
            {
                // Debug.Log("Level_05: resultSum = " + resultSum + "-> isFailed = true");
                // Debug.Log("Level_05: result[0] = " + result[0][0] + ", " + result[0][1] + ", " + result[0][2]);
                // Debug.Log("Level_05: result[1] = " + result[1][0] + ", " + result[1][1] + ", " + result[1][2]);
                return true;
            }
            else
            {
                SetFail("[OUTPUT] Output qubit sould be 1111.");
                return false;
            }
        }
        return false;
    }
    // --------------------------------------------------------
    // do something after achieved
    private void AfterAchieved()
    {
        // do something after achieved
        winText.text = "You achieved it!";
        winText.gameObject.SetActive(true);
        SoundManager.singleton.PlayMusic("inLevel_achieved");

        // go to next scene
        if (nextScene != "")
        {
            continueButton.SetActive(true);
            SaveManager.SaveCurrentScene();
            // StartCoroutine(WaitAndLoadScene());
        }
        else
        {
            Debug.Log("No next scene specified.");
        }
    }

    // System.Collections.IEnumerator WaitAndLoadScene()
    // {
    //     yield return new WaitForSeconds(waitBeforeNextScene);
    //     UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
    // }
    // --------------------------------------------------------
    private void SetFail(string message)
    {
        isFailed = true;
        winText.text = "You're failed! - " + message;
        winText.gameObject.SetActive(true);
    }

    // private void AfterFailed()
    // {
    //     // do something after failed
    //     winText.text = "You're failed! - " + resultString;
    //     winText.gameObject.SetActive(true);
    // }

    // --------------------------------------------------------

    // write all hints here
    private void SetHintText(string hint)
    {
        if (hintText != null)
        {
            hintText.text = hint;
        }
        else
        {
            Debug.LogError("HintText is not assigned.");
        }
    }

    public void HintOn()
    {
        if (levelHints.ContainsKey(currentLevel))
        {
            SetHintText(levelHints[currentLevel] + galobalInstruction);
        }
        else
        {
            SetHintText("No hint available for this level.");
        }
        hintPanel.SetActive(true);
        BlockerManager.singleton.ShowBlocker3D();
        QuGameManager.singlton.launchButton.interactable = false; // disable the launch button

    }
    public void HintOff()
    {
        hintPanel.SetActive(false);
        BlockerManager.singleton.HideBlocker3D();
        QuGameManager.singlton.launchButton.interactable = true;
    }
}
