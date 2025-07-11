using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QubitGeneral : MonoBehaviour
{
    // public
    public float movingSpeed = 0;
    public Vector3 movingDirection = Vector3.right; // Default direction (forward)
    public int status = 0; // 用 -2，-1，0, 1 来表示叠加态（未知，箭头乱动），叠加态（已知，箭头固定），0， 和1 状态
    public Vector3 arrowDirection = Vector3.up; // 箭头方向
    public TMPro.TextMeshPro statusText; // 状态文字
    public GameObject arrow; // 箭头
    public bool arrowRandomMove = false; // 箭头是否随机移动
    // public float arrowRandomRotationSpeed = 4000f; // when status==-2, rotation speed
    public float arrowRotationDuration = 0.2f;
    public bool isEntangled = false; // 是否纠缠
    public float launchtime = 0f;
    public Vector3 launchPosition;

    // private
    private bool launchMode;
    private float arrowRotationTimer = 0f;
    // private bool isInGate = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void OnEnable()
    {
        if (ViewModeManager.singlton == null) return;

        ViewModeManager.singlton.SetViewModeForObject(this.gameObject);
    }
    void Awake()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        launchMode = QuGameManager.singlton.launchMode;
        SetFromStatusAndArrowDirection(status, arrowDirection);

        // add listeners to LaunchButton
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

    // Update is called once per frame
    void FixedUpdate()
    {
        // Debug.Log("Qubit: " + this.gameObject.name + ", " + status + ", " + arrowDirection);
        // // text
        // statusText.text = GetStatusToText();

        // arrow random move
        if (arrowRandomMove)
        {
            if (arrowRotationTimer >= arrowRotationDuration)
            {
                arrowRotationTimer = 0f;
                Vector3 randomRotation = new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f)
                ).normalized;
                // arrow.transform.Rotate(randomRotation * arrowRandomRotationSpeed * Time.fixedDeltaTime);
                arrow.transform.Rotate(randomRotation * 360f);
            }
            else
            {
                arrowRotationTimer += Time.fixedDeltaTime;
            }
        }

        if (QuGameManager.singlton.launchMode == false) return;
        // position
        // Debug.Log(this.transform.position.x + " " + Time.fixedDeltaTime);
        if (launchtime != 0f && movingSpeed == QuGameManager.singlton.quMovingSpeed)
        {
            float elapsedTime = Time.time - launchtime;
            Vector3 newPos = launchPosition + movingDirection.normalized * movingSpeed * elapsedTime;
            this.GetComponent<Rigidbody>().MovePosition(newPos);
        }
        else if (movingSpeed != 0f)
        {
            Vector3 newPos = transform.position + movingDirection.normalized * movingSpeed * Time.fixedDeltaTime;
            this.GetComponent<Rigidbody>().MovePosition(newPos);
        }
        // Debug.Log("Qubit: " + this.gameObject.name + ", Speed = "+ movingSpeed +", Position: " + this.transform.position);
        // status text
    }

    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Gate")) isInGate = true;
    // }

    // void OnTriggerExit(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Gate")) isInGate = false;
    // }


    // ----------------- public functions -----------------
    public void SetFromStatusAndArrowDirection(int status, Vector3? arrowDirection = null)
    {
        /*
        input: 
            status: -2: unknown; -1: known; 0: up, 1: down
            arrowDirection: the direction of the arrow, only used when status == -1
        output:
            arrowDirection: based on status or itself
            arrowRandomMove: true when status == -2
            arrow: set the direction of the arrow
        */
        this.status = status;
        arrowRandomMove = false;
        if (status == 0)
        {
            this.arrowDirection = Vector3.up;
            arrow.transform.up = this.arrowDirection;
        }
        else if (status == 1)
        {
            this.arrowDirection = Vector3.down;
            arrow.transform.up = this.arrowDirection;
        }
        else if (status == -1)
        {
            this.arrowDirection = arrowDirection ?? Vector3.right;
            arrow.transform.up = this.arrowDirection;
            if (arrowDirection == null)
            {
                Debug.Log(this.gameObject.name + "Missing Arrow Direction from input when status ==-1!");
            }
        }
        else
        { // -2
            arrowRandomMove = true;
        }
        SetStatusText();
    }

    public void SetFromArrowObject()
    {
        // Debug.Log("SetFromArrowObject >> arrow.transform.up: " + arrow.transform.up);
        // use arrow object to set the status and arrow direction
        if (arrow == null)
        {
            Debug.Log(this.gameObject.name + "Missing Arrow Object!");
            return;
        }
        if (status == -2)
        {
            Debug.Log(this.gameObject.name + " is in unknown state, cannot set status!");
            return;
        }
        arrowDirection = arrow.transform.up;
        if (arrowDirection == Vector3.up)
        {
            status = 0;
        }
        else if (arrowDirection == Vector3.down)
        {
            status = 1;
        }
        else
        {
            status = -1;
        }
        SetStatusText();
        // Debug.Log("Qubit: " + this.gameObject.name + ", " + status + ", " + arrowDirection);  
    }

    // public function: rotate the arrow along the axis
    // axis: the axis to rotate around
    // angle: the angle to rotate
    // duration: the time to rotate
    public void RotateArrow(Vector3 axis, float angle, float duration)
    {
        if (status == -2)
        {
            Debug.Log(this.gameObject.name + " is in unknown state, cannot rotate!");
            return;
        }
        StartCoroutine(RotateOverFixedTime(axis, angle, duration));
    }

    /// <summary>
    /// Set the time and position of the qubit when it is launched.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="position"></param>
    public void SetLaunchTimeAndPosition()
    {
        // set the launch time and position
        launchtime = Time.time;
        launchPosition = this.gameObject.transform.position;
        // isInGate = false; // reset isInGate
        // Debug.Log("Qubit: " + this.gameObject.name + " launched at time: " + launchtime + ", position: " + launchPosition);
    }

    public void ForceResetQubitPositionAndSpeed()
    {
        if (launchtime != 0f)
        {
            float elapsedTime = Time.time - launchtime;
            Vector3 newPos = launchPosition + movingDirection.normalized * movingSpeed * elapsedTime;
            this.GetComponent<Rigidbody>().MovePosition(newPos);
            movingSpeed = QuGameManager.singlton.quMovingSpeed; // reset speed
        }
    }
    // ----------------- private functions -----------------
    private IEnumerator RotateOverFixedTime(Vector3 axis, float angle, float duration)
    {
        Quaternion startRotation = arrow.transform.rotation;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, axis) * startRotation;
        // Debug.Log("startRotation" + startRotation.eulerAngles);
        // Debug.Log("tagetRotation: " + targetRotation.eulerAngles);
        float elapsedTime = 0f;

        // Debug.Log("startRotation: " + startRotation.eulerAngles + ", " + arrowDirection);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            arrow.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return new WaitForFixedUpdate();  // 关键：等待物理帧
        }

        arrow.transform.rotation = targetRotation; // 确保精确到位
        // when comlplete, set status and arrow direction
        SetFromArrowObject();
        // Debug.Log("endRotation: " + arrow.transform.rotation.eulerAngles + ", " + arrowDirection);
    }

    private void SetStatusText()
    {
        if (statusText == null)
        {
            Debug.Log(this.gameObject.name + "Missing Status Text!");
            return;
        }
        if (status == 0)
        {
            statusText.text = "0";
        }
        else if (status == 1)
        {
            statusText.text = "1";
        }
        else
        {
            statusText.text = "0/1";
        }
    }

    public void OnLaunch()
    {
        if (QuGameManager.singlton.launchMode == false)
        {
            // set the sphere collider radius to 0.5f
            this.GetComponent<SphereCollider>().radius = 0.5f;
        }
        else
        {
            // set the sphere collider radius to 0.25f
            this.GetComponent<SphereCollider>().radius = 0.25f;
        }
    }
}
