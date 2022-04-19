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
        if (CEO_script.money >= healthPrice && playerScript.health < CEO_script.healthLimit)
        {
            AudioManager.instance.Play("buttonClick1");
            AudioManager.instance.Play("upgrade");
            PlayerPrefs.SetInt("money",CEO_script.money - healthPrice);
            CEO_script.money -= healthPrice;
            PlayerPrefs.SetInt("health", CEO_script.health + healthAmount);
            CEO_script.health += healthAmount;
            playerScript.health += healthAmount;
        }
        else if(CEO_script.money < healthPrice) {
            AudioManager.instance.Play("buttonClick1_error");
            Debug.Log("Not Enough Vital Extract!");
        }
        else if(healthAmount >= CEO_script.healthLimit)
        {
            AudioManager.instance.Play("buttonClick1_error");
            Debug.Log("Max Health Reached!");
        }
    }
    
    public void speedUpgrade()
    {
        if (CEO_script.money >= speedPrice && playerScript.speed < CEO_script.speedLimit)
        {
            AudioManager.instance.Play("buttonClick1");
            AudioManager.instance.Play("upgrade");
            PlayerPrefs.SetInt("money",CEO_script.money - speedPrice);
            CEO_script.money -= speedPrice;
            PlayerPrefs.SetFloat("speed", CEO_script.speed + speedAmount);
            CEO_script.speed += speedAmount;
            playerScript.speed += speedAmount;
        }
        else if(CEO_script.money < speedPrice) {
            AudioManager.instance.Play("buttonClick1_error");
            Debug.Log("Not Enough Vital Extract!");
        }
        else if(CEO_script.speed >= CEO_script.speedLimit)
        {
            AudioManager.instance.Play("buttonClick1_error");
            Debug.Log("Max Speed Reached!");
        }
    }
}
