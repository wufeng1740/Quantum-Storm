using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
public class CooldownButton : MonoBehaviour
{
    private Button myButton;
    private bool canClick = true;

    void Awake()
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(OnButtonClicked);
    }
    public void OnButtonClicked()
    {
        if (!canClick) return;

        // 执行你想做的事情
        // Debug.Log("按钮点击触发！");
        
        // 开始冷却
        StartCoroutine(ClickCooldown());
    }

    private System.Collections.IEnumerator ClickCooldown()
    {
        canClick = false;
        myButton.interactable = false; // 可选：按钮变灰
        yield return new WaitForSeconds(1f); // 1秒冷却
        canClick = true;
        myButton.interactable = true;  // 恢复按钮状态
    }
}
