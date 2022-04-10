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
        pauseMenuUI.SetActive(true);
        pauseButton.SetActive(false);
        VEamt.text = CEO_script.money.ToString();
        Time.timeScale = 0;
    }
    public void resumeGame()
    {
        pauseMenuUI.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
    }

    public void endRun()
    {
        Debug.Log("confirmation");
        endConfirmUI.SetActive(true);
    }

    public void confirmEnd()
    {
        Debug.Log("Exiting...");
        UnityEngine.SceneManagement.SceneManager.LoadScene("main_menu");
    }
    public void notConfirmEnd()
    {
        endConfirmUI.SetActive(false);
    }

}
