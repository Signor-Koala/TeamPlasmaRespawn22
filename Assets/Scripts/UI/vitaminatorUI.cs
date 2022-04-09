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
        UI.SetActive(true);
        healthContainer.SetActive(false);
        VTbutton.SetActive(false);
        Time.timeScale = 0;
    }
    public void hideUI()
    {
        UI.SetActive(false);
        healthContainer.SetActive(true);
        VTbutton.SetActive(true);
        Time.timeScale = 1;
    }
}
