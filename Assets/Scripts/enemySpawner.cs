using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    bool isTriggered=false;
    public Transform playerPos;
    public float spawnTriggerRadius=3f;
    [SerializeField] GameObject[] enemyList;
    int[] enemyCount = {5,1};
    int[] enemyCountScatter = {2,0};
    
    private void Start() {
        playerPos = GameObject.Find("Player").GetComponent<Transform>();
    }
    
    void Update()
    {
        if(((this.transform.position - playerPos.position).magnitude < spawnTriggerRadius) && isTriggered==false)
        {
            isTriggered=true;
            initializeRoom(2,enemyList,enemyCount,enemyCountScatter,5);
        }
    }

    void initializeRoom(int enemyVariety, GameObject[] enemies, int[] enemyCount, int[] countScatter, int roomSize)
    {
        for (int i = 0; i < enemyVariety; i++)
        {
            for (int j = 0; j < enemyCount[i] + (int)(countScatter[i]*Random.Range(-1f,1f)); j++)
            {
                GameObject newEnemy = Instantiate(enemies[i]);
                Vector3 pos = new Vector3(transform.position.x,transform.position.y,0);
                pos.x += Random.Range(-roomSize/2,roomSize/2);
                pos.y += Random.Range(-roomSize/2,roomSize/2);
                newEnemy.transform.position = pos;
            }
            if(Random.Range(0f,1f)<0.75f)
                break;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawSphere(transform.position,0.1f);
    }
}
