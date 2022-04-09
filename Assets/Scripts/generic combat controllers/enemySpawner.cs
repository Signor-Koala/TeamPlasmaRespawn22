using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    bool isTriggered=false;
    public Transform playerPos;
    public float spawnTriggerRadius=3f;
    [SerializeField] GameObject[] enemyList;
    int[] enemyCount = {4,1,1};
    int[] enemyCountScatter = {1,1,0};
    public int enemyVariety;
    
    private void Start() {
        playerPos = GameObject.Find("Player").GetComponent<Transform>();
    }
    
    void Update()
    {
        if(((this.transform.position - playerPos.position).magnitude < spawnTriggerRadius) && isTriggered==false)
        {
            isTriggered=true;
            initializeRoom(enemyVariety,enemyList,enemyCount,enemyCountScatter,3);
        }
    }

    void initializeRoom(int enemyVariety, GameObject[] enemies, int[] enemyCount, int[] countScatter, int roomSize)
    {
        for (int i = 0; i < enemyVariety; i++)
        {
            int scatterDir=Random.Range(-1,2);
            for (int j = 0; j < enemyCount[i] + (countScatter[i]*scatterDir); j++)
            {
                GameObject newEnemy = Instantiate(enemies[i]);
                Vector3 pos = new Vector3(transform.position.x,transform.position.y,0);
                pos.x += Random.Range(-(float)roomSize/5,(float)roomSize/5);
                pos.y += Random.Range(-(float)roomSize/5,(float)roomSize/5);
                newEnemy.transform.position = pos;
            }
            if(Random.Range(0f,1f)<0.5f)
                break;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawSphere(transform.position,0.1f);
    }
}
