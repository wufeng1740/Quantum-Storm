using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueLine
{
	public string speaker;
	public string side;
	public string avatar;
	public string text;
	public string background;
}

public class DialogueManager : MonoBehaviour
{
	public RawImage backgroundRaw;
	public GameObject dialogueBox;
	public TextMeshProUGUI nameText;
	public RawImage leftAvatarRaw;
	public RawImage rightAvatarRaw;
	public TextMeshProUGUI dialogueText;
	public GameObject clickToContinue;

	public float typingSpeed = 0.05f;

	private List<DialogueLine> currentDialogue;
	private int lineIndex = 0;
	private bool isTyping = false;

	void Start()
	{
		LoadDialogue("scene1_dialogue1");
	}

	public void LoadDialogue(string fileName)
	{
		TextAsset jsonFile = Resources.Load<TextAsset>($"Dialogues/{fileName}");
		if (jsonFile != null)
		{
			currentDialogue = new List<DialogueLine>(
				JsonUtility.FromJson<DialogueArray>("{\"lines\":" + jsonFile.text + "}").lines
			);
			lineIndex = 0;
			ShowLine();
		}
		else
		{
			Debug.LogError("Dialog file not found: " + fileName);
		}
	}

	void ShowLine()
	{
		if (lineIndex >= currentDialogue.Count)
		{
			dialogueBox.SetActive(false);
			return;
		}

		dialogueBox.SetActive(true);
		clickToContinue.SetActive(false);

		DialogueLine line = currentDialogue[lineIndex];

		if (!string.IsNullOrEmpty(line.background))
		{
			Texture2D bgTex = Resources.Load<Texture2D>($"Backgrounds/{line.background}");
			if (bgTex != null)
			{
				backgroundRaw.texture = bgTex;
			}
			else
			{
				Debug.LogWarning($"Background '{line.background}' not found in Resources/Backgrounds.");
			}
		}

		nameText.text = line.speaker;

		Texture2D avatarTex = Resources.Load<Texture2D>($"Avatars/{line.avatar}");
		Texture2D transparentTex = Resources.Load<Texture2D>("Avatars/transparent");

		if (line.side == "left")
		{
			leftAvatarRaw.texture = avatarTex;
			rightAvatarRaw.texture = transparentTex;
		}
		else
		{
			rightAvatarRaw.texture = avatarTex;
			leftAvatarRaw.texture = transparentTex;
		}

		StartCoroutine(TypeSentence(line.text));
	}

	IEnumerator TypeSentence(string sentence)
	{
		isTyping = true;
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return new WaitForSeconds(typingSpeed);
		}

		isTyping = false;
		clickToContinue.SetActive(true);
	}

	void Update()
	{
		if (dialogueBox.activeSelf && Input.GetMouseButtonDown(0))
		{
			if (isTyping)
			{
				StopAllCoroutines();
				dialogueText.text = currentDialogue[lineIndex].text;
				isTyping = false;
				clickToContinue.SetActive(true);
			}
			else
			{
				lineIndex++;
				ShowLine();
			}
		}
	}

	[System.Serializable]
	private class DialogueArray
	{
		public DialogueLine[] lines;
	}
}
