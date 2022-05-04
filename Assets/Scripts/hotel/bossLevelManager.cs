using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossLevelManager : MonoBehaviour
{
    [SerializeField] GameObject player, portal, endScreen, whiteScreen;
    public TMPro.TextMeshProUGUI scoreText;
    Animator whiteScreenAnim;
    bool endingTriggered = false;
    
    void Start()
    {
        CEO_script.currentGameState=CEO_script.gameState.bossBattle;
        player = GameObject.Find("Player");
        whiteScreen = GameObject.Find("White Screen");
        whiteScreenAnim = GameObject.Find("White Screen").GetComponent<Animator>();
        endScreen.SetActive(false);
        whiteScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(CEO_script.currentGameState==CEO_script.gameState.bossBattleCleared && endingTriggered==false && CEO_script.dangerLevel<=0)
        {
            StartCoroutine(exitSequence());
            endingTriggered=true;
        }

    }

    IEnumerator exitSequence()
    {
        yield return new WaitForSeconds(1);

        GameObject newPortal = Instantiate(portal);
        AudioManager.instance.Play("portal_crack");
        newPortal.transform.localScale = new Vector3(6,6,6);
        newPortal.transform.position = player.transform.position;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;

        yield return new WaitForSeconds(4);

        newPortal.GetComponent<portalScript>().activatePortal();
        AudioManager.instance.Play("portal_open");
        whiteScreen.SetActive(true);
        whiteScreenAnim.SetTrigger("portalTrigger");
        player.GetComponent<controller>().enabled = false;

        yield return new WaitForSeconds(2);
        endScreen.SetActive(true);

        int runScore = CEO_script.totalKillScore + CEO_script.money;
        scoreText.text = "Score: " + runScore.ToString();

        if(runScore+CEO_script.money>CEO_script.Highscore)
            CEO_script.Highscore = runScore+CEO_script.money;
            
        yield return new WaitForSeconds(5);
        UnityEngine.SceneManagement.SceneManager.LoadScene("end_credits");
        Debug.Log("The End");
    }
}
