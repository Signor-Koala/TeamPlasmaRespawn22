using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossScript : MonoBehaviour
{
    public int maxhealth = 1000;
    public int health = 1000;
    public int damage = 20;
    public float speed = 40f;
    public float attackRangeMelee = 0.5f;
    public float attackRangePepper = 3f;
    public LayerMask playerLayer;

    public GameObject enemyMinion;
    public GameObject[] enemies;
    public GameObject targetRing;
    public Transform spawnPoint;
    public Transform pepperPoint;
    public GameObject PlayerObject;
    
    public Transform plr;
    private Vector3 destination;
    private Vector3 direction;
    private float destinationLeniency = 0.15f;
    private float lastAttack = 0;
    private Quaternion rot = Quaternion.Euler(0, 0, 0);
    private Rigidbody2D rbd;
    
    private bool isAttacking = false,isAngry = false, isDashing = false;
    private int nextAttack = 0;
    private Animator anim;
    [SerializeField] GameObject bossBullet, popcorn, pepperBullet;
    public healthBar healthBar;
    public float[] phase2LastAttackTime = new float[6];

    // Start is called before the first frame update
    void Start()
    {
        rbd = this.GetComponent<Rigidbody2D>();
        healthBar.gameObject.SetActive(true);
        health = maxhealth;
        healthBar.initializeHealth(maxhealth);
        anim = this.GetComponent<Animator>();
        //anim.SetBool("isAngry",true);         //for phase 2 testing
    }

    bool healthBarCorrection = false;
    void Update()
    {
        if(!healthBarCorrection)
        {
            healthBar.setHealth(health);
            healthBarCorrection = true;
        }
    }

    public void IdleStage1()
    {
        nextAttack = Random.Range(1, 3);
        switch (nextAttack)
        {
            case 1:
                Debug.Log("Idle");
                break;
            case 2:
                Debug.Log("attackSpawn");
                anim.SetTrigger("Spawn");
                break;
        }
    }

    public void IdleStage2()    //decides the next action
    {
        nextAttack = Random.Range(1,7);
        switch (nextAttack)
        {
            case 1:
                Debug.Log("Idle");
                break;
            case 2:
                Debug.Log("Bullet Attack");
                anim.SetTrigger("bulletAttack");
                break;
            case 3:
                if(Time.time - phase2LastAttackTime[2]>4)
                {
                    Debug.Log("Dash-n-Smash!");
                    anim.SetTrigger("DashAttack");
                    phase2LastAttackTime[2]=Time.time;
                }
                break;
            case 4:
                if(Time.time - phase2LastAttackTime[3]>10)
                {
                    Debug.Log("Spawn");
                    anim.SetTrigger("Spawn");
                    phase2LastAttackTime[3]=Time.time;
                }
                break;
            case 5:
                if(Time.time - phase2LastAttackTime[4]>10)
                {
                    Debug.Log("Pepper Blasts");
                    anim.SetTrigger("pepperAttack");
                    anim.SetTrigger("pepperGunFire");
                    phase2LastAttackTime[4]=Time.time;
                }
                break;
            case 6:
                if(Time.time - phase2LastAttackTime[5]>20)
                {
                    Debug.Log("MeleeMania!");
                    anim.SetTrigger("spawnHorde");
                    phase2LastAttackTime[5]=Time.time;
                }
                break;
        }
    }
    
    public void bulletAttack()  //recieves trigger from animator
    {
        float angleDev = Random.Range(0f,1f);
        for (int i = 0; i < 16; i++)
        {
            GameObject newBossBullet = Instantiate(bossBullet);
            newBossBullet.transform.position = spawnPoint.position + new Vector3(0,-0.2f,0) + new Vector3(Mathf.Cos(Mathf.PI*i/8 + angleDev),Mathf.Sin(Mathf.PI*i/8 + angleDev))*0.125f;
        }
    }

    public void bulletReload()
    {
        if(Random.Range(0f,1f)<0.75f)
        {
            anim.SetTrigger("bulletAttack");
        }
    }

    public void serving() //receives trigger from animator
    {
        int i = Random.Range(0,3);
        switch (i)
        {
            case 0: case 1:
                Instantiate(enemyMinion, spawnPoint.position, rot);
                break;
            case 2:
                Debug.Log("Popcorns");
                popcornMania();
                break;
        }
    }

    Vector3 popcornTarget;
    private void popcornMania()
    {
        for (int i = 0; i < Random.Range(10,20); i++)
        {
            popcornTarget = plr.transform.position;
            GameObject newPopcorn = Instantiate(popcorn,spawnPoint.position,rot);
            Rigidbody2D popcornRb = newPopcorn.GetComponent<Rigidbody2D>();
            popcornRb.velocity = new Vector3(Random.Range(-1.5f,1.5f),Random.Range(-2f,2f),0);
            popcornRb.angularVelocity = 360;
            newPopcorn.GetComponent<popcorn>().gravityVector = (popcornTarget - transform.position).normalized*0.5f;
        }
    }

    public void spawnAngry() //receives trigger from animator
    {
        int j = 2;
        for (int i = 0; i < j; i++)
        {
            Instantiate(enemies[Random.Range(0,3)], spawnPoint.position + new Vector3(Mathf.Cos(Mathf.PI*Random.Range(0,16)*2/16)*0.75f,-Mathf.Sin(Mathf.PI*Random.Range(0,8)*2/16))*0.75f, rot);
        }
        
    }

    public void meleeMania() //receives trigger from animator
    {
        for (int i = 0; i < 8; i++)
        {
            Instantiate(enemyMinion, spawnPoint.position + new Vector3(Mathf.Cos(Mathf.PI*2*i/8),Mathf.Sin(Mathf.PI*2*i/8))*0.5f, rot);
        }
        
    }
    
    float dashDuration=0.1f;
    public void dashAttackFast() //receives trigger from animator
    {
        transform.position = plr.transform.position + (transform.position - plr.position).normalized*0.2f;
        anim.SetTrigger("Smash");
    }

    IEnumerator dashing()
    {
        yield return new WaitForSeconds(dashDuration);
        rbd.velocity = Vector2.zero;
        anim.SetTrigger("Smash");
        isDashing=false;
    }
    
    
    void Hit() // receives trigger from animator
    {
        Collider2D[] hitplayer = Physics2D.OverlapCircleAll(transform.position + new Vector3(0,-0.2f,0), attackRangeMelee, playerLayer);
        
        foreach (Collider2D player in hitplayer)
        {
            controller play = player.GetComponent<controller>();
            if (play != null)
            {
                play.TakeDamage(damage);
            }
        }
    }
    
    public void TakeDamage(int dam)
    {
        health -= dam;
        healthBar.setHealth(health);
        Debug.Log("Boss health : "+health);
        if(dam > 30)
            anim.SetTrigger("Damage");

        if (health <= (maxhealth / 2) && !isAngry)
        {
            anim.SetBool("isAngry",true);
            isAngry = true;
        }
        if (health <= 0)
        {
            rbd.velocity = new Vector2(0, 0);
            anim.SetTrigger("Die");
            CEO_script.currentGameState=CEO_script.gameState.bossBattleCleared;
            healthBar.gameObject.SetActive(false);
            
            this.enabled = false;
        }
    }

    /*public void pepperAttack() //receives trigger from animator
    {
        //shoot cans up
        Instantiate(projectile, pepperPoint.position, rot);
        
        //spawn cans at location
        Vector2 pos1 = new Vector2(Random.Range(0.1f * attackRangePepper, attackRangePepper),
                                   Random.Range(0.1f * attackRangePepper, attackRangePepper));
        Vector2 pos2 = new Vector2(Random.Range(0.1f * attackRangePepper, attackRangePepper),
                                   Random.Range(0.1f * attackRangePepper, attackRangePepper));
        Vector2 pos3 = new Vector2(Random.Range(0.1f * attackRangePepper, attackRangePepper),
                                   Random.Range(0.1f * attackRangePepper, attackRangePepper));
        
        Instantiate(projectile, pos1 + rbd.position, rot);
        Instantiate(projectile, pos2 + rbd.position, rot);
        Instantiate(projectile, pos3 + rbd.position, rot);
    }*/

    public void firePepperShot()
    {
        GameObject newPepperShot = Instantiate(pepperBullet, pepperPoint.position, rot);
        newPepperShot.GetComponent<Rigidbody2D>().velocity = new Vector3(0,3,0);
        newPepperShot.GetComponent<pepperBullet>().target = plr.transform.position;
        Instantiate(targetRing,plr.position,rot);
    }


    public void pepperGunReload()
    {
        if(Random.Range(0f,1f)<0.75f)
        {
            anim.SetTrigger("pepperGunFire");
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && isDashing==true)
        {
            StopCoroutine(dashing());
            isDashing=false;
            rbd.velocity=Vector2.zero;
            anim.SetTrigger("Smash");
        }
    }
    
}
