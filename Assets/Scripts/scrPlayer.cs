using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrPlayer : MonoBehaviour
{
    //Player in control
    string playerPrefix;
    public int instanceID;
    //reference the game controller to pass scores and other game related info between objects
    public GameObject gameController; 
    public GameObject model;
    //The chosen hat to wear
    public GameObject Hat;   

    float speed = 15;
    public float horizontal;
    public float vertical;
    Rigidbody rb;

    [SerializeField]
    Slider myUISlider;

    float MonkeyCoolDownTimer = 0.0f;
    float MonkeyCoolDown = 10.0f;

    bool superMonkey = false;

    void Start() {
        rb = GetComponent<Rigidbody>();
        if (instanceID == 0) {
            //Get this players slider
            myUISlider = gameController.GetComponent<scrGameControl>().superMonkeyMeter1;
            myUISlider.value = 0;
            //Set the players hat
            Hat = gameController.GetComponent<scrGameControl>().player1_hat;
            //Set the playerPrefix
            playerPrefix = "1";
        }
        if (instanceID == 1) {
            //Get this players slider
            myUISlider = gameController.GetComponent<scrGameControl>().superMonkeyMeter2;
            myUISlider.value = 0;
            //Set the players hat
            Hat = gameController.GetComponent<scrGameControl>().player2_hat;
            //Set the playerPrefix
            playerPrefix = "2";
        }
        if (instanceID == 2) {
            //Get this players slider
            myUISlider = gameController.GetComponent<scrGameControl>().superMonkeyMeter3;
            myUISlider.value = 0;
            //Set the players hat
            Hat = gameController.GetComponent<scrGameControl>().player3_hat;
            //Set the playerPrefix
            playerPrefix = "3";
        }
        if (instanceID == 3) {
            //Get this players slider
            myUISlider = gameController.GetComponent<scrGameControl>().superMonkeyMeter4;
            myUISlider.value = 0;
            //Set the players hat
            Hat = gameController.GetComponent<scrGameControl>().player4_hat;
            //Set the playerPrefix
            playerPrefix = "4";
        }
    }

    // Update is called once per frame
    void Update() {
        //Action (pick up other players if super moneky)
        if (Input.GetButtonDown("ActionPlayer" + playerPrefix)) {
            if (superMonkey == true) {
                Debug.Log("Pickup");
                model.GetComponent<scrPlayerKeepOrientation>().pickup = true;
            }
        }
        //movement
        horizontal = Input.GetAxis("HorizontalPlayer" + playerPrefix);
        vertical = Input.GetAxis("VerticalPlayer" + playerPrefix);
        rb.AddForce(new Vector3(horizontal * speed, rb.velocity.y, vertical * speed));
        if (horizontal != 0.0f || vertical != 0.0f) {
            model.GetComponent<scrPlayerKeepOrientation>().isRunning = true;
        } else {
            model.GetComponent<scrPlayerKeepOrientation>().isRunning = false;
        }

        //Cancel Super Monkey power up after set duration
        if (superMonkey == true) {
            model.GetComponent<scrPlayerKeepOrientation>().superMonkeyAbility = true;
            //Trigger countdown
            if (MonkeyCoolDownTimer <= MonkeyCoolDown) {
                MonkeyCoolDownTimer += Time.deltaTime;
            } else {
                //reset everything for the next time the monkey is powered up
                model.GetComponent<scrPlayerKeepOrientation>().superMonkeyAbility = false;
                superMonkey = false;
                this.GetComponent<scrThrowable>().pickingMeUp = null;
                this.GetComponent<scrThrowable>().heldObject = null;               
                MonkeyCoolDownTimer = 0.0f;
                myUISlider.value = 0;
            }
        }
        //Update the position of the players worn hat
        UpdateHats();
    }

    void OnCollisionEnter(Collision col) {
        //Pick up bananas
        if (col.gameObject.tag == "Banana") {
            Destroy(col.gameObject);
            //Score points
            gameController.GetComponent<scrGameControl>().playerScores[instanceID] += 1;
            //Check and update slider to see if player should be powered up
            if (myUISlider.value < 5) {
                myUISlider.value += 1;
            } else {
                superMonkey = true;
            }
        }
        //Death by Magma
        if (col.gameObject.tag == "Magma") {
            gameController.GetComponent<scrGameControl>().playerScores[instanceID] -= 3;
            if (gameController.GetComponent<scrGameControl>().playerScores[instanceID] < 0) {
                gameController.GetComponent<scrGameControl>().playerScores[instanceID] = 0;
            }
            transform.position = new Vector3(0.0f, 40.0f, 0.0f);
        }
    }

    void UpdateHats() {
        //Move hats onto the top of the players head constantly
        if (Hat != null) {
            if (superMonkey == false)
            {
                Hat.transform.position = new Vector3(transform.position.x, transform.position.y + 1.2f + transform.localScale.y, transform.position.z);
            } else {
                Hat.transform.position = new Vector3(transform.position.x, 
                    model.GetComponent<scrPlayerKeepOrientation>().funkyMonkey.transform.position.y + model.GetComponent<scrPlayerKeepOrientation>().transform.localScale.y, 
                    transform.position.z);
            }          
            Hat.active = true;
        }
    }
}