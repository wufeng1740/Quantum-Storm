using UnityEngine;
using UnityEngine.EventSystems;

public class GlobalInteractionLocker : MonoBehaviour
{
    public CanvasGroup uiBlocker;   // 拖入你的 Blocker 遮罩（带 CanvasGroup）
    public bool freezeTime = true;  // 是否暂停游戏时间
    private float originalTimeScale;

    void Start()
    {
        originalTimeScale = Time.timeScale;

        // 开场锁定
        if (uiBlocker != null)
        {
            uiBlocker.blocksRaycasts = true;
            uiBlocker.interactable = true;
            uiBlocker.alpha = 1f;
            uiBlocker.gameObject.SetActive(true);
        }

        if (freezeTime)
        {
            Time.timeScale = 0;
        }

        Debug.Log("⛔ 所有交互已被锁定！");
    }

    // Fungus 调用此方法后解锁交互
    public void UnlockAll()
    {
        if (uiBlocker != null)
        {
            uiBlocker.blocksRaycasts = false;
            uiBlocker.interactable = false;
            uiBlocker.alpha = 0f;
            uiBlocker.gameObject.SetActive(false);
        }

        if (freezeTime)
        {
            Time.timeScale = originalTimeScale;
        }

        Debug.Log("✅ 所有交互已解锁！");
    }
}
