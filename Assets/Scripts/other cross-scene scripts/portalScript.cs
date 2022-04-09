using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalScript : MonoBehaviour
{
    public bool isActivated;
    public Sprite inactiveSprite, activeSprite;

    private void Start() {
        
        deactivatePortal();
    }

    public void activatePortal()
    {
        this.GetComponent<SpriteRenderer>().sprite = activeSprite;
        isActivated=true;
    }

    public void deactivatePortal()
    {
        this.GetComponent<SpriteRenderer>().sprite = inactiveSprite;
        isActivated=false;
    }

}
