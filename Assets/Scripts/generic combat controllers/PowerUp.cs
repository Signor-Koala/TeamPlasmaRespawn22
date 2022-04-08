using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject projectile;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        controller player = col.GetComponent<controller>();
        if (player != null)
        {
            player.currenProj = projectile;
            Destroy(gameObject);
        }
        if(col.CompareTag("Tree"))
            {
                col.gameObject.SetActive(false);
            }
    }
}
