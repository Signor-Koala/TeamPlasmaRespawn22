using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public Transform plr,boss;
    public GameObject muzzleFlashLight;
    public float reload = 0.5f;
    public int damage = 20;
    public bool enemyBullet = false;
    public bool rotating = false;
    public bool bossBullet;
    public bool explosive = false;
    public float explosiveRange = 4f;
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public float bulletLifetime = 10f;
    public float knockBackVal = 1, recoilVal=1;

    private Vector2 speedVec;
    private Rigidbody2D rb;
    Animator anim,cameraAnim;
    private float bulletLife = 0;
    void Start()
    {
        GameObject newlight= Instantiate(muzzleFlashLight);
        newlight.transform.position = transform.position;

        rb = this.GetComponent<Rigidbody2D>();
        cameraAnim = FindObjectOfType<Camera>().GetComponent<Animator>();
        if(this.GetComponent<Animator>() != null)
            anim = this.GetComponent<Animator>();

        if(GameObject.Find("Boss") != null)
            boss = GameObject.Find("Boss").GetComponent<Transform>();

        var position = rb.position;
        var positionplr = plr.position;


        if(!bossBullet && !enemyBullet)
            speedVec = speed*(position-(Vector2)positionplr).normalized;
        else if(enemyBullet && !bossBullet)
            speedVec = speed*((position - (Vector2)positionplr).normalized+Vector2.Perpendicular(position - (Vector2)positionplr).normalized*UnityEngine.Random.Range(-0.1f,0.1f)).normalized;
        //This above code doesnt really need to be there since all the calculations for the 
        //bullet trajectory have been done in the enemy script itself and forwarded to
        //the bullet script in the exact same way as the player script
        //in the eyes of the the bullet (in my initial script) a player and enemy are the exact same thing
        //nevertheless, i will keep this here and assume you had good reason for putting
        //this here :harold:
        //it shouldn't cause too much problems since it only executes once
        //but still, complicates things :harold:
        else if(boss != null && bossBullet)
        {
            speedVec = speed*(plr.position+new Vector3(0,-0.2f,0) - transform.position).normalized;
        }

        transform.rotation = Quaternion.LookRotation(Vector3.forward, -1*speedVec);  

        rb.velocity = speedVec;

        if(rotating)
            rb.angularVelocity = 360; //rotation for bananas!

        bulletLife = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (enemyBullet)
        {
            controller player = col.GetComponent<controller>();
            if (player != null) 
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (explosive)
        {
            
            Collider2D[] hitenemy = Physics2D.OverlapCircleAll(this.transform.position, explosiveRange,enemyLayer);
        
            anim.SetTrigger("explosion");
            cameraShake.instance.shakeCamera(3f,1f);

            foreach (Collider2D enemy in hitenemy)
            {
                EnemyScript enem = enemy.GetComponent<EnemyScript>();
                if (enem != null)
                {
                    enem.TakeDamage(damage,rb.velocity*knockBackVal);
                }
                bossScript boss = enemy.GetComponent<bossScript>();
                if (boss != null)
                {
                    boss.TakeDamage(damage);
                }
            }
            
            
            rb.velocity = Vector2.zero;
            
        }
        else
        {
            EnemyScript enemy = col.GetComponent<EnemyScript>();
            if (enemy != null) 
            {
                enemy.TakeDamage(damage,rb.velocity*knockBackVal);
            }
            bossScript boss = col.GetComponent<bossScript>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

        if(col.CompareTag("OutLands"))
        {
            if(!explosive)
                Destroy(gameObject);
            else{
                anim.SetTrigger("explosion");
            }
        }
    }
    
    public void destroy() 
    {
        Destroy(gameObject);
    }

    public void impact()
    {
        //cameraShake.instance.shakeCamera(3f,0.3f);
        AudioManager.instance.Play("explosion");
    }

    private void Update()
    {
        if (Time.time > bulletLife + bulletLifetime)
            Destroy(gameObject);
    }
}
