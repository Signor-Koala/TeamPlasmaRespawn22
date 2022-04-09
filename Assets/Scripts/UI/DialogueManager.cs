using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public TMPro.TextMeshProUGUI nameText;
	public TMPro.TextMeshProUGUI dialogueText;
	[SerializeField] GameObject dialogueBox;
	public Animator animator;

	private Queue<string> sentences;
	private Queue<string> speakers;
	public int sentenceNumber;
	public enum dialogueState
	{
		dialogueNotStarted, dialogueStarted, dialogueEnded
	}
	public dialogueState currentDialogueState;

	// Use this for initialization
	void Start () {
		dialogueBox.SetActive(false);
		sentences = new Queue<string>();
		speakers = new Queue<string>();
		sentenceNumber=0;
		currentDialogueState = dialogueState.dialogueNotStarted;
	}


	public void StartDialogue (Dialogue dialogue)
	{
		dialogueBox.SetActive(true);
		animator.SetBool("IsOpen", true);

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}
		foreach (string name in dialogue.speakers)
		{
			speakers.Enqueue(name);
		}

		currentDialogueState = dialogueState.dialogueStarted;
		sentenceNumber=0;
		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
		sentenceNumber++;

		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		string speaker = speakers.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(speaker,sentence));
	}

	IEnumerator TypeSentence (string speaker, string sentence)
	{	
		if(speaker !=null)
			nameText.text = speaker;
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue()
	{
		currentDialogueState = dialogueState.dialogueEnded;
		animator.SetBool("IsOpen", false);
		sentenceNumber=0;
	}

}
