using UnityEngine;

public class FollowSphere : MonoBehaviour
{
    public Transform sphere; // 球体引用

    void Update()
    {
        if (sphere != null)
        {
            // 让坐标轴始终跟随球体的位置
            transform.position = sphere.position;
            transform.rotation = Quaternion.identity;
        }
    }
}
