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
        transform.localScale = new Vector3(2,2,2);
        isActivated=true;
    }

    public void deactivatePortal()
    {
        this.GetComponent<SpriteRenderer>().sprite = inactiveSprite;
        isActivated=false;
    }

}
