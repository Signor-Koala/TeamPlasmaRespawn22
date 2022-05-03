using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class forestManager : MonoBehaviour
{
    float lastRerollTime=0;
    float lastNotifyTime=0,lastDangerTime=0;
    public static Vector3 spawnPoint;
    public GameObject pidjon;
    public GameObject player;
    public bool pidjonHasArrived = false;
    Rigidbody2D pidjonRB;
    Animator pidjonAnim;
    public Dialogue dialogue;
	DialogueManager dialogueManager;
    bool PidjonCame = false, inDanger=false;
    void Start()
    {
        CEO_script.currentGameState = CEO_script.gameState.forestLevel; 
        player.GetComponent<controller>().currenProj = player.GetComponent<controller>().projList[0];
        pidjonRB = GameObject.Find("Pidjon").GetComponent<Rigidbody2D>();
        pidjonAnim = GameObject.Find("Pidjon").GetComponent<Animator>();
        pidjon.SetActive(false);
        dialogueManager = FindObjectOfType<DialogueManager>();
        inDanger=false;

        AudioManager.instance.Play("forest_normal_theme");
        AudioManager.instance.Play("forest_danger_theme");
        AudioManager.instance.setVolume("forest_danger_theme",0);
    }

    private void Update() {

        if(CEO_script.dangerLevel>0 && Time.time - lastDangerTime > 0.33f && PidjonCame==false)
        {
            lastDangerTime=Time.time;
            AudioManager.instance.FadeIn("forest_danger_theme",1f);
            AudioManager.instance.FadeOut("forest_normal_theme",1f);
            inDanger=true;
        }
        else if(CEO_script.dangerLevel<=0 && Time.time - lastDangerTime > 0.33f && PidjonCame==false)
        {
            lastDangerTime=Time.time;
            AudioManager.instance.FadeOut("forest_danger_theme",1f);
            AudioManager.instance.FadeIn("forest_normal_theme",1f);
            inDanger=false;
        }
        
        if(Time.time - lastNotifyTime > 2f)
        {
            lastNotifyTime = Time.time;
            Debug.Log("Danger Level = " + CEO_script.dangerLevel);
        }
        if(Time.time - lastRerollTime > 30f && CEO_script.dangerLevel==0 && pidjonHasArrived==false)    //probability of pidjon appearing
        {
            lastRerollTime = Time.time;
            if(Random.Range(0,100)<=CEO_script.totalKillScore)
            {
                Debug.Log("HELLO PIDJON!");
                AudioManager.instance.FadeIn("hello_pidjon",1f);
                dialogueManager.StartDialogue(dialogue);
                pidjon.SetActive(true);
                PidjonCame=true;
                AudioManager.instance.FadeOut("forest_danger_theme",1f);
                AudioManager.instance.FadeOut("forest_normal_theme",1f);
                
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
        AudioManager.instance.FadeOut("hello_pidjon",1f);
        pidjonRB.constraints = RigidbodyConstraints2D.FreezePositionX;
        pidjonRB.constraints = RigidbodyConstraints2D.FreezePositionY;
    }


}  
