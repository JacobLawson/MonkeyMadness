using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    //Declare serialised variables
    [SerializeField]
    GameObject gameControl;
    [SerializeField]
    GameObject marker;
    [SerializeField]
    Slider platformSlider;
    [SerializeField]
    Slider bombSlider;
    [SerializeField]
    Slider timerSlider;
    [SerializeField]
    Text timeLimitText;

    bool AxisInUse;
    int menuIndex;

    //Set private variables at Start
    void Start() {
        menuIndex = 0;
        AxisInUse = false;
    }

    //Update is called once per frame
    void Update() {
        //Get key down for an axis input for switching menu options
        if (Input.GetAxisRaw("VerticalPlayer1") != 0) {
            if (AxisInUse == false) {
                if (Input.GetAxisRaw("VerticalPlayer1") < 0) {
                    menuIndex += 1;
                } else {
                    menuIndex -= 1;
                }
                AxisInUse = true;
            }
        }
        if (Input.GetAxisRaw("VerticalPlayer1") == 0) {
            AxisInUse = false;
        }
        //Check index boundries
        if (menuIndex < 0) {
            menuIndex = 2;
        }
        if (menuIndex > 2) {
            menuIndex = 0;
        }

        //switch what to do based on the current menu option chosen
        switch(menuIndex) {
            case 0:
                //Moving platform On/Off
                marker.transform.position = new Vector3 (platformSlider.transform.position.x - 96.0f, platformSlider.transform.position.y, marker.transform.position.z);
                platformSlider.value += Input.GetAxis("HorizontalPlayer1");
                if (platformSlider.value <= 0) {
                    gameControl.GetComponent<scrGameControl>().platformMoves = false;
                } else {
                    gameControl.GetComponent<scrGameControl>().platformMoves = true;
                }               
                break;
            case 1:
                //Bombs On/Off
                marker.transform.position = new Vector3(bombSlider.transform.position.x - 96.0f, bombSlider.transform.position.y, marker.transform.position.z);
                bombSlider.value += Input.GetAxis("HorizontalPlayer1");
                if (platformSlider.value <= 0) {
                    gameControl.GetComponent<scrGameControl>().bombsOn = false;
                } else {
                    gameControl.GetComponent<scrGameControl>().bombsOn = true;
                }
                break;
            case 2:
                //Set time limit for game
                marker.transform.position = new Vector3(timeLimitText.transform.position.x - 176.0f, timeLimitText.transform.position.y, marker.transform.position.z);
                timerSlider.value += Input.GetAxis("HorizontalPlayer1");
                gameControl.GetComponent<scrGameControl>().timerLength = timerSlider.value;
                break;
        }
        timeLimitText.text = "Time Limit: " + timerSlider.value.ToString() + " Seconds";

        //Return to main menu/ titlescreen
        if (Input.GetButtonDown("BackPlayer1")) {
            gameControl.GetComponent<scrGameControl>().moveToMenu = true;
        }
    }  
}
