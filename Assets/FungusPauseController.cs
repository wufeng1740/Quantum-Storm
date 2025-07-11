/*using Fungus;
using UnityEngine;

public class FungusPauseController : MonoBehaviour
{
    public Flowchart flowchart; // 关联的Flowchart组件

    // 记录当前状态
    private string pausedBlockName; // 暂停时的Block名称
    private int pausedCommandIndex; // 暂停时的命令索引

    // 暂停当前剧情
    public void PauseStory()
    {
        // 获取当前正在执行的Block
        Block executingBlock = flowchart.get();
        if (executingBlock != null)
        {
            pausedBlockName = executingBlock.BlockName;
            pausedCommandIndex = executingBlock.PreviousActiveCommandIndex;
            flowchart.StopAllBlocks();
        }
    }

    // 从记录的索引位置恢复播放
    public void ResumeStory()
    {
        if (string.IsNullOrEmpty(pausedBlockName)) return;

        // 找到之前暂停的Block
        Block targetBlock = flowchart.FindBlock(pausedBlockName);
        if (targetBlock == null) return;

        // 设置命令索引并执行
        targetBlock.set(pausedCommandIndex);
        flowchart.ExecuteBlock(targetBlock);
    }

    // 重置记录的状态（在剧情结束时调用）
    public void ResetPauseState()
    {
        pausedBlockName = null;
        pausedCommandIndex = 0;
    }
}*/