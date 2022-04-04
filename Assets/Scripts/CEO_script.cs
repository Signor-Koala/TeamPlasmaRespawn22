using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEO_script : MonoBehaviour {
public static CEO_script instance;
public static GameObject[] powerups;
public static int[] powerupSpawned;
 
  private void Awake() {
   if (instance != null) {
     Destroy(gameObject);
   }else{
     instance = this;
     DontDestroyOnLoad(gameObject);
   }
 }

 private void Start() {
     powerupSpawned = new int[4];
 }

}
