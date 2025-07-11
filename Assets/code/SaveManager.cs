using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
	public string lastSceneName;
	public string lastPlayedTime;
}


public static class SaveManager
{
	private static string SavePath => Application.persistentDataPath + "/save.json";

	public static void SaveCurrentScene()
	{
		string sceneName = SceneManager.GetActiveScene().name;

		if (!IsSceneSavable(sceneName)) return;

		SaveData data = new SaveData
		{
			lastSceneName = sceneName,
			lastPlayedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
		};

		string json = JsonUtility.ToJson(data, true);
		File.WriteAllText(SavePath, json);

		Debug.Log("Archive successful:" + json);
		Debug.Log("Save file path: " + Application.persistentDataPath);
	}

	public static SaveData Load()
	{
		if (!File.Exists(SavePath))
		{
			Debug.Log("No save files found");
			return null;
		}

		string json = File.ReadAllText(SavePath);
		return JsonUtility.FromJson<SaveData>(json);
	}

	public static void Delete()
	{
		if (File.Exists(SavePath))
			File.Delete(SavePath);
	}

	private static bool IsSceneSavable(string sceneName)
	{
		return sceneName == "QuMachine" || sceneName == "Level_01" || sceneName == "Level_02"
			|| sceneName == "Level_03" || sceneName == "Level_04" || sceneName == "Level_05" || sceneName == "Level_06"
			|| sceneName == "Level_07";
	}
}
