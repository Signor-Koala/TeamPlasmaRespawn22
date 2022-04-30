
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class controller : MonoBehaviour
{
    public float speed = 1.0f;
    public int health = 100;
    public bool died = false;
    public Vector2 looking;
    bool firing=false;
    public float firepoint = 1f;
    public float reloadTime = 0.5f;
    public float rollDuration = 0.2f;
    bool pointerBusy=false;
    public float rollReload = 1f;

    public GameObject currenProj;
    public Camera maincam;
    
    private Vector2 direction = new Vector2(0, -1);
    private Vector2 dodgeDir = new Vector2(0, 0);
    private Vector2 deviation;
    private Rigidbody2D rbd;
    private float lastFireTime = 0f;
    private float lastRollTime = 0f;
    private Quaternion rot = Quaternion.Euler(0, 0, 0);
    private bool invincible = false;
    int dashDamage=30;
    Collider2D dashCollider;
    Animator anim, cameraAnim;
    public GameObject[] projList;
    ParticleSystem trail;
    TrailRenderer trailRender;


    void Start()
    {
        rbd = GetComponent<Rigidbody2D>();
        anim = this.gameObject.GetComponent<Animator>();
        cameraAnim = FindObjectOfType<Camera>().GetComponent<Animator>();
        dashCollider = GameObject.Find("dashDamager").GetComponent<CircleCollider2D>();
        dashCollider.enabled = false;
        gameObject.GetComponentInChildren<Light2D>().intensity=0.5f;

        trailRender = this.gameObject.GetComponent<TrailRenderer>();
        trail = GetComponent<ParticleSystem>();
        trailRender.enabled = false;
        trail.Stop();

        health = CEO_script.health;
        speed = CEO_script.speed;
        currenProj = CEO_script.activePowerUp;    //for level testing
        PlayerPrefs.SetInt("firstload",1);
    }

    
    void FixedUpdate()
    {
        //Input
        float xaxis = Input.GetAxisRaw("Horizontal");
        float yaxis = Input.GetAxisRaw("Vertical");

        if(xaxis !=0 || yaxis !=0)
            anim.SetFloat("animationSpeed",1);
        else
            anim.SetFloat("animationSpeed",0.5f);
        
        //if(xaxis<0)
            //this.gameObject.transform.localScale = new Vector3(-0.5f,0.5f,1);
        
        
        //Bullet positioning
        looking = rbd.position - (Vector2)maincam.ScreenToWorldPoint(Input.mousePosition);
        looking /= (looking.magnitude/firepoint);
        
        deviation = Vector2.Perpendicular(looking);
        deviation /= 10;

        //sending input to the sprite animator
        float facingX,facingY;
        if(looking.normalized.x>1/Mathf.Sqrt(2))
            facingX=-1;
        else if(looking.normalized.x<-1/Mathf.Sqrt(2))
            facingX=1;
        else
            facingX=0;

        if(looking.normalized.y>1/Mathf.Sqrt(2))
            facingY=-1;
        else if(looking.normalized.y<-1/Mathf.Sqrt(2))
            facingY=1;
        else
            facingY=0;

        if(CEO_script.activePowerUp==null && CEO_script.currentGameState==CEO_script.gameState.preForestLevel)
        {
            anim.SetFloat("xInput",xaxis);
            anim.SetFloat("yInput",yaxis);
        }
        else
        {
            anim.SetFloat("xInput",facingX);
            anim.SetFloat("yInput",facingY);
        }

        //firing weapon
        if (Input.GetButton("Fire1") && (Time.time > lastFireTime + reloadTime) && !(invincible) && currenProj !=null && !pointerBusy)
        {    
            FireWeapon((rbd.position-looking), rot);
        }

        direction = new Vector2(xaxis, yaxis);

        if (direction.x != 0 || direction.y != 0)
        {
            //Dash
            if (Input.GetButton("Fire2") && (Time.time > lastRollTime + rollReload))
            {
                invincible = true;
                lastRollTime = Time.time;
                dodgeDir = direction;
                cameraAnim.SetTrigger("shake");
                dashCollider.enabled=true;
            }

            if (invincible)
            {
                trail.Play();
                rbd.velocity = (Vector3)dodgeDir * (5 * speed);
                if(gameObject.GetComponentInChildren<Light2D>().intensity<1)
                    gameObject.GetComponentInChildren<Light2D>().intensity+=0.05f;
                //trailRender.enabled = true;   //trailRender, yes or no? hmmm...

                AudioManager.instance.Play("dashEffect");   //play dash sound

                StartCoroutine(trailfadeDelay());

                if (Time.time > lastRollTime + rollDuration)
                {
                    rbd.velocity = Vector2.zero;
                    trail.Stop();
                    invincible = false;
                    dashCollider.enabled = false;
                }
            }
            else if(!(CEO_script.currentGameState==CEO_script.gameState.bossBattleCleared && CEO_script.dangerLevel<=0)) // Normal movement
            {
                //Motion
                rbd.transform.position += (Vector3) direction * (speed * Time.deltaTime);
            }

            if(gameObject.GetComponentInChildren<Light2D>().intensity>0.5f)   //dash glow fade off
                    gameObject.GetComponentInChildren<Light2D>().intensity-=0.05f;
        }

        for (int i = 0; i < 5; i++)
        {
            if(currenProj !=null && currenProj==projList[i])
            {
                anim.SetInteger("attackMode",i+1);
            }
        }

    }

    IEnumerator trailfadeDelay()
    {
        yield return new WaitForSeconds(0.3f);
        trailRender.enabled = false;
        
    }

    void FireWeapon(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(currenProj, position, rotation);
        bullet.GetComponent<Bullet>().plr = transform;
        playShotSound();
        
        if (currenProj.name == "shotgun")
        {
            GameObject bullet1 = Instantiate(currenProj, position + (Vector3)deviation, rotation);
            bullet1.GetComponent<Bullet>().plr = transform;
            GameObject bullet2 = Instantiate(currenProj, position - (Vector3)deviation, rotation);
            bullet2.GetComponent<Bullet>().plr = transform;
        }
        
        reloadTime = bullet.GetComponent<Bullet>().reload;
        lastFireTime = Time.time;
    }

    public void TakeDamage(int dam)
    {
        if (!invincible)
        {
            health -= dam;
            CEO_script.health -= dam;
            Debug.Log("health:" + health);

            AudioManager.instance.Play("playerDamage");
            cameraAnim.SetTrigger("shake");

            if (health <= 0)
            {
                CEO_script.currentGameState=CEO_script.gameState.gameOver;
                //dying animation
                anim.SetBool("isDead",true);
                
                //trigger game over
                StartCoroutine(gameOverSequence());

                this.enabled = false;
            }
        }
    }

    public void deathSound()
    {
        AudioManager.instance.Play("playerDeath");
    }

    public void playShotSound()
    {
        if(currenProj==projList[0])
            AudioManager.instance.Play("bullet");
        else if(currenProj==projList[1] && !firing)
        {
            StartCoroutine(machineGunLoop());
        }
        else if(currenProj==projList[2])
            AudioManager.instance.Play("Sniper");
        else if(currenProj==projList[3])
            AudioManager.instance.Play("shotGun");
        else if(currenProj==projList[4])
        {
            AudioManager.instance.Play("rpg_fire");
            StartCoroutine(playSound("rpg_load",1.5f));
        }
    }   
    IEnumerator playSound(string name, float delay)    //play sound with delay
    {
        yield return new WaitForSeconds(delay);
        AudioManager.instance.Play(name);
    }
    IEnumerator machineGunLoop()    //play sound with delay
    {
        firing=true;
        while (Input.GetButton("Fire1"))
        {
            AudioManager.instance.Play("machine_gun_shot");
            yield return new WaitForSeconds(0.1f);
        }
        firing=false;
        yield return null;
        
    }

    IEnumerator gameOverSequence()
    {
        yield return new WaitForSeconds(2);
        CEO_script.gameOver();
    }

    public void cursorOnButton()
    {
        pointerBusy=true;
    }
    public void cursorOffButton()
    {
        pointerBusy=false;
    }
     
}