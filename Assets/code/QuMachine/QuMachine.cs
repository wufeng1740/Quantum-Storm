using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuMachine : MonoBehaviour
{   
    // public
    // public static QuMachine singlton;
    public int gateNumber;
    public GameObject gateComponent;
    public GameObject outputComponent;
    public TextMeshPro[] outputTextObjects;   // amount of 0, 0/1, 1
    // public Button button;
    // public int gameMode = 0;
    // public QubitGeneral[] qubitsInMachine = new QubitGeneral[0];

    // private

    void Awake()
    {
        // singlton = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        if (!QuGameManager.singlton.autoBuiltMachine)
        {
            return;
        }
        // Gate
        if (gateNumber < 1)
        {
            gateNumber = 1;
        }
        if (gateNumber == 1)
        {
            return;
        }
        else
        {
            for (int i = 0; i < gateNumber-1; i++)
            {
                Instantiate(gateComponent, new Vector3(this.transform.position.x+(i+2)*5, this.transform.position.y, this.transform.position.z), Quaternion.identity, this.transform);
            }
        }
        // output
        outputComponent.transform.localPosition = new Vector3((gateNumber+1)*5, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

        public void OnLaunch()
    {
        // if (gameMode == 0)
        // {
        //     gameMode = 1;
        //     button.image.color = Color.red;
            
        // }
        // else    // gameMode == 1 change to 0
        // {
        //     gameMode = 0;
        //     button.image.color = Color.white;
            
        // }
    }
}
