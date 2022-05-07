using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class controller : MonoBehaviour
{
    float xaxis,yaxis;
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
    public float inaccuracyFloat = 1f;
    public float recoilplr = 1f;
    public int ammocapacity = 300, ammo=300,ammoRate=3;              //ammo
    
    public int staminacap = 600;                //stamina
    public int stamina = 600;
    public int dashUsage = 60;

    public GameObject currenProj;
    public Camera maincam;
    
    private Vector2 direction = new Vector2(0, -1);
    private Vector2 dodgeDir = new Vector2(0, 0);
    private Vector2 deviation;
    private Vector2 inaccuracy;
    private Rigidbody2D rbd;
    private float lastFireTime = 0f;
    private float lastRollTime = 0f;
    private Quaternion rot = Quaternion.Euler(0, 0, 0);
    private bool invincible = false;
    int dashDamage=30;
    Collider2D dashCollider;
    Animator anim;
    public GameObject[] projList;
    ParticleSystem trail;
    TrailRenderer trailRender;
    healthBar ammoBar,staminaBar;
    Light2D dashLight;


    void Start()
    {
        gameObject.GetComponent<Collider2D>().enabled=true;
        rbd = GetComponent<Rigidbody2D>();
        anim = this.gameObject.GetComponent<Animator>();
        dashCollider = GameObject.Find("dashDamager").GetComponent<CircleCollider2D>();
        //The above can just be statically assigned to the prefab since there is only 1
        //using Find() is expensive
        dashCollider.enabled = false;
        //disable this by default ^
        gameObject.GetComponentInChildren<Light2D>().intensity=0.5f;
        //just make the default value 0.5 instead of assigning it in script ^

        trailRender = this.gameObject.GetComponent<TrailRenderer>();
        dashLight = gameObject.GetComponentInChildren<Light2D>();
        trail = GetComponent<ParticleSystem>();
        trailRender.enabled = false;
        trail.Stop();
        //same thing here, just disable them by default

        health = CEO_script.health;
        speed = CEO_script.speed;
        //invincible=false;
        
        
        ammoBar=GameObject.Find("AmmoBar").GetComponent<healthBar>();               //initializing stamina and ammo bars.
        staminaBar=GameObject.Find("StaminaBar").GetComponent<healthBar>();
        ammoBar.initializeHealth(ammocapacity);
        staminaBar.initializeHealth(staminacap);
        ammoBar.setHealth(ammocapacity);
        staminaBar.setHealth(staminacap);

        currenProj = CEO_script.activePowerUp;    //for level testing//
        switchweapons();
        PlayerPrefs.SetInt("firstload",1);
    }

    
    void FixedUpdate()
    {
        //Input
        xaxis = Input.GetAxisRaw("Horizontal");
        yaxis = Input.GetAxisRaw("Vertical");

        //to change animation from idle to walking, without indulging in the complexities of 16 animations inside a blend tree
        if(xaxis !=0 || yaxis !=0)
            anim.SetFloat("animationSpeed",1);
        else
            anim.SetFloat("animationSpeed",0.5f);
        
        //Bullet positioning
        looking = (rbd.position - (Vector2)maincam.ScreenToWorldPoint(Input.mousePosition)).normalized;
        looking *= firepoint;
        
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
        if (Input.GetButton("Fire1") && (Time.time > lastFireTime + reloadTime) && !(invincible) && currenProj !=null && !pointerBusy && ammo > 0)
        {    
            ammo-=ammoRate;
            FireWeapon((rbd.position-looking)+inaccuracy, rot);
            recoilplr=currenProj.GetComponent<Bullet>().recoilVal;
	        rbd.velocity = recoilplr*looking; //recoil for player
            ammoBar.setHealth(ammo);        //ammo, stamina bar set
        } else if (!Input.GetButton("Fire1") && ammo < ammocapacity){   //ammo recharge
	        ammo++;
            ammoBar.setHealth(ammo);        //ammo, stamina bar set
        }
	
        direction = new Vector2(xaxis, yaxis);

        if (direction.x != 0 || direction.y != 0)
        {
            //Dash trigger
            if (Input.GetButton("Fire2") && (Time.time > lastRollTime + rollReload))
            {
                invincible = true;
                lastRollTime = Time.time;
                dodgeDir = direction;
                dashCollider.enabled=true;
                if(stamina>=dashUsage)
		            stamina -= dashUsage;
                else if(stamina<dashUsage)
                    stamina=0;
                staminaBar.setHealth(stamina);
            }
        }

        //dash process
        if (invincible)
        {
            rbd.velocity = (Vector3)dodgeDir * (5 * speed);
                
            //trailRender.enabled = true;   //trailRender, yes or no? hmmm...

            var newEmission = trail.emission;
                newEmission.rateOverDistance = 100 * stamina/staminacap;
                trail.Play();

            if (Time.time > lastRollTime + rollDuration)
            {
                rbd.velocity = Vector2.zero;
                trail.Stop();
                invincible = false;
                dashCollider.enabled = false;
            }
            

            if(dashLight.intensity<1)
                    dashLight.intensity+=0.05f;
            StartCoroutine(trailfadeDelay());
            AudioManager.instance.Play("dashEffect");   //play dash sound
            cameraShake.instance.shakeCamera(1f*stamina/staminacap,rollDuration);

        } //moved as an else since its more efficient than not
        else  // Normal movement
        {
            //modified the statements as otherwise it would stop player motion if stamina is regenerating.
            if (!invincible && stamina < staminacap)    //stamina recharge
            {
                if(stamina<=0)
                    stamina=0;
                if(Time.time > lastRollTime + rollReload)
                    stamina+=2;
                staminaBar.setHealth(stamina);
            } 
            //Motion
                
        
        if(CEO_script.currentGameState!=CEO_script.gameState.bossBattleCleared || CEO_script.dangerLevel>0)     
            rbd.transform.position += (Vector3) direction * (speed * Time.fixedDeltaTime);

        }

        // I have moved the stuff in the second update loop into here
        // please tell me you had good reason to have 2 update loops :harold:       //yep, reason was good, but there was a misunderstanding

        //this doesnt need to be set unless it is updated
        //so putting these there \/
        
        /*
        ammoBar.setHealth(ammo);        //ammo, stamina bar set
        staminaBar.setHealth(stamina);
        */

        //bruh
        //why do you have a loop that constantly checks for the sprites?        //yep, I realized that today :harold:
        //put these things in a function that is only triggered when
        //the player goes over a powerup
        /*
        
        */



        //why is this seperate from the firing :confused:               //was with the firing actually, but then thought not to update it in fixedupdate frames
        /*
        if (Input.GetButton("Fire1") && (Time.time > lastFireTime + reloadTime) && !(invincible) && currenProj !=null && !pointerBusy && ammo > 0)  //ammo update
            ammo-=ammoRate;
        */

        /*
        if(invincible)            //dash effects
        {
            var newEmission = trail.emission;
                newEmission.rateOverDistance = 100 * stamina/staminacap;
                trail.Play();

            if(dashLight.intensity<1)
                    dashLight.intensity+=0.05f;
            StartCoroutine(trailfadeDelay());
            AudioManager.instance.Play("dashEffect");   //play dash sound
            cameraShake.instance.shakeCamera(1f*stamina/staminacap,rollDuration);
        } //moved as an else since its more efficient than not
        else if (!invincible && stamina < staminacap)    //stamina recharge
        {
            if(stamina<=0)
                stamina=0;
            if(Time.time > lastRollTime + rollReload)
	            stamina+=2;
            staminaBar.setHealth(stamina);
        }
        */
        //i moved this into an already existing if statement
        
        if(dashLight.intensity>0.5f)   //dash glow fade off
                dashLight.intensity-=0.05f;
    }
    
    IEnumerator trailfadeDelay()
    {
        yield return new WaitForSeconds(0.3f);
        trailRender.enabled = false; 
    }

    void FireWeapon(Vector3 position, Quaternion rotation)
    {
	    //inaccuracy for player gun, idk how to exactly implenet it for the enmy bullets, since
	    //the logic for it has changed
	    inaccuracy = deviation;
	    inaccuracy = UnityEngine.Random.Range(-1.0f,1.0f)*inaccuracyFloat*inaccuracy;
        //moved this from the update loop

        GameObject bullet = Instantiate(currenProj, position, rotation);
        bullet.GetComponent<Bullet>().plr = transform;
        playShotSound();
        
        if (currenProj.name == "shotgun")
        {
            deviation = Vector2.Perpendicular(looking);
            deviation /= 10;
            //moved this from the update loop

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
            cameraShake.instance.shakeCamera(0.5f,0.2f);

            if (health <= 0)
            {
                CEO_script.currentGameState=CEO_script.gameState.gameOver;
                //dying animation
                anim.SetBool("isDead",true);
                gameObject.GetComponent<Collider2D>().enabled = false;
                
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
        cameraShake.instance.shakeCamera(0.5f,0.05f);
        if(currenProj==projList[0])
            AudioManager.instance.Play("bullet");
        else if(currenProj==projList[1] && !firing)
        {
            //StartCoroutine(machineGunLoop());
            AudioManager.instance.Play("machine_gun_shot");
        }
        else if(currenProj==projList[2])
            AudioManager.instance.Play("Sniper");
        else if(currenProj==projList[3])
            AudioManager.instance.Play("shotGun");
        else if(currenProj==projList[4])
        {
            AudioManager.instance.Play("rpg_fire");
            if(ammo<=0)
                StartCoroutine(playSound("rpg_load",1.5f));
        }
    }   
    IEnumerator playSound(string name, float delay)    //play sound with delay/
    {
        yield return new WaitForSeconds(delay);
        AudioManager.instance.Play(name);
    }
    
    IEnumerator gameOverSequence()
    {
        
        AudioManager.instance.Stop("boss_phase_1");
        AudioManager.instance.Stop("boss_phase_2");
        AudioManager.instance.Stop("forest_normal_theme");
        AudioManager.instance.Stop("forest_danger_theme");
        AudioManager.instance.Stop("hotel_normal_theme");
        AudioManager.instance.Stop("hotel_danger_theme");
        
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

    public void switchweapons(){
        
        CEO_script.activePowerUp = currenProj;
        for (int i = 0; i < 5; i++)             //sprite switching
        {
            if(currenProj !=null && currenProj.name==projList[i].name)
            {
                anim.SetInteger("attackMode",i+1);
            }
        }
    }
     
}
