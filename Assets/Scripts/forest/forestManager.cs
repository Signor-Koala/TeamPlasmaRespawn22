using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class forestManager : MonoBehaviour
{
    float lastRerollTime=0;
    float lastNotifyTime=0;
    public static Vector3 spawnPoint;
    public GameObject pidjon;
    public GameObject player;
    public bool pidjonHasArrived = false;
    Rigidbody2D pidjonRB;
    Animator pidjonAnim;
    public Dialogue dialogue;
	DialogueManager dialogueManager;
    bool PidjonCame = false;
    void Start()
    {
        CEO_script.currentGameState = CEO_script.gameState.forestLevel; 
        player.GetComponent<controller>().currenProj = player.GetComponent<controller>().projList[0];
        pidjonRB = GameObject.Find("Pidjon").GetComponent<Rigidbody2D>();
        pidjonAnim = GameObject.Find("Pidjon").GetComponent<Animator>();
        pidjon.SetActive(false);
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void Update() {
        
        if(Time.time - lastNotifyTime > 2f)
        {
            lastNotifyTime = Time.time;
            Debug.Log("Danger Level = " + CEO_script.dangerLevel);
        }
        if(Time.time - lastRerollTime > 10f && CEO_script.dangerLevel==0 && pidjonHasArrived==false)
        {
            lastRerollTime = Time.time;
            if(Random.Range(0,100)<=CEO_script.totalKillScore)
            {
                Debug.Log("HELLO PIDJON!");
                dialogueManager.StartDialogue(dialogue);
                pidjon.SetActive(true);
                
                PidjonCame=true;
                
                pidjonAnim.SetBool("isFlying",true);
                pidjon.transform.position = player.transform.position + new Vector3(-2f,0.5f,0);
                pidjonRB.velocity = new Vector3(1,0,0);
                StartCoroutine("setExit");
            }
        }
        if(PidjonCame==true)
		{
			if(Input.GetKeyDown("z"))
				dialogueManager.DisplayNextSentence();	
		}
    }

    private IEnumerator setExit()
    {
        yield return new WaitForSeconds(2.5f);
        dialogueManager.DisplayNextSentence();	
        yield return new WaitForSeconds(4f);
        dialogueManager.DisplayNextSentence();
        yield return new WaitForSeconds(3f);
        dialogueManager.DisplayNextSentence();
        pidjonRB.velocity = new Vector3(0,0,0);
        pidjon.transform.position = forestManager.spawnPoint;
        pidjonAnim.SetBool("isFlying",false);
        pidjonHasArrived = true;
        PidjonCame=false;
        pidjonRB.constraints = RigidbodyConstraints2D.FreezePositionX;
        pidjonRB.constraints = RigidbodyConstraints2D.FreezePositionY;
    }


}  
