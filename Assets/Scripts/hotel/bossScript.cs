using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossScript : MonoBehaviour
{
    public int maxhealth = 1000;
    public int health = 1000;
    public int damage = 20;
    public float speed = 40f;
    public float attackDistance = 3f;
    public LayerMask playerLayer;
    
    public GameObject enemyMinion;
    public GameObject projectile;
    public Transform spawnPoint;
    
    private Transform plr;
    private Vector3 destination;
    private Vector3 direction;
    private float destinationLeniency = 0.5f;
    private float lastAttack = 0;
    private Quaternion rot = Quaternion.Euler(0, 0, 0);
    private Rigidbody2D rbd;
    private bool isAngry = false;

    // Start is called before the first frame update
    void Start()
    {
        plr = GameObject.Find("Player").transform;
        rbd = this.GetComponent<Rigidbody2D>();
        health = maxhealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void attackSpawn() //receives trigger from animator
    {
        Instantiate(enemyMinion, spawnPoint.position, rot);
    }

    public void dashAttack() //receives trigger from animator
    {
        destination = plr.transform.position;
        Vector3 position = transform.position;
        direction = (destination - position) / ((destination - position).magnitude);
        
        while ((destination-transform.position).magnitude <= destinationLeniency)
        {
            rbd.transform.position += direction * speed * Time.deltaTime;
        }
        //send trigger to animator
    }
    
    public void dashAttackFast() //receives trigger from animator
    {
        destination = plr.transform.position;
        Vector3 position = transform.position;
        direction = (destination - position) / ((destination - position).magnitude);
        
        while ((destination-transform.position).magnitude <= destinationLeniency)
        {
            rbd.transform.position += direction * (5 * speed * Time.deltaTime);
        }
        //send trigger to animator
    }
    
    void Hit() // receives trigger from animator
    {
        Collider2D[] hitplayer = Physics2D.OverlapCircleAll(transform.position, attackDistance, playerLayer);
        
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

        if (health <= (maxhealth / 2) && !isAngry)
        {
            //trigger anger
            isAngry = true;
        }
        if (health <= 0)
        {
            rbd.velocity = new Vector2(0, 0);
            //dying animation
            this.enabled = false;
        }
    }

    public void pepperAttack() //receives trigger from animator
    {
        //Unimplemented
    }
}
