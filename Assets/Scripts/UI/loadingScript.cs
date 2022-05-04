using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadingScript : MonoBehaviour
{
    
    public Slider loadSlider;
    float startTime;
    private void Start() {
        loadSlider.value=0;
        StartCoroutine(asyncLoading(CEO_script.nextLevel));
        startTime=Time.time;
    }

    IEnumerator asyncLoading(string levelName)
    {
        startTime=Time.time;
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelName);
        loadOperation.allowSceneActivation=false;

        while (!loadOperation.isDone || Time.time -startTime < 1.5f )
        {
            float progress = Mathf.Clamp01(loadOperation.progress/0.9f);
            loadSlider.value=(Time.time - startTime + progress)/2.5f;
            if(loadSlider.value>=1 && loadOperation.progress>=0.9f)
                loadOperation.allowSceneActivation=true;
            yield return null;
        }
        
    }
}
