using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float speed = 1f;
    public int health = 100;
    public bool died = false;
    public Vector2 looking;
    public float firepoint = 1f;
    public float reloadTime = 0.5f;

    public GameObject currenProj;
    public Camera maincam;
    
    private Vector2 direction = new Vector2(0, -1);
    private Rigidbody2D rbd;
    private float lastFireTime = 0f;
    private Quaternion rot = Quaternion.Euler(0, 0, 0);
    
    void Start()
    {
        rbd = GetComponent<Rigidbody2D>();
        PlayerPrefs.SetInt("firstload",1);
    }

    
    void FixedUpdate()
    {
        float xaxis = Input.GetAxisRaw("Horizontal");
        float yaxis = Input.GetAxisRaw("Vertical");
        
        looking = (Vector2)maincam.ScreenToWorldPoint(Input.mousePosition) - rbd.position;
        float temp = (float)Math.Sqrt(Math.Pow(looking.x, 2) + Math.Pow(looking.y, 2));
        looking.x /= (temp/firepoint);
        looking.y /= (temp/firepoint);
        
        if (Input.GetButton("Fire1") && (Time.time > lastFireTime + reloadTime))
        {
            fireWeapon((looking + rbd.position), rot);
        }

        direction.x = xaxis;
        direction.y = yaxis;

        if (direction.x != 0 || direction.y != 0)
        {
            rbd.transform.position += (Vector3)direction * speed * Time.deltaTime;
        }


    }

    void fireWeapon(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(currenProj, position, rotation);
        bullet.GetComponent<Bullet>().plr = transform;
        reloadTime = bullet.GetComponent<Bullet>().reload;
        lastFireTime = Time.time;
    }
}