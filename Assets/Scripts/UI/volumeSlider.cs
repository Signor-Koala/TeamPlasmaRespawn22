using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class volumeSlider : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;
    enum sliderType
    {
        music,sfx
    }
    sliderType currentSliderType;

    private void Start() {
        if(currentSliderType==sliderType.music)
            slider.value = CEO_script.musicLevel;
        else if(currentSliderType==sliderType.sfx)
            slider.value=CEO_script.SFxLevel;
    }
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
