using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pepperCansDown : MonoBehaviour
{
    public float speed = 3f;
    public float bulletLifetime = 5f;
    
    
    private Rigidbody2D rb;
    private float bulletLife = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0,-speed);
        bulletLife = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        pepperClouds destination = col.GetComponent<pepperClouds>();

        if (destination != null)
        {
            destination.cloudActive = true;
            Destroy(gameObject);
        }
    }
}
