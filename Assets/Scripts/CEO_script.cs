using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEO_script : MonoBehaviour {
public static CEO_script instance;
 
  private void Awake() {
   if (instance != null) {
     Destroy(gameObject);
   }else{
     instance = this;
     DontDestroyOnLoad(gameObject);
   }
 }
}
