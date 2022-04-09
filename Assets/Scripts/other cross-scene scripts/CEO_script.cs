using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CEO_script : MonoBehaviour 
{
    public static CEO_script instance;
    public static GameObject[] powerups;
    public static int[] powerupSpawned;
    public static GameObject activePowerUp;
    public static int[] enemiesKilled = new int[3];
    public static int totalKillScore=0;
    public static int dangerLevel=0;
    public enum gameState
    {
        preForestLevel,forestLevel, forestLevelCleared,
        preHotelLevel,hotelLevel, hotelLevelCleared,
        bossBattle, bossBattleCleared
    }
    public static gameState currentGameState;
    
    public static int money = 0;
    public static int health = 100;
    public static float speed = 1.0f;
    public static int firstLoad = 0;
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
        firstLoad = PlayerPrefs.GetInt("firstload",0);
    }

    public static void transition(int newhealth)
    {
        health = newhealth;
    }

    public static void gameOver()
    {
        PlayerPrefs.SetInt("money",money);
        //other things
    }

    private void Update() {
        for (int i = 0; i < 3; i++)
        {
            totalKillScore += i*enemiesKilled[i];
        }
        if(dangerLevel<0)
            dangerLevel=0;
    }
}