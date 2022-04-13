using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetRing : MonoBehaviour
{
    float startTime;
    public float lifeSpan=1;
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - startTime >= 1)
            Destroy();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
