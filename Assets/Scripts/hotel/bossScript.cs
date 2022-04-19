using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class bossScript : MonoBehaviour
{
    public int maxhealth = 1500;
    public int health = 1500;
    public int damage = 20;
    public float speed = 40f;
    public float attackRangeMelee = 0.5f;
    public float attackRangePepper = 3f;
    public LayerMask playerLayer;

    public GameObject enemyMinion;
    public GameObject[] enemies;
    public GameObject targetRing;
    public Transform spawnPoint;
    public Transform pepperPoint;
    public GameObject PlayerObject;
    
    public Transform plr;
    private Vector3 destination;
    private Vector3 direction;
    private float destinationLeniency = 0.15f;
    private float lastAttack = 0;
    private Quaternion rot = Quaternion.Euler(0, 0, 0);
    private Rigidbody2D rbd;
    
    private bool isAttacking = false,isAngry = false, isDashing = false, invincible=false;
    private int nextAttack = 0;
    private Animator anim;
    [SerializeField] GameObject bossBullet, popcorn, pepperBullet;
    public healthBar healthBar;
    public float[] phase2LastAttackTime = new float[6];

    // Start is called before the first frame update
    void Start()
    {
        rbd = this.GetComponent<Rigidbody2D>();
        healthBar.gameObject.SetActive(true);
        health = maxhealth;
        healthBar.initializeHealth(maxhealth);
        anim = this.GetComponent<Animator>();
        GetComponent<ParticleSystem>().Stop();
        //anim.SetBool("isAngry",true);         //for phase 2 testing
    }

    bool healthBarCorrection = false;
    void Update()
    {
        if(!healthBarCorrection)
        {
            healthBar.setHealth(health);
            healthBarCorrection = true;
        }

        if(CEO_script.currentGameState==CEO_script.gameState.gameOver)
            this.enabled=false;
    }


    public void IdleStage1()
    {
        nextAttack = Random.Range(1, 3);
        switch (nextAttack)
        {
            case 1:
                Debug.Log("Idle");
                break;
            case 2:
                Debug.Log("attackSpawn");
                anim.SetTrigger("Spawn");
                break;
        }
    }
    public void serving() //receives trigger from animator
    {
        int i = Random.Range(0,3);
        switch (i)
        {
            case 0: case 1:
                Instantiate(enemyMinion, spawnPoint.position, rot);
                break;
            case 2:
                Debug.Log("Popcorns");
                popcornMania();
                break;
        }
    }
    Vector3 popcornTarget;
    private void popcornMania()
    {
        AudioManager.instance.Play("popcorn"+Random.Range(1,9).ToString());

        for (int i = 0; i < Random.Range(5,10); i++)
        {
            popcornTarget = plr.transform.position;
            GameObject newPopcorn = Instantiate(popcorn,spawnPoint.position,rot);
            Rigidbody2D popcornRb = newPopcorn.GetComponent<Rigidbody2D>();
            popcornRb.velocity = new Vector3(Random.Range(-1.5f,1.5f),Random.Range(-1f,1f),0);
            popcornRb.angularVelocity = 360;
            newPopcorn.GetComponent<popcorn>().gravityVector = (popcornTarget - transform.position).normalized*0.5f;
        }
    }

    int stage2AttackLowerBound=1;
    int stage2AttackUpperBound=6;
    public void IdleStage2()    //decides the next action
    {
        nextAttack = Random.Range(stage2AttackLowerBound,stage2AttackUpperBound);
        switch (nextAttack)
        {
            case 1:
                Debug.Log("Idle");
                break;
            case 2:
                Debug.Log("Bullet Attack");
                anim.SetTrigger("bulletAttack");
                break;
            case 3:
                if(Time.time - phase2LastAttackTime[2]>4)
                {
                    Debug.Log("Dash-n-Smash!");
                    anim.SetTrigger("DashAttack");
                    phase2LastAttackTime[2]=Time.time;
                }
                break;
            case 4:
                if(Time.time - phase2LastAttackTime[4]>10)
                {
                    Debug.Log("Pepper Blasts");
                    anim.SetTrigger("pepperAttack");
                    anim.SetTrigger("pepperGunFire");
                    phase2LastAttackTime[4]=Time.time;
                }
                break;
            case 5:
                if(Time.time - phase2LastAttackTime[5]>20)
                {
                    Debug.Log("MeleeMania!");
                    anim.SetTrigger("spawnHorde");
                    phase2LastAttackTime[5]=Time.time;
                }
                break;
        }
    }

    bool veryAngry=false;
    public void finalPush()
    {
        //stage2AttackUpperBound=5; //Fine, I'll do it by myself...? or is it the more the merrier? :)
        stage2AttackLowerBound=2; //no time to be idle anymore :harold:
        bulletReloadProbability=0.75f;   //what's the point in saving bullets? :harold:
        pepperGunReloadProbability=0.9f;    //Rain fire protocol :harold:
        dashEncoreProbablilty=0.75f;     //I'll smack you down!
        anim.SetBool("lastDitchEffort",true);
        veryAngry = true;
    }
    
    public void bulletAttack()  //recieves trigger from animator
    {
        Play("smash_medium");

        float angleDev = Random.Range(0f,1f);
        for (int i = 0; i < 16; i++)
        {
            GameObject newBossBullet = Instantiate(bossBullet);
            newBossBullet.GetComponent<Bullet>().plr = this.transform;
            newBossBullet.transform.position = spawnPoint.position + new Vector3(0,-0.2f,0) + new Vector3(Mathf.Cos(Mathf.PI*i/8 + angleDev),Mathf.Sin(Mathf.PI*i/8 + angleDev))*0.125f;
        }
    }

    float bulletReloadProbability=0.4f;
    public void bulletReload()
    {
        if(Random.Range(0f,1f)<bulletReloadProbability)
        {
            anim.SetTrigger("bulletAttack");
        }
    }

    public void meleeMania() //receives trigger from animator
    {
        if(!veryAngry)
        {
            Play("horde_summon");
            for (int i = 0; i < 8; i++)
            {
                Instantiate(enemyMinion, spawnPoint.position + new Vector3(Mathf.Cos(Mathf.PI*2*i/8),Mathf.Sin(Mathf.PI*2*i/8))*0.5f, rot);
            }
        }
        
        
    }
    
    float dashDuration=0.1f, dashEncoreProbablilty=0.4f;
    public void dashAttackFast() //receives trigger from animator
    {
        transform.position = plr.transform.position + (transform.position - plr.position).normalized*0.2f;
        anim.SetTrigger("Smash");
    }

    //IEnumerator dashing()
    //{
        //yield return new WaitForSeconds(dashDuration);
        //anim.SetTrigger("Smash");
        //isDashing=false;
    //}
    public void dashAgain()
    {
        if(Random.Range(0f,1f) < dashEncoreProbablilty)
            anim.SetTrigger("DashAttack");
    }
    
    
    void Hit() // receives trigger from animator
    {
        Play("smash_big");

        Collider2D[] hitplayer = Physics2D.OverlapCircleAll(transform.position + new Vector3(0,-0.2f,0), attackRangeMelee, playerLayer);
        
        foreach (Collider2D player in hitplayer)
        {
            controller plyr = player.GetComponent<controller>();
            if (plyr != null)
            {
                plyr.TakeDamage(damage);
            }
        }
    }
    
    public void TakeDamage(int dam)
    {
        if(!invincible)
        {
            health -= dam;
            healthBar.setHealth(health);
            Debug.Log("Boss health : "+health);
            if(dam > 30)
                anim.SetTrigger("Damage");

            if (health <= (maxhealth / 2) && !isAngry)
            {
                anim.SetBool("isAngry",true);
                isAngry = true;
            }
            if (health <= (maxhealth / 4) && !veryAngry)
            {
                finalPush();
            }
            if (health <= 0)
            {
                rbd.velocity = new Vector2(0, 0);
                anim.SetTrigger("Die");
            }
        }
    }

    public void hasFainted()    //recieves trigger from the animator
    {
        CEO_script.currentGameState=CEO_script.gameState.bossBattleCleared;
        Play("boss_thud");
        healthBar.gameObject.SetActive(false);
        this.enabled = false;
    }

    public void invincibility()
    {
        invincible=true;
    }
    public void heal()
    {
        StartCoroutine(regeneration());
    }
    IEnumerator regeneration()
    {
        while(health<0.75*maxhealth)
        {
            health+=1;
            healthBar.setHealth(health);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        invincible=false;
        yield return null;
    }

    public void firePepperShot()
    {
        GameObject newPepperShot = Instantiate(pepperBullet, pepperPoint.position, rot);
        newPepperShot.GetComponent<Rigidbody2D>().velocity = new Vector3(0,6,0);
        newPepperShot.GetComponent<pepperBullet>().target = plr.transform.position;
        Instantiate(targetRing,plr.position,rot);
    }

    float pepperGunReloadProbability=0.66f;
    public void pepperGunReload()
    {
        if(Random.Range(0f,1f)<pepperGunReloadProbability)
        {
            anim.SetTrigger("pepperGunFire");
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) {

        //if(other.CompareTag("Player") && isDashing==true)
        //{
            //StopCoroutine(dashing());
            //isDashing=false;
            //rbd.velocity=Vector2.zero;
            //anim.SetTrigger("Smash");
        //}
    }

	public Sound[] sounds;

    private void Awake() {

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
		Sound s = System.Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

    public void caneSound()
    {
        Play("cane_whoosh");
    }
    public void thudSound()
    {
        Play("thud");
    }
    public void damageSound()
    {
        Play("bossDamage");
    }
    public void smokeScreen()
    {
        Play("smoke_screen");
        GetComponent<ParticleSystem>().Play();
        StartCoroutine(smokeScreenTimeout());
    }
    IEnumerator smokeScreenTimeout()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<ParticleSystem>().Stop();
        yield return null;
    }
    public void platterOpenSound()
    {
        Play("platterOpen"+Random.Range(1,4).ToString());
    }
    public void platterCloseSound()
    {
        Play("platterClose"+Random.Range(1,4).ToString());
    }
    
}
