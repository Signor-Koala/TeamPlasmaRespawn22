using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitDoorScript : MonoBehaviour
{
    [SerializeField] Animator fadeAnim;
    GameObject blackScreen;

    private void Start() {
        fadeAnim = GameObject.Find("Black Screen").GetComponent<Animator>();
        blackScreen = GameObject.Find("Black Screen"); 
        blackScreen.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Entering Boss room...");
            CEO_script.health = other.gameObject.GetComponent<controller>().health;
            CEO_script.speed = GameObject.Find("Player").GetComponent<controller>().speed;
            StartCoroutine(bossRoomTransition());
        }
    }

    IEnumerator bossRoomTransition()
    {
        blackScreen.SetActive(true);
        fadeAnim.SetTrigger("fadeTrigger");
        yield return new WaitForSeconds(1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("boss_room");
    }
}
