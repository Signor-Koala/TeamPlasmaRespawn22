using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endCredits : MonoBehaviour
{
    
    void Start()
    {
        
    }

    public void goToMenu()
    {
        CEO_script.loadLevel("Main_menu");
    }
}
