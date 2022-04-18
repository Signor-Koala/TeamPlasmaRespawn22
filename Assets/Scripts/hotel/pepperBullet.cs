using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class pepperBullet : MonoBehaviour
{
    public int dps=20;
    public float lifeDur=1;
    float startTime;
    public Vector3 target;
    Rigidbody2D rb;
    Animator anim;
    bool isFalling=false, exploded=false;
	public AudioMixerGroup mixerGroup;
	public Sound[] sounds;

    private void Awake() {

        foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
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

    void Start()
    {
        startTime = Time.time;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Time.time - startTime >= lifeDur/2 && !isFalling)
        {
            gameObject.transform.position = new Vector3(target.x,transform.position.y,0);
            rb.velocity = Vector2.zero;
            rb.velocity = new Vector3(0,-2*(transform.position.y - target.y)/lifeDur,0);
            isFalling = true;
        }
        if(transform.position==target && isFalling)
        {
            rb.velocity = Vector2.zero;
            explosion();
        }
        else if(Time.time - startTime >= lifeDur)
        {
            rb.velocity = Vector2.zero;
            explosion();
        }

    }

    void explosion()
    {
        anim.SetTrigger("explosion");
        exploded=true;
    }

    public void explosionSound()
    {
        Play("pepper_release");
    }

    public void destroy()
    {
        Destroy(gameObject);
    }

    float lastDmgTime=0;
    private void OnTriggerStay2D(Collider2D other) {
        if(exploded && other.CompareTag("Player"))
        {
            if(Time.time - lastDmgTime >=1)
            {
                controller player = other.GetComponent<controller>();
                if (player != null) 
                {
                    player.TakeDamage(dps);
                    lastDmgTime = Time.time;
                }
            }
            
        }
    }

    
    
}


