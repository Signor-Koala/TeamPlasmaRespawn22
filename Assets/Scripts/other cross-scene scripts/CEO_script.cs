using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class CEO_script : MonoBehaviour 
{
    public static CEO_script instance;
    public static GameObject[] powerups;
    public static int[] powerupSpawned;
    public static GameObject activePowerUp;
    public AudioMixer mixer;
    public static int[] enemiesKilled = new int[3];
    public static int totalKillScore=0;
    public static int dangerLevel=0, lastDangerLevel=0;
    public enum gameState
    {
        preForestLevel,forestLevel, forestLevelCleared,withDaPidgon,
        preHotelLevel,hotelLevel, hotelLevelCleared,
        bossBattle, bossBattleCleared, gameOver
    }
    public static gameState currentGameState;
    
    public static int money = 0;
    public static int health = 100, healthLimit = 320;
    public static float speed = 1.0f, speedLimit = 2f;
    public static int firstLoad,firstTimeInSession=1;
    public static int Highscore;
    public static float musicLevel, SFxLevel;
    public static string nextLevel="Main_menu";
    private void Awake()
    { 
        if (instance != null)
        { 
            Destroy(gameObject); 
        }
        else
        { 
            instance = this; 
            DontDestroyOnLoad(gameObject); 
        } 
    }

    private void Start() 
    { 
        powerupSpawned = new int[4];
        money = PlayerPrefs.GetInt("money", 0);
        health = PlayerPrefs.GetInt("health", 100);
        speed = PlayerPrefs.GetFloat("speed", 1.0f);
        firstLoad = PlayerPrefs.GetInt("firstload",1);
        Highscore = PlayerPrefs.GetInt("Highscore",0);
        musicLevel = PlayerPrefs.GetFloat("musicLevel", 0.75f);
        SFxLevel = PlayerPrefs.GetFloat("SFxLevel", 0.75f);
        lastDangerLevel=0;
        if(firstLoad==1)
            Highscore=0;
        
        mixer.SetFloat("musicLevel",musicLevel);
        mixer.SetFloat("sfxLevel",SFxLevel);
    }

    

    public static void transition(int newhealth)
    {
        health = newhealth;
    }

    public static void gameOver()
    {
        PlayerPrefs.SetInt("money",money);
        if(totalKillScore>Highscore)
            Highscore=totalKillScore;
        PlayerPrefs.SetInt("Highscore",Highscore);

        UnityEngine.SceneManagement.SceneManager.LoadScene("game_over");
        //other things
    }
    

    private void Update() {
        
        //Debug.Log(1/Time.deltaTime);
        if(dangerLevel<0)
            dangerLevel=0;
    }

    public void setVolume()
    {
        mixer.SetFloat("musicLevel",musicLevel);
        mixer.SetFloat("sfxLevel",SFxLevel);
    }

    public static void loadLevel(string levelName)
    {
        nextLevel = levelName;
        UnityEngine.SceneManagement.SceneManager.LoadScene("loading_scene");
    }

    public static void quitGame()
    {
        PlayerPrefs.SetInt("HighScore",Highscore);
        PlayerPrefs.SetInt("money",money);
        PlayerPrefs.SetFloat("musicLevel", musicLevel);
        PlayerPrefs.SetFloat("SFxLevel", SFxLevel);
    }
}