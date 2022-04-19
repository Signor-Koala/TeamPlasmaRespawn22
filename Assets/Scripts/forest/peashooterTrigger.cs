using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class peashooterTrigger : MonoBehaviour
{
    public Dialogue dialogue;
	DialogueManager dialogueManager;
    [SerializeField] controller player;
    [SerializeField] PowerUp peaPowerUp;
    bool eventFlag=false, playerInRange=false;
    

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void Update() {

        
        if(dialogueManager.sentenceNumber>=2 && eventFlag==false && playerInRange && peaPowerUp !=null && CEO_script.firstLoad==1)
        {
            player.currenProj=peaPowerUp.projectile;
            CEO_script.activePowerUp = peaPowerUp.projectile;
            CEO_script.firstLoad=0;
            Destroy(peaPowerUp.gameObject);
            AudioManager.instance.Play("powerUp1");
            eventFlag=true;
        }
    }

   private void OnTriggerEnter2D(Collider2D other) {
       if(other.CompareTag("Player") && peaPowerUp !=null && CEO_script.firstLoad==1)
       {
           playerInRange=true;
           FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
       }
       else if(other.CompareTag("Player") && peaPowerUp !=null)
       {
            player.currenProj=peaPowerUp.projectile;
            CEO_script.activePowerUp = peaPowerUp.projectile;
            Destroy(peaPowerUp.gameObject);
       }
   }
    
}
