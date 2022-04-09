using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forestEntryTrigger : MonoBehaviour 
{

	public Dialogue dialogue;
	DialogueManager dialogueManager;
	portalScript portalController;
	[SerializeField] GameObject titleUI;

	bool eventFlag=false, playerInRange=false;

	private void Start() {
		titleUI.SetActive(false);
		dialogueManager = FindObjectOfType<DialogueManager>();
		portalController = this.GetComponent<portalScript>();
		CEO_script.currentGameState = CEO_script.gameState.preForestLevel;
	}

	private void Update() {
		if(dialogueManager.sentenceNumber>=2 && eventFlag==false && playerInRange)
		{
			portalController.isActivated=true;
			portalController.activatePortal();
			eventFlag=true;
			StartCoroutine("teleporter");
		}
		if(dialogueManager.currentDialogueState==DialogueManager.dialogueState.dialogueStarted)
		{
			if(Input.GetKeyDown("z"))
				dialogueManager.DisplayNextSentence();	
		}
		else if(titleUI.activeSelf)
		{
			if(Input.GetKeyDown("z"))
				UnityEngine.SceneManagement.SceneManager.LoadScene("forest");
		}
		
	}

	public void TriggerDialogue ()
	{
		FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
	}

	IEnumerator teleporter()
	{
		yield return new WaitForSeconds(2);
		dialogueManager.DisplayNextSentence();
		Debug.Log("Entering the forest");
		titleUI.SetActive(true);
	}

	private void OnTriggerEnter2D(Collider2D other) {

		if(other.CompareTag("Player"))
		{
			playerInRange=true;
			TriggerDialogue();
		}
	}

}
