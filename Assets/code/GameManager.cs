using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text[] TextValue;
    public GameObject InPutObj;
    public GameObject obj1;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartObj());
    }

    IEnumerator StartObj()
    {
        TextValue[0].DOText("Beyond Mars orbit, the \"Stellar Ring IX\" space station silently floats.", 3).SetEase(Ease.Linear);
        TextValue[0].DOFade(1, 3).SetEase(Ease.Linear).OnComplete(() => {  TextValue[0].DOFade(0, 2);});
        yield return new WaitForSeconds(5);
        TextValue[1].DOText("This is humanity's quantum computing frontier, with the super AI Q-NOVA managing all operations.", 3).SetEase(Ease.Linear);
        TextValue[1].DOFade(1, 3).SetEase(Ease.Linear).OnComplete(() => { TextValue[1].DOFade(0, 2); });
        yield return new WaitForSeconds(5);
        TextValue[2].DOText("Deep-space probes, quantum communications, and fleet commands......\r\n", 3).SetEase(Ease.Linear);
        TextValue[2].DOFade(1, 3).SetEase(Ease.Linear).OnComplete(() => { TextValue[2].DOFade(0, 2); });
        yield return new WaitForSeconds(5);
        TextValue[3].DOText("Due to high automation, only essential maintenance crews remain onboard.", 3).SetEase(Ease.Linear);
        TextValue[3].DOFade(1, 3).SetEase(Ease.Linear).OnComplete(() => { TextValue[3].DOFade(0, 2); });
        yield return new WaitForSeconds(5);
        TextValue[4].DOText("Quantum engineers like you are mere inspectors now.", 3).SetEase(Ease.Linear);
        TextValue[4].DOFade(1, 3).SetEase(Ease.Linear).OnComplete(() => { TextValue[4].DOFade(0, 2); });
        yield return new WaitForSeconds(5);
        obj1.SetActive(true);
        TextValue[5].DOText("Three days ago, intense solar activity striked Stellar Ring IX.", 3).SetEase(Ease.Linear);
        TextValue[5].DOFade(1, 3).SetEase(Ease.Linear).OnComplete(() => { TextValue[5].DOFade(0, 2); });
        yield return new WaitForSeconds(5);
        EnterGame();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void EnterGame()
    {
        SceneManager.LoadScene("story1");
    }
}
