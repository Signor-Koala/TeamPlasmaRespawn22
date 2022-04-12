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
    public GameObject projectile;
    public GameObject clouds;
    public Transform spawnPoint;
    public Transform pepperPoint;
    public GameObject PlayerObject;
    
    public Transform plr;
    private Vector3 destination;
    private Vector3 direction;
    private float destinationLeniency = 0.1f;
    private float lastAttack = 0;
    private Quaternion rot = Quaternion.Euler(0, 0, 0);
    private Rigidbody2D rbd;
    private bool isAngry = false;
    private bool isAttacking = false;
    private int nextAttack = 0;
    private Animator anim;
    public GameObject[] enemies;
    [SerializeField] GameObject bossBullet;
    public float[] phase2LastAttackTime = new float[6];

    // Start is called before the first frame update
    void Start()
    {
        rbd = this.GetComponent<Rigidbody2D>();
        health = maxhealth;
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IdleStage1()
    {
        nextAttack = Random.Range(1, 4);
        switch (nextAttack)
        {
            case 1: case 2:
                Debug.Log("Idle");
                break;
            case 3:
                Debug.Log("attackSpawn");
                anim.SetTrigger("Spawn");
                break;
        }
    }

    public void IdleStage2()    //decides the next action
    {
        nextAttack = Random.Range(1, 7);
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
                if(Time.time - phase2LastAttackTime[3]>8)
                {
                    Debug.Log("Spawn");
                    anim.SetTrigger("Spawn");
                    phase2LastAttackTime[3]=Time.time;
                }
                break;
            case 5:
                if(Time.time - phase2LastAttackTime[4]>8)
                {
                    Debug.Log("Pepper Blasts");
                    phase2LastAttackTime[4]=Time.time;
                }
                break;
            case 6:
                if(Time.time - phase2LastAttackTime[5]>16)
                {
                    Debug.Log("MeleeMania!");
                    anim.SetTrigger("spawnHorde");
                    phase2LastAttackTime[5]=Time.time;
                }
                break;
            case 7:
                Debug.Log("Bullet Attack");         //increasing the probability of bullet attack
                anim.SetTrigger("bulletAttack");
                break;
        }
    }
    
    public void bulletAttack()  //recieves trigger from animator
    {
        for (int i = 0; i < 16; i++)
        {
            GameObject newBossBullet = Instantiate(bossBullet);
            newBossBullet.transform.position = spawnPoint.position + new Vector3(0,-0.2f,0) + new Vector3(Mathf.Cos(Mathf.PI*i/8),Mathf.Sin(Mathf.PI*i/8))*0.125f;
        }
    }

    public void attackSpawn() //receives trigger from animator
    {
        Instantiate(enemyMinion, spawnPoint.position, rot);
    }

    public void spawnAngry() //receives trigger from animator
    {
        int j = 2;
        for (int i = 0; i < j; i++)
        {
            Instantiate(enemies[Random.Range(0,3)], spawnPoint.position + new Vector3(Mathf.Cos(Mathf.PI*Random.Range(0,16)*2/16),-Mathf.Sin(Mathf.PI*Random.Range(0,8)*2/16))*1f, rot);
        }
        
    }

    public void meleeMania() //receives trigger from animator
    {
        for (int i = 0; i < 8; i++)
        {
            Instantiate(enemyMinion, spawnPoint.position + new Vector3(Mathf.Cos(Mathf.PI*2*i/8),Mathf.Sin(Mathf.PI*2*i/8))*0.5f, rot);
        }
        
    }
    
    public void dashAttackFast() //receives trigger from animator
    {
        rbd.velocity = (transform.position - plr.position)/0.2f;
        
        StartCoroutine(dashing());
    }

    IEnumerator dashing()
    {
        while (true)
        {
            if((transform.position+new Vector3(0,-0.2f,0) - plr.position).magnitude <= 0.2f)
            {
                rbd.velocity=Vector2.zero;
                break;
            }
        }
        yield return new WaitForSeconds(0.2f);
        anim.SetTrigger("Smash");
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
        Debug.Log("Boss health : "+health);
        if(dam > 30)
            anim.SetTrigger("Damage");

        if (health <= (maxhealth / 2) && !isAngry)
        {
            anim.SetTrigger("Angry");
            isAngry = true;
        }
        if (health <= 0)
        {
            rbd.velocity = new Vector2(0, 0);
            anim.SetTrigger("Die");
            CEO_script.currentGameState=CEO_script.gameState.bossBattleCleared;
            
            this.enabled = false;
        }
    }

    public void pepperAttack() //receives trigger from animator
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
    }
    
    
}
