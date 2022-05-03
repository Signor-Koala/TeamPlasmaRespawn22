using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hotelmanager : MonoBehaviour
{
    bool inDanger;
    float lastDangerTime=0;
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
        if(CEO_script.dangerLevel>0 && Time.time - lastDangerTime > 0.33f)
        {
            AudioManager.instance.FadeIn("hotel_danger_theme",1f);
            AudioManager.instance.FadeOut("hotel_normal_theme",1f);
            lastDangerTime=Time.time;
            inDanger=true;
        }
        else if(CEO_script.dangerLevel==0 && Time.time - lastDangerTime > 0.33f)
        {
            AudioManager.instance.FadeOut("hotel_danger_theme",1f);
            AudioManager.instance.FadeIn("hotel_normal_theme",1f);
            lastDangerTime=Time.time;
            inDanger=false;
        }
    }
}
