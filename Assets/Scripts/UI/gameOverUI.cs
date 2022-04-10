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
        UnityEngine.SceneManagement.SceneManager.LoadScene("main_menu");
    }
}
