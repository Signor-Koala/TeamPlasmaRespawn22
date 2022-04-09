using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pepperClouds : MonoBehaviour
{
    public GameObject cansFromAbove;
    public float height = 5f;
    public float timeDecay = 5f;
    public int dpf = 1;
    public float cloudRange = 1f;
    public LayerMask playerLayer;
    public bool cloudActive = false;

    private Rigidbody2D rbd;
    private float timeStart = 0;
    private Quaternion rot = Quaternion.Euler(0, 0, 0);
    void Start()
    {
        rbd = GetComponent<Rigidbody2D>();
        Instantiate(cansFromAbove, rbd.transform.position + new Vector3(0,height,0), rot);
    }

    private void FixedUpdate()
    {
        if (cloudActive)
        {
            Collider2D[] hitplayer = Physics2D.OverlapCircleAll(rbd.transform.position, cloudRange, playerLayer);
        
            foreach (Collider2D player in hitplayer)
            {
                controller play = player.GetComponent<controller>();
                if (play != null)
                {
                    play.TakeDamage(dpf);
                }
            }

            if (Time.time <= timeStart + timeDecay)
            {
                cloudActive = false;
                Destroy(gameObject);
            }
            
        }
    }
    
}
