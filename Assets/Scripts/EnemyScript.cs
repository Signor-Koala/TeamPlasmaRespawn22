using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int health = 50;
    public int damage = 20;
    public float reload = 1;
    public float agroDistance = 12f;
    public float speed = 380f;
    public float attackDistance = 3f;
    public int enemyType = 0; // (0,1,2) -> (Invalid,Melee,Ranged)
    public GameObject currenProj;
    public LayerMask playerLayer;

    public Transform attackPoint;
    
    private Transform plr;
    private Vector3 destination;
    private float lastAttack = 0;
    private bool isAgro = false;
    private Quaternion rot = Quaternion.Euler(0, 0, 0);

    private Rigidbody2D rbd;
    void Start()
    {
        plr = GameObject.Find("Player").transform;
        rbd = this.GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        Vector2 distance = this.transform.position - plr.position;

        if (!isAgro && distance.magnitude < agroDistance)
        {
            isAgro = true;
        }

        if (isAgro && enemyType == 1)
        {
            Melee(distance);
        }
        else if (isAgro && enemyType == 2)
        {
            Ranged();
        }

    }

    void Melee(Vector2 distance)
    {
        var positionplr = plr.position;
        var position = rbd.transform.position;
        destination = (positionplr - position) / (positionplr - position).magnitude;
        rbd.velocity = destination * (speed * Time.deltaTime);
        if ((distance.magnitude < attackDistance) && (Time.time > reload + lastAttack))
        {
            lastAttack = Time.time;
            Hit();
        }
    }

    void Ranged()
    {
        var position = rbd.transform.position;
        var positionplr = plr.position;
        destination = (position - positionplr) / (positionplr - position).magnitude;
        destination *= attackDistance;
        if (Time.time > lastAttack + reload)
        {
            FireWeapon((rbd.position- (Vector2)destination), rot);
        }
    }
    
    void FireWeapon(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(currenProj, position, rotation);
        bullet.GetComponent<Bullet>().plr = this.transform;
        reload = bullet.GetComponent<Bullet>().reload;
        lastAttack = Time.time;
    }

    void Hit()
    {
        Collider2D[] hitplayer = Physics2D.OverlapCircleAll(attackPoint.position, attackDistance, playerLayer);
        
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
        if (!isAgro) isAgro = true;

        health -= dam;
        if (health <= 0)
        {
            rbd.velocity = new Vector2(0, 0);
            //dying animation
            this.enabled = false;
        }
    }
}
