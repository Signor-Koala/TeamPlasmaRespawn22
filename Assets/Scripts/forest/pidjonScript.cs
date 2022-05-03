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
            AudioManager.instance.Play("portal_open");
            StartCoroutine("teleporter");
        }
        else if(dialogueStarted==true && dialogueManager.sentenceNumber==2 && portalFlag==false)
        {
            AudioManager.instance.Stop("hello_pidjon");
            AudioManager.instance.Play("pidjon_da_god");
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
        AudioManager.instance.FadeOut("pidjon_da_god",2);
		yield return new WaitForSeconds(2);

        AudioManager.instance.Stop("forest_normal_theme");
        AudioManager.instance.Stop("forest_danger_theme");

        CEO_script.health = GameObject.Find("Player").GetComponent<controller>().health;
        CEO_script.speed = GameObject.Find("Player").GetComponent<controller>().speed;
		Debug.Log("Exiting the forest");
        UnityEngine.SceneManagement.SceneManager.LoadScene("hotel");
	}
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && forestManager.pidjonHasArrived)
        {
            Debug.Log("Letsa Gooo!");
            AudioManager.instance.FadeOut("forest_normal_theme",0.5f);
            AudioManager.instance.FadeOut("forest_danger_theme",0.5f);
            AudioManager.instance.FadeIn("hello_pidjon",1f);
            dialogueStarted=true;
            dialogueManager.StartDialogue(dialogue);
        }
        if(other.CompareTag("Tree") && forestManager.pidjonHasArrived==true)
        {
            Destroy(other.gameObject);
        }
    }
}
