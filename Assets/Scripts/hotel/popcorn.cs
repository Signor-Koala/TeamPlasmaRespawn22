using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popcorn : MonoBehaviour
{
    public int damage = 20;
    Rigidbody2D rb;
    public Vector3 gravityVector;
    private void Start() {
        rb = this.GetComponent<Rigidbody2D>();
    }
    private void Update() {
        rb.AddForce(gravityVector);
    }

    private void OnTriggerEnter2D(Collider2D col) {
            if(col.CompareTag("Player"))
            {
                controller player = col.GetComponent<controller>();
                if (player != null) 
                {
                    player.TakeDamage(damage);
                }
                Destroy(gameObject);
            }
            else if(col.CompareTag("OutLands"))
            {
                Destroy(gameObject);
            }
            
    }
}
