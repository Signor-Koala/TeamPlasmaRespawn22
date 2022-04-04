using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public Transform plr;
    public float reload = 0.5f;
    public int damage = 20;
    public bool enemyBullet = false;
    public bool explosive = false;
    public float explosiveRange = 4f;

    private Vector2 speedVec;
    private Rigidbody2D rb;
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        speedVec = new Vector2(speed*(rb.position.x - plr.position.x),speed*(rb.position.y - plr.position.y));
        rb.velocity = speedVec;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (enemyBullet)
        {
            controller player = col.GetComponent<controller>();
            if (player != null) 
            {
                player.takeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (explosive)
        {
            Collider2D[] hitenemy = Physics2D.OverlapCircleAll(this.transform.position, explosiveRange);
        
            foreach (Collider2D enemy in hitenemy)
            {
                EnemyScript enem = enemy.GetComponent<EnemyScript>();
                if (enem != null)
                {
                    enem.takeDamage(damage);
                }
            }
            Destroy(gameObject);
        }
        else
        {
            EnemyScript enemy = col.GetComponent<EnemyScript>();
            if (enemy != null) 
            {
                enemy.takeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
