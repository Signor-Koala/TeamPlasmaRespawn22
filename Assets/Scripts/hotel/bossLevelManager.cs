using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossLevelManager : MonoBehaviour
{
    [SerializeField] GameObject player, portal;
    portalScript portalScript;
    bool endingTriggered = false;
    void Start()
    {
        player = GameObject.Find("Player");
        portalScript = portal.GetComponent<portalScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(CEO_script.currentGameState==CEO_script.gameState.bossBattleCleared && endingTriggered==false)
        {
            endingTriggered=true;
            StartCoroutine(exitSequence());
        }
    }

    IEnumerator exitSequence()
    {
        yield return new WaitForSeconds(3);
        GameObject newPortal = Instantiate(portal);
        newPortal.transform.position = player.transform.position;
        yield return new WaitForSeconds(1);
        portalScript.activatePortal();
        Debug.Log("The End");
    }
}
