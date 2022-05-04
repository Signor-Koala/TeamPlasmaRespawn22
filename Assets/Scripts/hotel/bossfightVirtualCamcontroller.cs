using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class bossfightVirtualCamcontroller : MonoBehaviour
{
    [SerializeField] GameObject player,boss;
    Vector3 playerPosInit,bossPosInit;
    bool isIntro=true;
    float separation;
    Camera cam;
    CinemachineVirtualCamera vCam;
    public Transform followPoint;
    CinemachineTargetGroup targetGroup;
    
    void Start()
    {
        player = GameObject.Find("Player");
        boss = GameObject.Find("Boss");

        vCam = this.GetComponent<CinemachineVirtualCamera>();
        targetGroup = followPoint.GetComponent<CinemachineTargetGroup>();
        playerPosInit=player.transform.position;
        bossPosInit=boss.transform.position;

        vCam.m_Lens.OrthographicSize = 1;
        vCam.Follow.position = bossPosInit;
        StartCoroutine(introSequence());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isIntro==false)
        {
            separation = (player.transform.position - boss.transform.position).magnitude;
            float ratio = 0.3151f*separation + 0.1071f;
            followPoint.position = (player.transform.position*ratio + boss.transform.position)/(1+ratio); // + new Vector3(0,0,-10);
            vCam.m_Lens.OrthographicSize = 0.757f*separation +0.215f;
        }
    }

    IEnumerator introSequence()
    {
        followPoint.transform.position=bossPosInit + new Vector3(0,0,-10);
        yield return new WaitForSeconds(2);
        isIntro=false;
    }
}
