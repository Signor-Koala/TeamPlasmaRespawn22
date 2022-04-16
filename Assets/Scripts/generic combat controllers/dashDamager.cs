using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dashDamager : MonoBehaviour
{
    public int dashDamage=30;
    private void OnTriggerEnter2D(Collider2D col) {        //Damaging enemies while dashing
        
        EnemyScript enemy = col.GetComponent<EnemyScript>();
            if (enemy != null) 
            {
                enemy.TakeDamage(dashDamage);
            }
            bossScript boss = col.GetComponent<bossScript>();
            if (boss != null)
            {
                boss.TakeDamage(dashDamage);
            }
    }
}
