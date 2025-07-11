/*using Fungus;
using UnityEngine;
using UnityEngine.UI;

public class AutoPlayController : MonoBehaviour
{
    public Flowchart flowchart;       // 关联的Flowchart组件
    public string targetBlockName;    // 目标Block名称（如 "StoryBlock"）
    public Button playPauseButton;    // 播放/暂停按钮
    public Sprite playIcon;           // ▶️图标
    public Sprite pauseIcon;          // ⏸️图标

    private bool isAutoPlaying = false; // 当前是否在自动播放
    private Block currentBlock;        // 当前执行的Block
    private int pausedCommandIndex;    // 暂停时的命令索引

    void Start()
    {
        // 初始化按钮图标
        UpdateButtonIcon();
    }

    // 点击按钮切换播放/暂停
    public void ToggleAutoPlay()
    {
        isAutoPlaying = !isAutoPlaying;

        if (isAutoPlaying)
        {
            // 首次播放：从Block开头开始
            if (currentBlock == null)
            {
                currentBlock = flowchart.FindBlock(targetBlockName);
                pausedCommandIndex = 0;
            }

            // 从记录的索引位置继续执行
            currentBlock.JumpToCommandIndex(pausedCommandIndex);
            flowchart.ExecuteBlock(currentBlock);
        }
        else
        {
            // 暂停时记录当前状态
            currentBlock = flowchart.HasBlock();
            if (currentBlock != null)
            {
                pausedCommandIndex = currentBlock.PreviousActiveCommandIndex;
                flowchart.StopAllBlocks();
            }
        }

        UpdateButtonIcon();
    }

    // 更新按钮图标
    private void UpdateButtonIcon()
    {
        playPauseButton.image.sprite = isAutoPlaying ? pauseIcon : playIcon;
    }

    // 剧情结束时重置状态（在Flowchart末尾调用）
    public void OnStoryEnd()
    {
        isAutoPlaying = false;
        currentBlock = null;
        pausedCommandIndex = 0;
        UpdateButtonIcon();
    }
}*/