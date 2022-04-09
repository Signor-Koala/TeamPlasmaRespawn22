using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pidjonScript : MonoBehaviour
{
    public Dialogue dialogue;
	DialogueManager dialogueManager;
    public portalScript portalController;
    public forestManager forestManager;
    bool dialogueStarted = false;
    bool dialogueFinished = false;
    bool portalFlag=false;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        portalController.gameObject.SetActive(false);

    }

    private void Update() {
        if(dialogueStarted==true && dialogueManager.sentenceNumber==3 && portalFlag==false)
        {
            Debug.Log("Portal Spawned");
            portalController.gameObject.SetActive(true);
            portalController.isActivated=true;
            portalFlag=true;
			portalController.activatePortal();
            StartCoroutine("teleporter");
        }
        if(dialogueManager.currentDialogueState==DialogueManager.dialogueState.dialogueStarted && dialogueStarted==true)
		{
			if(Input.GetKeyDown("z"))
				dialogueManager.DisplayNextSentence();	
		}
    }

    IEnumerator teleporter()
	{
		yield return new WaitForSeconds(2);
		dialogueManager.DisplayNextSentence();
		Debug.Log("Exiting the forest");
        UnityEngine.SceneManagement.SceneManager.LoadScene("hotel");
	}
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Letsa Gooo!");
            dialogueStarted=true;
            dialogueManager.StartDialogue(dialogue);
        }
        if(other.CompareTag("Tree") && forestManager.pidjonHasArrived==true)
        {
            Destroy(other.gameObject);
        }
    }
}
