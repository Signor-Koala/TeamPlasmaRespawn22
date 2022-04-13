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
    private float bulletLife = 0;
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        plr = GameObject.Find("Player").GetComponent<Transform>();

        if(GameObject.Find("Boss") != null)
            boss = GameObject.Find("Boss").GetComponent<Transform>();

        var position = rb.position;
        var positionplr = plr.position;

        if(!bossBullet && !enemyBullet)
            speedVec = speed*(position-(Vector2)positionplr).normalized;
        else if(enemyBullet && !bossBullet)
            speedVec = speed*((Vector2)positionplr - position).normalized;
        else if(boss != null && bossBullet)
        {
            speedVec = speed*(boss.position+new Vector3(0,-0.2f,0) - transform.position).normalized;
        }
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
                    enem.TakeDamage(damage);
                }
                bossScript boss = enemy.GetComponent<bossScript>();
                if (boss != null)
                {
                    boss.TakeDamage(damage);
                }
            }
            Destroy(gameObject);
        }
        else
        {
            EnemyScript enemy = col.GetComponent<EnemyScript>();
            if (enemy != null) 
            {
                enemy.TakeDamage(damage);
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
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Time.time > bulletLife + bulletLifetime)
            Destroy(gameObject);
    }
}
