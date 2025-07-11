using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneTracker
{
	public static string previousSceneName;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void RegisterSceneTracking()
	{
		SceneManager.sceneLoaded += (scene, mode) =>
		{
			if (scene.name != "LoadingScene")
			{
				previousSceneName = scene.name;
			}
		};
	}
}
