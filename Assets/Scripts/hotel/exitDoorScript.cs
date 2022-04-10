using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitDoorScript : MonoBehaviour
{
    [SerializeField] Animator fadeAnim;

    private void Start() {
        fadeAnim = GameObject.Find("Black Screen").GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Entering Boss room...");
            StartCoroutine(bossRoomTransition());
        }
    }

    IEnumerator bossRoomTransition()
    {
        fadeAnim.SetTrigger("fadeTrigger");
        yield return new WaitForSeconds(1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("boss_room");
    }
}
