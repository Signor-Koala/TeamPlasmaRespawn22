using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class volumeSlider : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;
    public enum sliderType
    {
        music,sfx
    }
    public sliderType currentSliderType;

    private void Start() {
        if(currentSliderType==sliderType.music)
            slider.value = PlayerPrefs.GetFloat("musicLevel",1.5f);
        else if(currentSliderType==sliderType.sfx)
            slider.value = PlayerPrefs.GetFloat("SFxLevel", 1.5f);
    }
    public void setMusicLvl(float sliderValue)
    {
        mixer.SetFloat("musicLevel",Mathf.Log10(sliderValue)*20);
        CEO_script.musicLevel = Mathf.Log10(sliderValue)*20;
        PlayerPrefs.SetFloat("SFxLevel",sliderValue);
    }
    public void setSfxLvl(float sliderValue)
    {
        mixer.SetFloat("sfxLevel",Mathf.Log10(sliderValue)*20);
        PlayerPrefs.SetFloat("musicLevel",sliderValue);
    }
}
