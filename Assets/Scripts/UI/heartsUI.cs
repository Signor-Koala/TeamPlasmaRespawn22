using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class heartsUI : MonoBehaviour
{

    public GameObject heart;
    [SerializeField] controller playerScript;
    public List<Image> hearts;
    Stack<GameObject> heartStack;

    // Start is called before the first frame update
    void Start()
    {
        heartStack = new Stack<GameObject>();

        for(int i = 0; i < playerScript.health/20; i++)
        {
            GameObject h = Instantiate(heart, this.transform);
            heartStack.Push(h);
        }
        
    }

    private void Update() {
        if(playerScript.health>=0)
            UpdateHearts(playerScript.health);
    }
    //make this update only when the player gets damaged or ^^
    //upgrades his health by making the below public
    //and accessing it outside \/
    public void UpdateHearts(int currentHealth)
    {
        while(currentHealth/20 < heartStack.Count)
        {
            GameObject h = heartStack.Pop();
            h.SetActive(false);
        }
        while(currentHealth/20 > heartStack.Count)
        {
            foreach (GameObject heart in heartStack)
            {
                if(!heart.activeSelf)
                    Destroy(heart);
            }

            GameObject h = Instantiate(heart, this.transform);
            heartStack.Push(h);
        }

    }

}