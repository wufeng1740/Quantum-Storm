using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
	public float totalLoadTime = 5f;
	public float minFakeStall = 1f;
	public float maxFakeStall = 2f;

	private void Start()
	{
		StartCoroutine(LoadGameScene());
	}

	IEnumerator LoadGameScene()
	{
		string targetScene;

		if (!string.IsNullOrEmpty(LoadingData.targetScene))
		{
			targetScene = LoadingData.targetScene;
			LoadingData.targetScene = null;
		}
		else
		{
			string previousScene = GetPreviousSceneName();
			targetScene = MapTargetScene(previousScene);
		}

		float fakeDelay = Random.Range(minFakeStall, maxFakeStall);
		yield return new WaitForSeconds(fakeDelay);

		AsyncOperation operation = SceneManager.LoadSceneAsync(targetScene);
		operation.allowSceneActivation = false;

		yield return new WaitForSeconds(totalLoadTime - fakeDelay);

		operation.allowSceneActivation = true;
	}

	private string GetPreviousSceneName()
	{
		return SceneTracker.previousSceneName;
	}

	private string MapTargetScene(string fromScene)
	{
		switch (fromScene)
		{
			case "Level_01": return "MainMenu";
			case "Level_02": return "MainMenu";
			case "Level_03": return "MainMenu";
			case "Level_04": return "MainMenu";
			case "Level_05": return "MainMenu";
			case "Level_06": return "MainMenu";
			case "Level_07": return "MainMenu";
			case "QuMachine": return "MainMenu";
			case "MainMenu": return "story0";
			default: return "MainMenu";
		}
	}
}
