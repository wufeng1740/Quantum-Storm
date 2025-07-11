using UnityEngine;
using TMPro;

public class StartGameController : MonoBehaviour
{
    public GameObject blocker;                  // 拖入 Blocker 遮罩
    public TextMeshProUGUI instructionText;     // 拖入 Text
    [TextArea(3, 10)]
    public string[] lines = new string[]
{
    "Drop the particle into the round slot.",
    "Plug the detector into the square one.",
    "Then hit \"Launch\" and watch the quantum states collapse.",
    "Get three 0s and three 1s to pass the level."
};                    // 多行说明文字

    private int currentIndex = 0;
    private bool isShowing = true;

    void Start()
    {
        instructionText.text = "";
        blocker.SetActive(true); // 开场激活遮罩
    }

    void Update()
    {
        if (!isShowing) return;

        if (Input.GetMouseButtonDown(0)) // 每次点击
        {
            ShowNextLine();
        }
    }

    void ShowNextLine()
    {
        if (currentIndex < lines.Length)
        {
            instructionText.text += lines[currentIndex] + "\n";
            currentIndex++;
        }
        else
        {
            // 全部显示完，退出引导
            blocker.SetActive(false);
            instructionText.text = "";
            isShowing = false;

            Debug.Log("指引结束，开始游戏！");
            // 🔧 这里可以调用游戏开始方法（启用控制器、计时器等）
        }
    }
}
