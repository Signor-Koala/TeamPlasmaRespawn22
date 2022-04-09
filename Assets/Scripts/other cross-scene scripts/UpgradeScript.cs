using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeScript : MonoBehaviour
{
    public int healthPrice = 50;
    public int speedPrice = 50;
    public int healthAmount = 20;
    public float speedAmount = 0.2f;
    [SerializeField] controller playerScript;

    private void Start() {
        playerScript = GameObject.Find("Player").GetComponent<controller>();
    }

    public void healthUpgrade()
    {
        if (CEO_script.money >= healthPrice)
        {
            PlayerPrefs.SetInt("money",CEO_script.money - healthPrice);
            CEO_script.money -= healthPrice;
            PlayerPrefs.SetInt("health", CEO_script.health + healthAmount);
            CEO_script.health += healthAmount;
            playerScript.health += healthAmount;
        }
    }
    
    public void speedUpgrade()
    {
        if (CEO_script.money >= speedPrice)
        {
            PlayerPrefs.SetInt("money",CEO_script.money - speedPrice);
            CEO_script.money -= speedPrice;
            PlayerPrefs.SetFloat("speed", CEO_script.speed + speedAmount);
            CEO_script.speed += speedAmount;
            playerScript.speed += speedAmount;
        }
    }
}
