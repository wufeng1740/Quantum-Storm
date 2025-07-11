using UnityEngine;
using UnityEngine.UI;

public class QuGameManager : MonoBehaviour
{
    // public
    public static QuGameManager singlton;
    // ----------------------
    [Header("Game Level Settings")]
    [Tooltip("Time to copy qubit in the input")]
    public float quMovingSpeed = 1.0f;  // normal moving speed
    [Tooltip("true: auto build machine, false: manual build machine")]
    public bool autoBuiltMachine = true;
    public int qubitMachineNumnber = 1;
    public int qubitMachineGateNumber = 1;

    // ----------------------
    [Header("Game Internal Settings")]
    public float timeToCopy = 10f;   // time of one gate = 5s
    public float timeScale = 1.0f; // time scale for the game
    // ----------------------

    [Header("Game API (don't change)")]
    public GameObject qubitMachine;
    public GameObject[] qubitMachines;  // for other scripts to access
    public Button launchButton;
    public Slider quMovingSpeedSlider;
    public TMPro.TextMeshProUGUI quMovingSpeedText;

    public bool launchMode = false;
    // true: launch mode
    // false: editor mode;

    // private
    private float qubitMachineDistance = 8f;

    void Awake()
    {
        singlton = this;
        // Set the time scale for the game
        Time.timeScale = timeScale;
        // Set the qubit moving speed
        quMovingSpeed = quMovingSpeedSlider.value;
        quMovingSpeedText.text = quMovingSpeed.ToString();

        // Set the qubit machine gate number
        if (qubitMachineGateNumber < 1)
        {
            qubitMachineGateNumber = 1;
        }
        qubitMachine.GetComponent<QuMachine>().gateNumber = qubitMachineGateNumber;

        if (autoBuiltMachine)
        {
            // Set the qubit machine
            if (qubitMachineNumnber < 1)
            {
                qubitMachineNumnber = 1;
            }
            qubitMachines = new GameObject[qubitMachineNumnber];
            qubitMachines[0] = qubitMachine;
            qubitMachines[0].name = "QubitMachine1";
            // Set the qubit machines
            if (qubitMachineNumnber > 1)
            {
                for (int i = 2; i <= qubitMachineNumnber; i++)
                {
                    GameObject qubitMachineClone = Instantiate(qubitMachine, qubitMachine.transform.position + new Vector3(0, -qubitMachineDistance * (i-1), 0), Quaternion.identity);
                    qubitMachineClone.name = "QubitMachine" + i;
                    qubitMachineClone.transform.parent = this.transform;
                    qubitMachines[i-1] = qubitMachineClone;
                }
            }
        }
        else
        {
            qubitMachines = new GameObject[qubitMachineNumnber];
            qubitMachines[0] = qubitMachine;
            qubitMachines[0].name = "QubitMachine1";
            for (int i = 1; i < transform.childCount; i++)
            {
                qubitMachines[i] = transform.GetChild(i).gameObject;
                qubitMachines[i].name = "QubitMachine" + (i + 1);
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLaunch()
    {
        launchMode = !launchMode;
        if (launchMode)
        {
            launchButton.image.color = Color.red;
            quMovingSpeedSlider.interactable = false;
            SoundManager.singleton.PlaySFX("launch_click");
            SoundManager.singleton.PlayLoopSFX("launch_running");
        }
        else
        {
            launchButton.image.color = Color.white;
            quMovingSpeedSlider.interactable = true;
            SoundManager.singleton.StopLoopSFX();
            SoundManager.singleton.PlaySFX("launch_off");
        }
    }

    public void OnSlideChange(){
        if (launchMode) return;
        quMovingSpeed = quMovingSpeedSlider.value;
        quMovingSpeedText.text = quMovingSpeed.ToString();
    }
}
