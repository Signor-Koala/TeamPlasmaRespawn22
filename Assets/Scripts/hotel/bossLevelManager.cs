using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossLevelManager : MonoBehaviour
{
    [SerializeField] GameObject player, portal, endScreen;
    public TMPro.TextMeshProUGUI scoreText;
    Animator whiteScreenAnim;
    bool endingTriggered = false;
    void Start()
    {
        player = GameObject.Find("Player");
        whiteScreenAnim = GameObject.Find("White Screen").GetComponent<Animator>();
        endScreen.SetActive(false);
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
        newPortal.transform.localScale = new Vector3(6,6,6);
        newPortal.transform.position = player.transform.position;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        yield return new WaitForSeconds(4);
        newPortal.GetComponent<portalScript>().activatePortal();
        whiteScreenAnim.SetTrigger("portalTrigger");
        player.GetComponent<controller>().enabled = false;
        yield return new WaitForSeconds(2);
        endScreen.SetActive(true);
        scoreText.text = "Score - " + (CEO_script.totalKillScore*10 + CEO_script.money).ToString();
        yield return new WaitForSeconds(5);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main_Menu");
        Debug.Log("The End");
    }
}
