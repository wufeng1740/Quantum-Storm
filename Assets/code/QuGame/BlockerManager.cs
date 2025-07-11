using UnityEngine;

public class BlockerManager : MonoBehaviour
{
    public static BlockerManager singleton;

    [Header("Blocker settings")]
    public GameObject BlockerUI;
    public GameObject Blocker3D;

    void Awake()
    {
        // ����ȥ��
        if (singleton != null && singleton != this)
        {
            Destroy(gameObject);
            return;
        }
        singleton = this;
        DontDestroyOnLoad(gameObject);

        // --- �Զ�Ѱ�ҳ������Ӧ�����壬��������ʱ���ö�ʧ ---
        if (BlockerUI == null)
        {
            BlockerUI = GameObject.Find("BlockerUI");
            if (BlockerUI == null)
                Debug.LogError("BlockerManager: �Զ����� BlockerUI ʧ�ܣ����鳡������������");
        }
        if (Blocker3D == null)
        {
            Blocker3D = GameObject.Find("Blocker3D");
            if (Blocker3D == null)
                Debug.LogError("BlockerManager: �Զ����� Blocker3D ʧ�ܣ����鳡������������");
        }
        // ----------------------------------------------------
    }

    public void ShowBlocker()
    {
        if (BlockerUI != null) BlockerUI.SetActive(true);
        if (Blocker3D != null) Blocker3D.SetActive(true);
        Debug.Log("BlockerManager: BlockerUI and Blocker3D are now active.");
    }

    public void HideBlocker()
    {
        if (BlockerUI != null) BlockerUI.SetActive(false);
        if (Blocker3D != null) Blocker3D.SetActive(false);
    }

    public void ShowBlocker3D()
    {
        Blocker3D.SetActive(true);
    }
    public void HideBlocker3D()
    {
        Blocker3D.SetActive(false);
    }
}
