using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossfightCamcontroller : MonoBehaviour
{
    [SerializeField] GameObject player,boss;
    Vector3 playerPosInit,bossPosInit;
    bool isIntro=true;
    Camera cam;
    
    void Start()
    {
        player = GameObject.Find("Player");
        boss = GameObject.Find("Boss");
        cam = this.GetComponent<Camera>();
        cam.orthographicSize = 1;
        playerPosInit=player.transform.position;
        bossPosInit=boss.transform.position;
        StartCoroutine(introSequence());
    }

    // Update is called once per frame
    void Update()
    {
        if(isIntro==false)
        {
            transform.position = (player.transform.position + boss.transform.position)/2 + new Vector3(0,0,-10);
            cam.orthographicSize = (player.transform.position - boss.transform.position).magnitude/(playerPosInit-bossPosInit).magnitude +0.16f;
        }
    }

    IEnumerator introSequence()
    {
        transform.position=bossPosInit + new Vector3(0,0,-10);
        yield return new WaitForSeconds(2);
        isIntro=false;
    }
}
