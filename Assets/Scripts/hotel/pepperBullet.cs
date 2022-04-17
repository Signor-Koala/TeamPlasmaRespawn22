using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pepperBullet : MonoBehaviour
{
    public int dps=20;
    public float lifeDur=1;
    float startTime;
    public Vector3 target;
    Rigidbody2D rb;
    Animator anim;
    bool isFalling=false, exploded=false;
    void Start()
    {
        startTime = Time.time;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Time.time - startTime >= lifeDur/2 && !isFalling)
        {
            gameObject.transform.position = new Vector3(target.x,transform.position.y,0);
            rb.velocity = Vector2.zero;
            rb.velocity = new Vector3(0,-2*(transform.position.y - target.y)/lifeDur,0);
            isFalling = true;
        }
        if(transform.position==target && isFalling)
        {
            rb.velocity = Vector2.zero;
            explosion();
        }
        else if(Time.time - startTime >= lifeDur)
        {
            rb.velocity = Vector2.zero;
            explosion();
        }

    }

    void explosion()
    {
        anim.SetTrigger("explosion");
        exploded=true;
    }

    public void explosionSound()
    {
        AudioManager.instance.Play("pepper_release");
    }

    public void destroy()
    {
        Destroy(gameObject);
    }

    float lastDmgTime=0;
    private void OnTriggerStay2D(Collider2D other) {
        if(exploded && other.CompareTag("Player"))
        {
            if(Time.time - lastDmgTime >=1)
            {
                controller player = other.GetComponent<controller>();
                if (player != null) 
                {
                    player.TakeDamage(dps);
                    lastDmgTime = Time.time;
                }
            }
            
        }
    }
    
}


