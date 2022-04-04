
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class controller : MonoBehaviour
{
    public float speed = 5f;
    public int health = 100;
    public bool died = false;
    public Vector2 looking;
    public float firepoint = 1f;
    public float reloadTime = 0.5f;
    public float rollDuration = 0.2f;
    public float rollReload = 1f;

    public GameObject currenProj;
    public Camera maincam;
    
    private Vector2 direction = new Vector2(0, -1);
    private Vector2 dodgeDir = new Vector2(0, 0);
    private Vector2 deviation;
    private Rigidbody2D rbd;
    private float lastFireTime = 0f;
    private float lastRollTime = 0f;
    private Quaternion rot = Quaternion.Euler(0, 0, 0);
    private bool invincible = false;

    void Start()
    {
        rbd = GetComponent<Rigidbody2D>();
    }

    
    void FixedUpdate()
    {
        float xaxis = Input.GetAxisRaw("Horizontal");
        float yaxis = Input.GetAxisRaw("Vertical");
        
        looking = rbd.position - (Vector2)maincam.ScreenToWorldPoint(Input.mousePosition);
        
        float temp = (float)Math.Sqrt(Math.Pow(looking.x, 2) + Math.Pow(looking.y, 2));
        looking.x /= (temp/firepoint);
        looking.y /= (temp/firepoint);
        
        deviation = Vector2.Perpendicular(looking);
        deviation.x /= 10;
        deviation.y /= 10;

        if (Input.GetButton("Fire1") && (Time.time > lastFireTime + reloadTime) && !(invincible))
        {
            fireWeapon((rbd.position-looking), rot);
        }

        direction.x = xaxis;
        direction.y = yaxis;

        if (direction.x != 0 || direction.y != 0)
        {
            if (Input.GetButton("Fire2") && (Time.time > lastRollTime + rollReload))
            {
                invincible = true;
                lastRollTime = Time.time;
                dodgeDir = direction;
            }

            if (invincible)
            {
                rbd.transform.position += (Vector3)dodgeDir * 5 * speed * Time.deltaTime;
                if (Time.time > lastRollTime + rollDuration)
                {
                    invincible = false;
                }
            }
            else
            {
                rbd.transform.position += (Vector3) direction * speed * Time.deltaTime;
            }
        }


    }

    void fireWeapon(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(currenProj, position, rotation);
        bullet.GetComponent<Bullet>().plr = transform;
        
        if (currenProj.name == "shotgun")
        {
            GameObject bullet1 = Instantiate(currenProj, position + (Vector3)deviation, rotation);
            bullet1.GetComponent<Bullet>().plr = transform;
            GameObject bullet2 = Instantiate(currenProj, position - (Vector3)deviation, rotation);
            bullet2.GetComponent<Bullet>().plr = transform;
        }
        
        reloadTime = bullet.GetComponent<Bullet>().reload;
        lastFireTime = Time.time;
    }

    public void takeDamage(int dam)
    {
        if (!invincible)
        {
            health -= dam;
            Debug.Log("health:" + health);
            if (health <= 0)
            {
                //dying animation
                //trigger game over
                this.enabled = false;
            }
        }
    }
}