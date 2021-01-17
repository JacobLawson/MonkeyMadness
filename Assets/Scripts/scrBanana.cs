using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrBanana : MonoBehaviour {
    //Initialise a gameobject to refer to game controller (set on spawn by spawner)
    public GameObject gameControler;

    void Update() {
        //Destroy if the gamecontroller knows the game is over
        if (gameControler != null) {
            if (gameControler.GetComponent<scrGameControl>().matchStarted == false) {
                Destroy(gameObject);
            }
        }     
    }
    void OnCollisionEnter(Collision col) {
        //Destroy if the object falls off the platform and into magma
        if (col.gameObject.tag == "Magma") {
            Destroy(gameObject);
        }      
    }
}
