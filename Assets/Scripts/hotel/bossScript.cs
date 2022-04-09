using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossScript : MonoBehaviour
{
    public int maxhealth = 1000;
    public int health = 1000;
    public int damage = 20;
    public float speed = 40f;
    public float attackRangeMelee = 1f;
    public float attackRangePepper = 3f;
    public LayerMask playerLayer;

    public GameObject enemyMinion;
    public GameObject projectile;
    public GameObject clouds;
    public Transform spawnPoint;
    public Transform pepperPoint;
    
    private Transform plr;
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

    // Start is called before the first frame update
    void Start()
    {
        plr = GameObject.Find("Player").transform;
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

    public void IdleStage2()
    {
        nextAttack = Random.Range(1, 5);
        switch (nextAttack)
        {
            case 1:
                Debug.Log("Idle");
                break;
            case 2:
                Debug.Log("spawnAngry");
                spawnAngry();
                break;
            case 3:
                Debug.Log("dashAttackFast");
                anim.SetTrigger("DashAttack");
                break;
            case 4:
                Debug.Log("pepperCan");
                break;
        }
    }
    
    

    public void attackSpawn() //receives trigger from animator
    {
        Instantiate(enemyMinion, spawnPoint.position, rot);
    }

    public void spawnAngry() //receives trigger from animator
    {
        Instantiate(enemyMinion, spawnPoint.position + Vector3.right, rot);
        Instantiate(enemyMinion, spawnPoint.position + Vector3.left, rot);
    }
    
    public void dashAttackFast() //receives trigger from animator
    {
        destination = plr.transform.position;
        Vector3 position = transform.position;
        direction = (destination - position) / ((destination - position).magnitude);
        
        while ((destination-transform.position).magnitude >= destinationLeniency)
        {
            rbd.transform.position += direction * (5 * speed * Time.deltaTime);
        }
        anim.SetTrigger("Smash");
    }
    
    void Hit() // receives trigger from animator
    {
        Collider2D[] hitplayer = Physics2D.OverlapCircleAll(transform.position, attackRangeMelee, playerLayer);
        
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
    
    public void DestroyBoss()
    {
        Destroy(gameObject);
    }
}
