using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class volumeSlider : MonoBehaviour
{
    public AudioMixer mixer;

    public void setMusicLvl(float sliderValue)
    {
        mixer.SetFloat("musicLevel",Mathf.Log10(sliderValue)*20);
        CEO_script.musicLevel = Mathf.Log10(sliderValue)*20;
    }
    public void setSfxLvl(float sliderValue)
    {
        mixer.SetFloat("sfxLevel",Mathf.Log10(sliderValue)*20);
        CEO_script.SFxLevel = Mathf.Log10(sliderValue)*20;
    }
}
