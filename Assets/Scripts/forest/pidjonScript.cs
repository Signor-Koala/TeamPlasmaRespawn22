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
    [SerializeField] GameObject portal;
    GameObject whiteScreen;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        portalController = portal.GetComponent<portalScript>();
        portalController.gameObject.SetActive(false);
        whiteScreen = GameObject.Find("White Screen");
        whiteScreen.SetActive(false);
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
        whiteScreen.SetActive(true);
        whiteScreen.GetComponent<Animator>().SetTrigger("portalTrigger");
		yield return new WaitForSeconds(2);
		dialogueManager.DisplayNextSentence();
		Debug.Log("Exiting the forest");
        UnityEngine.SceneManagement.SceneManager.LoadScene("hotel");
	}
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && forestManager.pidjonHasArrived)
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
