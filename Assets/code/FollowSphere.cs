using UnityEngine;

public class FollowSphere : MonoBehaviour
{
    public Transform sphere; // ��������

    void Update()
    {
        if (sphere != null)
        {
            // ��������ʼ�ո��������λ��
            transform.position = sphere.position;
            transform.rotation = Quaternion.identity;
        }
    }
}
