/*using Fungus;
using UnityEngine;

public class FungusPauseController : MonoBehaviour
{
    public Flowchart flowchart; // ������Flowchart���

    // ��¼��ǰ״̬
    private string pausedBlockName; // ��ͣʱ��Block����
    private int pausedCommandIndex; // ��ͣʱ����������

    // ��ͣ��ǰ����
    public void PauseStory()
    {
        // ��ȡ��ǰ����ִ�е�Block
        Block executingBlock = flowchart.get();
        if (executingBlock != null)
        {
            pausedBlockName = executingBlock.BlockName;
            pausedCommandIndex = executingBlock.PreviousActiveCommandIndex;
            flowchart.StopAllBlocks();
        }
    }

    // �Ӽ�¼������λ�ûָ�����
    public void ResumeStory()
    {
        if (string.IsNullOrEmpty(pausedBlockName)) return;

        // �ҵ�֮ǰ��ͣ��Block
        Block targetBlock = flowchart.FindBlock(pausedBlockName);
        if (targetBlock == null) return;

        // ��������������ִ��
        targetBlock.set(pausedCommandIndex);
        flowchart.ExecuteBlock(targetBlock);
    }

    // ���ü�¼��״̬���ھ������ʱ���ã�
    public void ResetPauseState()
    {
        pausedBlockName = null;
        pausedCommandIndex = 0;
    }
}*/