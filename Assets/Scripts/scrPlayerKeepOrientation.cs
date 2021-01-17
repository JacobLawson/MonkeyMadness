using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class scrPlayerKeepOrientation : MonoBehaviour
{
    //Reference to other parts of the player
    public GameObject mask;
    public GameObject funkyMonkey;

    //Super monkey variables
    public bool pickup = false;
    public bool superMonkeyAbility = false;
    public bool isRunning = false;

    //Variables and refrences for super monkey holding and throwing objects
    [SerializeField]
    GameObject pickupRangeMask;
    public GameObject heldObject;
    private GameObject currentPickupMask;

    Animator animationController;

    // Start is called before the first frame update
    void Start() {
        animationController = GetComponent<Animator>();
        currentPickupMask = null;
    }

    // Update is called once per frame
    void Update() {
        //Make sure the position of the models (reg monkey and super monkey are consistant of the controlled mask
        transform.position = new Vector3(mask.transform.position.x, mask.transform.position.y + 0.5f, mask.transform.position.z);
        funkyMonkey.transform.position = new Vector3(mask.transform.position.x, mask.transform.position.y + 0.5f, mask.transform.position.z);
        if (mask.GetComponent<scrPlayer>().horizontal != 0 || mask.GetComponent<scrPlayer>().vertical != 0) {
            transform.forward = Vector3.Normalize(new Vector3(mask.GetComponent<scrPlayer>().horizontal, 0f, mask.GetComponent<scrPlayer>().vertical));
            funkyMonkey.transform.forward = Vector3.Normalize(new Vector3(mask.GetComponent<scrPlayer>().horizontal, 0f, mask.GetComponent<scrPlayer>().vertical));
        }
        //Animate the player models to move
        animationController.SetBool("isWalk", isRunning);
        funkyMonkey.GetComponent<Animator>().SetBool("isRunning", isRunning);


        //Super Monkey
        if (superMonkeyAbility == true) {
            //Find the funkey monkey that was created alongside this object
            var funkeyMonkey_ModelRenderPoint = funkyMonkey.transform.Find("Monkey");
            funkeyMonkey_ModelRenderPoint.GetComponent<Renderer>().enabled = true;       
            var thisModelrenderPoint = this.transform.Find("Monkey");
            thisModelrenderPoint.GetComponent<Renderer>().enabled = false;
            //Set colour of funkey monkey to that of this player
            funkeyMonkey_ModelRenderPoint.GetComponent<Renderer>().material.color = thisModelrenderPoint.GetComponent<Renderer>().material.color;
               
            //Pick up or throw another player
            if (pickup) {
                Pickup();
                //Stop throwing player animation
                funkyMonkey.GetComponent<Animator>().SetBool("isThrowing", false);
            }
            //Make sure that the hit box for detecting other players remains in front of player
            if (GameObject.Find("Player_ArmMask(Clone)") != null) {
                currentPickupMask.transform.position = gameObject.transform.position + (transform.forward * 2);
                currentPickupMask.transform.rotation = transform.rotation;
            }
        } else {
            //reset animation to throw player
            funkyMonkey.GetComponent<Animator>().SetBool("isThrowing", false);

            //Hide the funky monkey model again when deactivated
            var funkeyMonkey_ModelRenderPoint = funkyMonkey.transform.Find("Monkey");
            funkeyMonkey_ModelRenderPoint.GetComponent<Renderer>().enabled = false;
            var thisModelrenderPoint = this.transform.Find("Monkey");
            thisModelrenderPoint.GetComponent<Renderer>().enabled = true;
            //Remove any held object
            if (heldObject != null) {
                if (GameObject.Find("Player_ArmMask(Clone)") != null) {
                    currentPickupMask.transform.position = gameObject.transform.position + (transform.forward * 2);
                    currentPickupMask.transform.rotation = transform.rotation;
                }
                heldObject.GetComponent<scrThrowable>().state = 0;
                heldObject = null;
                Destroy(currentPickupMask);
            }
            //Safe check Destory
            if (currentPickupMask != null) {
                Destroy(currentPickupMask);
            }
        }
    }       

    void Pickup() {
        //If the player is holding another then throw it, if not pick one up
        if (heldObject != null) {
            //Throw Monkey
            funkyMonkey.GetComponent<Animator>().SetBool("isThrowing", true);
            heldObject.GetComponent<scrThrowable>().state = 0;
            heldObject = null;
            Destroy(currentPickupMask);
        }
        else if (heldObject == null) {
            //Reset pickup
            if (GameObject.Find("Player_ArmMask(Clone)") != null) {
                Destroy(currentPickupMask);
            }
            //Pickup a new player
            currentPickupMask = Instantiate(pickupRangeMask, transform.position + (transform.forward * 2), transform.rotation);
            currentPickupMask.GetComponent<scrDeleteArmMask>().playerOrigin = gameObject;
        }
        //End pickup
        pickup = false;
    }
}