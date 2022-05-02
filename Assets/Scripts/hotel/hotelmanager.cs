using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hotelmanager : MonoBehaviour
{
    bool inDanger;
    void Start()
    {
        inDanger=false;
        
        AudioManager.instance.Play("hotel_normal_theme");
        AudioManager.instance.Play("hotel_danger_theme");
        AudioManager.instance.setVolume("hotel_danger_theme",0);
    }

    // Update is called once per frame
    void Update()
    {
        if(CEO_script.dangerLevel>0 && inDanger==false)
        {
            AudioManager.instance.FadeIn("hotel_danger_theme",1f);
            AudioManager.instance.FadeOut("hotel_normal_theme",1f);
            inDanger=true;
        }
        else if(CEO_script.dangerLevel==0 && inDanger==true)
        {
            AudioManager.instance.FadeOut("hotel_danger_theme",1f);
            AudioManager.instance.FadeIn("hotel_normal_theme",1f);
            inDanger=false;
        }
    }
}
