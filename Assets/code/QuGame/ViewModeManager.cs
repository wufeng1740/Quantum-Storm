using UnityEngine;
using UnityEngine.UI;

public class ViewModeManager : MonoBehaviour
{   
    public static ViewModeManager singlton;
    public bool simpleMode = true;  // true: simple mode | false: sphere mode
    public string[] viewModeTag = new string[] { "Qubit" , "StaticQubit", "Gate"};
    public Button viewModeButton;
    public TMPro.TextMeshProUGUI viewModeText;


    void Awake()
    {
        singlton = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Debug.Log("ViewModeManager - Start");
        foreach (string tag in viewModeTag) {
            SetViewModeByTag(tag, simpleMode);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetChildOfGameObjects (GameObject[] gameObjects, string childName, bool isActive)
    {   
        foreach (GameObject gameObject in gameObjects)
        {
            GameObject children = gameObject.transform.Find(childName)?.gameObject;
            if (children != null) {
                children.SetActive(isActive);
            }
        }
    }

    public void SetViewModeByTag(string tag, bool isSimpleMode) {
        // get all objects
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
        // Debug.Log("SetViewModeByTag: " + tag + " " + gameObjects.Length);
        if (isSimpleMode) {
            // switch to simple mode
            SetChildOfGameObjects(gameObjects, "SimpleMode", true);
            SetChildOfGameObjects(gameObjects, "SphereMode", false);
        } else {
            // switch to sphere mode
            SetChildOfGameObjects(gameObjects, "SimpleMode", false);
            SetChildOfGameObjects(gameObjects, "SphereMode", true);
        }
    }

    public void SwitchViewMode(){
        // switch view mode for all objects
        simpleMode = !simpleMode;
        foreach (string tag in viewModeTag) {
            SetViewModeByTag(tag, simpleMode);
        }

        // change button color and text
        if (simpleMode)
        {
            viewModeButton.image.color = Color.white;
            viewModeText.text = "Simple Mode";
        }
        else
        {
            // viewModeButton.image.color = Color.red;
            viewModeButton.image.color = new Color(255f / 255f, 200f / 255f, 255f / 255f); // Light purple color
            viewModeText.text = "Sphere Mode";
        }
    }

    /// <summary>
    /// for specific object to refresh its view mode
    /// </summary>
    /// <param name="gameObject">The gameObject to refresh</param>
    public void SetViewModeForObject(GameObject gameObject) 
    {
        GameObject child;
        child = gameObject.transform.Find("SimpleMode")?.gameObject;
        if (child != null) {
            child.SetActive(simpleMode);
        }
        child = gameObject.transform.Find("SphereMode")?.gameObject;
        if (child != null) {
            child.SetActive(!simpleMode);
        }
    }
}
