using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ButtonBreath : MonoBehaviour
{
    [Header("闪烁配色（示例：霓虹蓝）》")]
    [Tooltip("暗色，例如 #003F4F")]
    public Color darkColor = new Color32(0x00, 0x3F, 0x4F, 0xFF);
    [Tooltip("亮色，例如 #00FFFF")]
    public Color brightColor = new Color32(0x00, 0xFF, 0xFF, 0xFF);

    [Header("闪烁频率（对应正弦曲线中的频率）")]
    [Tooltip("值越大闪烁越快，推荐 1~3 之间")]
    public float speed = 1.5f;

    // 挂载的 Image 组件
    private Image _buttonImage;

    void Awake()
    {
        _buttonImage = GetComponent<Image>();
        if (_buttonImage == null)
        {
            Debug.LogError("ButtonBreath 脚本需要挂在有 Image 组件的 GameObject 上。");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        // t 从 0~1 循环往返
        float t = (Mathf.Sin(Time.time * speed) + 1f) / 2f;
        // 在 darkColor ↔ brightColor 之间线性插值
        _buttonImage.color = Color.Lerp(darkColor, brightColor, t);
    }
}
