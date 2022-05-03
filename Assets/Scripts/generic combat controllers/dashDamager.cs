using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dashDamager : MonoBehaviour
{
    public int dashDamage=30;
    public int dashDamageCurrent;

    private void Start() {
        dashDamageCurrent = dashDamage;
    }
    private void OnTriggerEnter2D(Collider2D col) {        //Damaging enemies while dashing
        
	    dashDamageCurrent = dashDamage *(int)((float)GetComponentInParent<controller>().stamina / GetComponentInParent<controller>().staminacap);
        EnemyScript enemy = col.GetComponent<EnemyScript>();
            if (enemy != null) 
            {
                enemy.TakeDamage(dashDamageCurrent,GetComponentInParent<Rigidbody2D>().velocity);
            }
            bossScript boss = col.GetComponent<bossScript>();
            if (boss != null)
            {
                boss.TakeDamage(dashDamageCurrent);
            }

    }
}
