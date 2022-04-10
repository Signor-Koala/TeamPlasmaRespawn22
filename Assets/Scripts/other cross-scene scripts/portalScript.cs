using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalScript : MonoBehaviour
{
    public bool isActivated=false;
    public Sprite inactiveSprite, activeSprite;

    private void Start() {
        this.GetComponent<SpriteRenderer>().sprite = inactiveSprite;
        transform.localScale = new Vector3(1,1,1);
    }
    public void activatePortal()
    {
        if(isActivated==false)
        {
            this.GetComponent<SpriteRenderer>().sprite = activeSprite;
            transform.localScale = new Vector3(2,2,2);
            isActivated=true;
        }
        
    }

    public void deactivatePortal()
    {
        if(isActivated==true)
        {
            this.GetComponent<SpriteRenderer>().sprite = inactiveSprite;
            isActivated=false;
        }
    }

}
