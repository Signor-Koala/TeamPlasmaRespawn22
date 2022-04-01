using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject projectile;
    
    private Rigidbody2D rb;

    private void Start()
    { 
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.name);
        
        PlayerController player = col.GetComponent<PlayerController>();
        if (player != null)
        {
            player.currenProj = projectile;
        }
        Destroy(gameObject);
    }


}
