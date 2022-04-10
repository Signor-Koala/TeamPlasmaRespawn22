using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject inactivePortal, activePortal,MainUI,HighScoreUI,OptionsUI;
    Animator blindingAnim;
    [SerializeField] TMPro.TextMeshProUGUI highscoreText;
    void Start()
    {
        Time.timeScale=1;
        MainUI.SetActive(true);
        HighScoreUI.SetActive(false);
        OptionsUI.SetActive(false);
        blindingAnim = GameObject.Find("White Screen").GetComponent<Animator>();
        activePortal.SetActive(false);
        inactivePortal.SetActive(true);
    }

    public void newRun()
    {
        MainUI.SetActive(false);
        inactivePortal.SetActive(false);
        activePortal.SetActive(true);
        blindingAnim.SetTrigger("portalTrigger");
        if(CEO_script.firstLoad==1)
            CEO_script.currentGameState = CEO_script.gameState.preForestLevel;
        CEO_script.health = 100;
        CEO_script.speed = 1f;
        StartCoroutine(sceneLoadDelay());

    }
    public void HighScores()
    {
        MainUI.SetActive(false);
        HighScoreUI.SetActive(true);
        highscoreText.text = "Highscore - " + CEO_script.Highscore.ToString();
    }
    public void Options()
    {
        MainUI.SetActive(false);
        OptionsUI.SetActive(true);
    }
    public void back()
    {
        MainUI.SetActive(true);
        HighScoreUI.SetActive(false);
        OptionsUI.SetActive(false);
    }
    public void quit()
    {
        CEO_script.quitGame();
        Debug.Log("Quitting Application...");
        Application.Quit();
    }

    IEnumerator sceneLoadDelay()
    {
        yield return new WaitForSeconds(2);
        if(CEO_script.firstLoad==1)
            UnityEngine.SceneManagement.SceneManager.LoadScene("forest_start");
        else if(CEO_script.firstLoad==0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("forest");
        }
    }
}
