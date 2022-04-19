using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vitaminatorUI : MonoBehaviour
{
    [SerializeField] GameObject UI, healthContainer,VTbutton;
    public TMPro.TextMeshProUGUI currentVE,healthCost,speedCost;
    UpgradeScript upgradeScript;

    private void Start() {
        upgradeScript = this.gameObject.GetComponent<UpgradeScript>();
        hideUI();
    }

    private void Update() {
        currentVE.text = CEO_script.money.ToString();
        healthCost.text = upgradeScript.healthPrice.ToString();
        speedCost.text = upgradeScript.speedPrice.ToString();
    }
    
    public void showUI()
    {
        AudioManager.instance.Play("buttonClick1");
        UI.SetActive(true);
        healthContainer.SetActive(true);
        VTbutton.SetActive(false);
        Time.timeScale = 0;
    }
    public void hideUI()
    {
        AudioManager.instance.Play("buttonClick1");
        UI.SetActive(false);
        healthContainer.SetActive(true);
        VTbutton.SetActive(true);
        Time.timeScale = 1;
    }
    public void playButtonHover()
    {
        AudioManager.instance.Play("buttonHover1");
    }
}
