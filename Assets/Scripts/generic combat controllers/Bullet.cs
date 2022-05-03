using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public Transform plr,boss;
    public float reload = 0.5f;
    public int damage = 20;
    public bool enemyBullet = false;
    public bool bossBullet;
    public bool explosive = false;
    public float explosiveRange = 4f;
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public float bulletLifetime = 10f;

    private Vector2 speedVec;
    private Rigidbody2D rb;
    Animator anim,cameraAnim;
    private float bulletLife = 0;
    void Start()
    {
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
            speedVec = speed*(position - (Vector2)positionplr).normalized;
        else if(boss != null && bossBullet)
        {
            speedVec = speed*(plr.position+new Vector3(0,-0.2f,0) - transform.position).normalized;
        }
        transform.rotation = Quaternion.LookRotation(Vector3.forward, -1*speedVec);
        rb.velocity = speedVec;
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
        
            foreach (Collider2D enemy in hitenemy)
            {
                EnemyScript enem = enemy.GetComponent<EnemyScript>();
                if (enem != null)
                {
                    enem.TakeDamage(damage,rb.velocity);
                }
                bossScript boss = enemy.GetComponent<bossScript>();
                if (boss != null)
                {
                    boss.TakeDamage(damage);
                }
            }
            AudioManager.instance.Play("explosion");
            anim.SetTrigger("explosion");
            cameraAnim.SetTrigger("shake");
            rb.velocity = Vector2.zero;
            
        }
        else
        {
            EnemyScript enemy = col.GetComponent<EnemyScript>();
            if (enemy != null) 
            {
                enemy.TakeDamage(damage,rb.velocity);
            }
            bossScript boss = col.GetComponent<bossScript>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

        if(col.CompareTag("OutLands") || col.CompareTag("Tree"))
        {
            if(!explosive)
                Destroy(gameObject);
            else{
                anim.SetTrigger("explosion");
                StartCoroutine(destroyDelay(2));
            }
        }
    }
    IEnumerator destroyDelay(float t) 
    {
        yield return new WaitForSeconds(t);
        destroy();
    }

    public void destroy() 
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Time.time > bulletLife + bulletLifetime)
            Destroy(gameObject);
    }
}