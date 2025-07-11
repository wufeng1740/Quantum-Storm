using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections;

public class Skip : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string nextScene = "story1";

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (var flowchart in GameObject.FindObjectsByType<Fungus.Flowchart>(FindObjectsSortMode.None))
        {
            flowchart.StopAllBlocks();
        }
        GameObject fungusManager = GameObject.Find("FungusManager");
        if (fungusManager != null)
        {
            fungusManager.SetActive(false);
            fungusManager.SetActive(true);
            // Find all AudioSource components under fungusManager and clear their audio clips
            AudioSource[] audioSources = fungusManager.GetComponentsInChildren<AudioSource>(true);
            foreach (AudioSource audioSource in audioSources)
            {
                audioSource.Stop();
                audioSource.clip = null;
            }
        }

    #if UNITY_EDITOR
        // 只关闭 Fungus.FlowchartWindow
        var asm = typeof(UnityEditor.EditorWindow).Assembly;
        var flowchartWindowType = asm.GetType("Fungus.FlowchartWindow");
        if (flowchartWindowType != null)
        {
            var windows = Resources.FindObjectsOfTypeAll(flowchartWindowType);
            foreach (var win in windows)
            {
                ((UnityEditor.EditorWindow)win).Close();
            }
        }

        UnityEditor.EditorApplication.delayCall += () =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
        };
    #else
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
    #endif
    }


// ----------------------------------------------------------------------
    public void OnPointerEnter(PointerEventData eventData)
    {
        // If you want to change the color of this text component
        if (GetComponent<TextMeshProUGUI>() != null)
        {
            GetComponent<TextMeshProUGUI>().color = new Color(0.5f, 0.8f, 1f); // sky blue color
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GetComponent<TextMeshProUGUI>() != null)
        {
            GetComponent<TextMeshProUGUI>().color = Color.white; // or any other color you prefer
        }
    }

}
