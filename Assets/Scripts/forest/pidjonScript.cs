using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pidjonScript : MonoBehaviour
{
    public Dialogue dialogue;
	DialogueManager dialogueManager;
    portalScript portalController;
    bool dialogueStarted = false;
    bool dialogueFinished = false;
    bool portalFlag=false;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void Update() {
        if(dialogueStarted==true && dialogueManager.sentenceNumber==3 && portalFlag==false)
        {
            Debug.Log("Portal Spawned");
            portalController.isActivated=true;
            portalFlag=true;
			portalController.activatePortal();
            StartCoroutine("teleporter");
        }
    }

    IEnumerator teleporter()
	{
		yield return new WaitForSeconds(2);
		dialogueManager.DisplayNextSentence();
		Debug.Log("Exiting the forest");
	}
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Letsa Gooo!");
            dialogueStarted=true;
            dialogueManager.StartDialogue(dialogue);
        }
    }
}
