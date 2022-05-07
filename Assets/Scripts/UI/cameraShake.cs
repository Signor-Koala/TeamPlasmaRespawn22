using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cameraShake : MonoBehaviour
{
    public static cameraShake instance {get; private set;}
    CinemachineVirtualCamera cinemachineVirtualCamera;
    float shakeStartTime;
    private void Awake() {
        instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void shakeCamera(float amplitude, float dur)
    {
        shakeStartTime = Time.time;
        StartCoroutine(shaking(amplitude,dur));
    }

    IEnumerator shaking(float amplitude, float dur)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if(Time.time - shakeStartTime < dur)
        {
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = amplitude;
            yield return null;
        }
        else
        {
            while (Time.time - shakeStartTime < 2*dur)
            {
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain -= amplitude*Time.unscaledDeltaTime/dur;
                yield return new WaitForSeconds(Time.fixedUnscaledTime);
            }
        }
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
    }

}
