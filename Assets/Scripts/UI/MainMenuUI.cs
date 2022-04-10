using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject inactivePortal, activePortal,MainUI,HighScoreUI,OptionsUI;
    Animator blindingAnim;
    void Start()
    {
        MainUI.SetActive(true);
        HighScoreUI.SetActive(false);
        OptionsUI.SetActive(false);
        blindingAnim = GameObject.Find("White Screen").GetComponent<Animator>();
    }

    public void newRun()
    {
        MainUI.SetActive(false);
        inactivePortal.SetActive(false);
        activePortal.SetActive(true);
        blindingAnim.SetTrigger("portalTrigger");
    }
    public void HighScores()
    {
        MainUI.SetActive(false);
        HighScoreUI.SetActive(true);
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
}
