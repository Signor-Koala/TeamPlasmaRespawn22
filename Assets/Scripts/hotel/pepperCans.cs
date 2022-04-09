using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pepperCans : MonoBehaviour
{
    public float speed = 3f;
    public float bulletLifetime = 5f;
    
    
    private Rigidbody2D rb;
    private float bulletLife = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0,speed);
        bulletLife = Time.time;
    }

    void Update()
    {
        if (Time.time > bulletLife + bulletLifetime) 
            Destroy(gameObject);
    }
}
