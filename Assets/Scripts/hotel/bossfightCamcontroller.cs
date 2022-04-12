using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossfightCamcontroller : MonoBehaviour
{
    [SerializeField] GameObject player,boss;
    Vector3 playerPosInit,bossPosInit;
    bool isIntro=true;
    float separation;
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
    void FixedUpdate()
    {
        if(isIntro==false)
        {
            separation = (player.transform.position - boss.transform.position).magnitude;
            float ratio = 0.3151f*separation + 0.1071f;
            transform.position = (player.transform.position*ratio + boss.transform.position)/(1+ratio) + new Vector3(0,0,-10);
            cam.orthographicSize = 0.757f*separation +0.215f;
        }
    }

    IEnumerator introSequence()
    {
        transform.position=bossPosInit + new Vector3(0,0,-10);
        yield return new WaitForSeconds(2);
        isIntro=false;
    }
}
