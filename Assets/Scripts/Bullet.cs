using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public Transform plr;
    public float reload = 0.5f;
    public int damage = 20;

    private Vector2 speedVec;
    private Rigidbody2D rb;
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        speedVec = new Vector2(speed*(rb.position.x - plr.position.x),speed*(rb.position.y - plr.position.y));
        rb.velocity = speedVec;
    }

}
