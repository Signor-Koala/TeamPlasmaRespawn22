using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameOverUI : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI score;
    void Start()
    {
        int currentScore = CEO_script.totalKillScore + CEO_script.money;
        score.text = "Score - " + currentScore.ToString();
    }

    public void returnToMenu()
    {
        AudioManager.instance.Play("buttonClick1");
        AudioManager.instance.Stop("game_over");
        UnityEngine.SceneManagement.SceneManager.LoadScene("main_menu");
    }
    public void playButtonHover()
    {
        AudioManager.instance.Play("buttonHover1");
    }
}
