using UnityEngine.Audio;
using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	public Sound[] sounds;

	void Awake()
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

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = s.mixerGroup;
		}
	}

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

	public void Stop(string sound) 
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.Stop();
	}

	public void setVolume(string sound, float vol)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.volume=vol;
	}

	public void FadeIn(string sound, float dur)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.pitch = s.pitch;
		StartCoroutine(transition(s,0,s.volume, dur));
	}
	public void FadeOut(string sound, float dur)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.pitch = s.pitch;
		StartCoroutine(transition(s,s.volume,0, dur));
	}

	IEnumerator transition(Sound s, float initVol, float finalVol, float dur)
	{
		s.source.volume = initVol;
		while (s.source.volume<finalVol && finalVol-initVol>0)
		{
			s.source.volume += Time.deltaTime/dur;
			yield return new WaitForSecondsRealtime(Time.deltaTime/dur);
		}
		while (s.source.volume>finalVol && finalVol-initVol<0)
		{
			s.source.volume -= Time.deltaTime;
			yield return new WaitForSecondsRealtime(Time.deltaTime/dur);
		}
		yield return null;
	}


}
