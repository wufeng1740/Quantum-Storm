using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuEntanglementManager : MonoBehaviour
{
    public static QuEntanglementManager singlton;

    // public
    public Dictionary<int, List<GameObject>> entanglementList = new Dictionary<int, List<GameObject>>();
    public int entangledObjectCount = 0;
    public Color[] colors = new Color[] { Color.red, Color.green, Color.blue, Color.yellow, Color.cyan, Color.magenta, Color.white, Color.black, Color.gray };

    // private



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Awake()
    {
        singlton = this;

        Button launchButton = QuGameManager.singlton.launchButton;
        if (launchButton != null)
        {
            var buttonComponent = launchButton.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.onClick.AddListener(Clear);
            }
        }
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEntangledQubit(GameObject qubit, int number)
    {
        // mark qubit as entangled
        qubit.GetComponent<QubitGeneral>().isEntangled = true;
        // set color
        qubit.GetComponent<Outline>().enabled = true;
        qubit.GetComponent<Outline>().OutlineColor = colors[number];
        qubit.GetComponent<Outline>().OutlineWidth = 5f;
        // Debug.Log("SetEntangledQubit: " + qubit.name + ", number: " + number + ", isEntangled: " + qubit.GetComponent<QubitGeneral>().isEntangled);
    }

    public void UnsetEntangledQubit(GameObject qubit){
        // mark qubit as entangled
        qubit.GetComponent<QubitGeneral>().isEntangled = false;
        // set color
        qubit.GetComponent<Outline>().enabled = false;
        // qubit.GetComponent<Outline>().OutlineColor = colors[number];
        // qubit.GetComponent<Outline>().OutlineWidth = 5f;
    }

    public void SetEntangledList(int number) {
        // set the status and color of the entangled list
        // Debug.Log("SetEntangledList: " + number);
        if (entanglementList.ContainsKey(number))
        {
            RemoveNull();
            if (entanglementList.ContainsKey(number))
            {
                List<GameObject> list = entanglementList[number];
                for (int i = 0; i < list.Count; i++)
                {
                    SetEntangledQubit(list[i], number);
                }
            }
        }
    }

    public void AddEntanglement(GameObject control, GameObject target){
        // Debug.Log("AddEntanglement: " + control.name + " and " + target.name);
        // check if control is entangled
        CheckAndRemove(target);
        int entanglementNumber =  CheckQubitEntanglement(control);
        // if so, find it in Dic, and add target to it
        if (entanglementNumber != -1){
            List<GameObject> list = entanglementList[entanglementNumber];
            // check if target is already in the list
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == target)
                {
                    return;
                }
            }
            list.Add(target);
        }
        // if not, create a new list and add it to the Dic
        else{
            List<GameObject> list = new List<GameObject>{control, target};
            if (entanglementList.ContainsKey(entangledObjectCount))
            {
                entanglementList.Remove(entangledObjectCount);
            }
            entanglementList.Add(entangledObjectCount, list);
            entanglementNumber = entangledObjectCount;
            entangledObjectCount = (entangledObjectCount+1)%colors.Length;
        }
        SetEntangledList(entanglementNumber);
        // Debug.Log("Entangled List: " + entangledObjectCount + ", " + string.Join(", ", entanglementList[entangledObjectCount-1]));

    }

    public int CheckQubitEntanglement(GameObject qubit){
        // check if the qubit is entangled
        // if so, return the entanglement number (>=0)
        // if not, return -1
        // check if the qubit is in the entangledObjects list
        foreach (var item in entanglementList)
        {
            List<GameObject> list = item.Value;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                {
                    list.RemoveAt(i);
                }
                else if (list[i] == qubit)
                {
                    return item.Key;
                }
            }
        }
        // if not, return false
        return -1;
    }

    // --------------------------------------------------------------------------------
    // Remove
    public void RemoveNull(){
        // check if the entangled objects are still in the scene
        // if not, remove them from the list
        foreach (var item in entanglementList)
        {
            List<GameObject> list = item.Value;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] == null)
                {
                    list.RemoveAt(i);
                }
            }
            if (list.Count == 0 || list.Count == 1)
            {
                RemoveEntanglementByNumber(item.Key);
                return;
            }
        }
    }

    public void CheckAndRemove(GameObject qubit){
        // Debug.Log("CheckAndRemove: " + qubit.name + ", isEntangled: " + qubit.GetComponent<QubitGeneral>().isEntangled);
        if (qubit == null) return;
        if (!qubit.GetComponent<QubitGeneral>().isEntangled) return;
        Remove(qubit);
    }

    /// <summary>
    /// Remove the entangled qubit from the list
    /// </summary>
    /// <param name="qubit"></param>
    public void Remove(GameObject qubit){
        // Debug.Log("Remove: " + qubit.GetInstanceID());
        foreach (var item in entanglementList)
        {
            List<GameObject> list = item.Value;
            for (int i = 0; i < list.Count; i++)
            {
                // Debug.Log("Check: " + list[i].GetInstanceID());
                if (list[i] == qubit)
                {
                    // Debug.Log("found~");
                    UnsetEntangledQubit(qubit);
                    list.RemoveAt(i);
                    break;
                }
            }
            // if the list is empty or only 1 item, remove it from the dictionary
            if (list.Count == 0 || list.Count == 1)
            {
                RemoveEntanglementByNumber(item.Key);
                return;
            }
        }
    }

    public void RemoveEntanglementByNumber(int number){
        // remove the entangled objects by number
        if (entanglementList.ContainsKey(number))
        {
            List<GameObject> list = entanglementList[number];
            for (int i = 0; i < list.Count; i++)
            {
                UnsetEntangledQubit(list[i]);
            }
            entanglementList.Remove(number);
        }
    }

    public void Clear(){
        // clear all entangled objects
        entanglementList.Clear();
        entangledObjectCount = 0;
    }
}
