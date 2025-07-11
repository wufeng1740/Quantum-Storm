using System.Collections.Generic;
using UnityEngine;

public class Dragging : MonoBehaviour
{
    public bool isDraggable = true;   // 拖拽开关

    private Vector3 offset;
    private Camera mainCamera;
    private bool isDragging = false;
    // private int gameMode;
    private GameObject copyOfObject;
    // private GameObject targetObject;
    // private GameObject sameTagObject;
    // public bool isInTarget = false;
    private Dictionary<string, string> targetDict = new Dictionary<string, string>();
    private List<GameObject> targetLocationsList = new List<GameObject>();
    void Awake()
    {   
        // add all target tags pairs
        targetDict.Add("Qubit", "QubitLocation");
        targetDict.Add("Gate", "GateLocation");
    }

    void Start()
    {
        mainCamera = Camera.main;
        
        // 检测是否存在 Rigidbody，如果没有则添加并关闭重力
        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
        }
    }

    void Update()
    {
        
    }
    
    void OnMouseDown()
    {
        if (!isDraggable) return;
        if (QuGameManager.singlton.launchMode == true) return;

        isDragging = true;

        // copy object if not in target
        if (targetLocationsList.Count == 0)      // -> it's not in targetLocation
        {
            copyOfObject = Instantiate(gameObject, transform.position, transform.rotation);
            copyOfObject.transform.SetParent(transform.parent);
            copyOfObject.transform.localScale = transform.localScale;
            copyOfObject.name = gameObject.name;
            gameObject.name = gameObject.name + "_copy";
        }

        // 计算鼠标与物体的偏移量
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z);
    }

    void OnMouseDrag()
    {
        if (QuGameManager.singlton.launchMode == true) return;
        if (!isDraggable || !isDragging) return;

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z) + offset;
    }

    void OnMouseUp()
    {   
        if (QuGameManager.singlton.launchMode) return;
        if (!isDraggable) return;
        isDragging = false;
        // Debug.Log(this.gameObject.name + " is in target: " + isInTarget);
        if (targetLocationsList.Count==0)
        {
            Destroy(gameObject);
        }
        else    // -> it's in targetLocation already
        {
            GameObject targetObject = GetClosestTargetLocation();
            // move to target position
            foreach (Transform child in targetObject.transform)
            {
                if(child.gameObject.CompareTag(gameObject.tag) && child.gameObject != gameObject)
                {
                    Destroy(child.gameObject);
                }
            }
            transform.position = targetObject.transform.position;
            transform.SetParent(targetObject.transform);
            
            // play sound
            if (gameObject.CompareTag("Qubit"))
            {
                SoundManager.singleton.PlaySFX("dropQubit");
            }
            else if (gameObject.CompareTag("Gate"))
            {
                SoundManager.singleton.PlaySFX("dropGate");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsTargetLocation(other.gameObject))
        {
            // Debug.Log(this.gameObject.name + ": " + string.Join(", ", targetLocationsList));
            // isInTarget = true;
            targetLocationsList.Add(other.gameObject);
        }
        // else if (CompareTag(other.gameObject.tag))
        // {
        //     sameTagObject = other.gameObject;
        // }
    }

    void OnTriggerExit(Collider other)
    {
        // Debug.Log(this.gameObject.name + " exit " + other.gameObject.tag);
        if (IsTargetLocation(other.gameObject))
        {
            // Debug.Log(this.gameObject.name + ": " + string.Join(", ", targetLocationsList));
            targetLocationsList.Remove(other.gameObject);
        }
        // else if (CompareTag(other.gameObject.tag))
        // {
        //     sameTagObject = null;
        // }
    }

    bool IsTargetLocation(GameObject target)
    {   
        // Debug.Log("CheckTarget: " + gameObject.tag + " " + target.tag);
        if (QuGameManager.singlton.launchMode == true) return false;
        if (targetDict.ContainsKey(gameObject.tag))
        {
            if (target.CompareTag(targetDict[gameObject.tag]))
            {   
                // Debug.Log("CheckTarget: " + gameObject.tag + " " + target.tag + " -> true");
                return true;
            }
        }
        return false;
    }

    private GameObject GetClosestTargetLocation()
    {
        if (targetLocationsList.Count == 1)
        {
            return targetLocationsList[0];
        }
        // else: targetLocationsList.Count>= 2
        GameObject closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject target in targetLocationsList)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }

        return closestTarget;
    }
}