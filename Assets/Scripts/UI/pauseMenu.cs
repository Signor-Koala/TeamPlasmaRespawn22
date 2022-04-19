using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI, pauseButton, endConfirmUI;
    [SerializeField] TMPro.TextMeshProUGUI VEamt;

    private void Start() {
        endConfirmUI.SetActive(false);
        resumeGame();
    }

    public void pauseGame()
    {
        AudioManager.instance.Play("buttonClick1");
        pauseMenuUI.SetActive(true);
        pauseButton.SetActive(false);
        VEamt.text = CEO_script.money.ToString();
        Time.timeScale = 0;
    }
    public void resumeGame()
    {
        AudioManager.instance.Play("buttonClick1");
        pauseMenuUI.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
    }

    public void endRun()
    {
        AudioManager.instance.Play("buttonClick1");
        Debug.Log("confirmation");
        endConfirmUI.SetActive(true);
    }

    public void confirmEnd()
    {
        AudioManager.instance.Play("buttonClick1");
        Debug.Log("Exiting...");
        UnityEngine.SceneManagement.SceneManager.LoadScene("main_menu");
    }
    public void notConfirmEnd()
    {
        AudioManager.instance.Play("buttonClick1");
        endConfirmUI.SetActive(false);
    }
    public void playButtonHover()
    {
        AudioManager.instance.Play("buttonHover1");
    }
}


