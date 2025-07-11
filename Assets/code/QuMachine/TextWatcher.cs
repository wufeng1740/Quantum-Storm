using TMPro;
using UnityEngine;
using System;

public class TextWatcher : MonoBehaviour
{
    public TextMeshPro targetText;
    private string lastText;

    public event Action OnTextChanged;

    void Awake()
    {
        if (targetText == null)
            targetText = GetComponent<TextMeshPro>();

        lastText = targetText.text;
    }

    void Update()
    {
        if (targetText.text != lastText)
        {
            // Debug.Log($"Text changed from {lastText} to: {targetText.text}");
            lastText = targetText.text;
            OnTextChanged?.Invoke();
        }
    }
}
