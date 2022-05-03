using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject inactivePortal, activePortal,MainUI,HighScoreUI,OptionsUI;
    Animator blindingAnim,portalAnim;
    [SerializeField] TMPro.TextMeshProUGUI highscoreText;
    void Start()
    {
        Time.timeScale=1;
        MainUI.SetActive(true);
        HighScoreUI.SetActive(false);
        OptionsUI.SetActive(false);
        blindingAnim = GameObject.Find("White Screen").GetComponent<Animator>();
        portalAnim = GameObject.Find("portal_inactive").GetComponent<Animator>();
        activePortal.SetActive(false);
        inactivePortal.SetActive(true);
        AudioManager.instance.Play("main_menu_theme");
    }

    public void newRun()
    {
        AudioManager.instance.FadeOut("main_menu_theme",0.66f);
        MainUI.SetActive(false);
        inactivePortal.SetActive(false);
        activePortal.SetActive(true);
        blindingAnim.SetTrigger("portalTrigger");

        AudioManager.instance.Play("portal_open");

        if(CEO_script.firstTimeInSession==1)
            CEO_script.currentGameState = CEO_script.gameState.preForestLevel;
        CEO_script.health = 100;
        CEO_script.speed = 1f;
        StartCoroutine(sceneLoadDelay());

    }
    public void HighScores()
    {
        AudioManager.instance.Play("buttonClick");
        MainUI.SetActive(false);
        HighScoreUI.SetActive(true);
        highscoreText.text = "Highscore - " + PlayerPrefs.GetInt("Highscore").ToString();
    }
    public void Options()
    {
        AudioManager.instance.Play("buttonClick");
        MainUI.SetActive(false);
        OptionsUI.SetActive(true);
    }
    public void back()
    {
        AudioManager.instance.Play("buttonClick");
        MainUI.SetActive(true);
        HighScoreUI.SetActive(false);
        OptionsUI.SetActive(false);
    }
    public void quit()
    {
        AudioManager.instance.Play("buttonClick");
        CEO_script.quitGame();
        Debug.Log("Quitting Application...");
        Application.Quit();
    }

    public void hardResetConfirm() 
    {
        AudioManager.instance.Play("buttonClick");
        questionBox.Instance.showQuestion("Are you really sure? You will lose all of your progress!", ()=>{ hardReset(); }, ()=>{} );
    }
    public void hardReset()
    {
        PlayerPrefs.DeleteAll();
        CEO_script.powerupSpawned = new int[4];
        CEO_script.money = 0;
        CEO_script.health = 100;
        CEO_script.speed = 1f;
        CEO_script.firstLoad = 1;
        CEO_script.Highscore = 0;
        CEO_script.musicLevel = 0.5f;
        CEO_script.SFxLevel = 0.5f;
    }

    IEnumerator sceneLoadDelay()
    {
        yield return new WaitForSeconds(2);
        AudioManager.instance.Stop("main_menu_theme");
        if(CEO_script.firstTimeInSession==1)
        {
            CEO_script.health=100;
            CEO_script.speed=1;
            UnityEngine.SceneManagement.SceneManager.LoadScene("forest_start");
        }
        else if(CEO_script.firstTimeInSession==0)
        {
            CEO_script.health=100;
            CEO_script.speed=1;
            UnityEngine.SceneManagement.SceneManager.LoadScene("forest");
        }
    }

    public void playButtonCrack()
    {
        portalAnim.SetFloat("Blend",1);
        AudioManager.instance.Play("portal_crack");
    }
    public void playButtonOut()
    {
        portalAnim.SetFloat("Blend",0);
    }
    public void playButtonHover()
    {
        AudioManager.instance.Play("buttonHover1");
    }
}
