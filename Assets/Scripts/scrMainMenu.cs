using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrMainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject gameControl;

    //Elements of canvas
    [SerializeField]
    GameObject titleLogo;
    [SerializeField]
    GameObject startText;
    [SerializeField]
    GameObject Marker0;
    [SerializeField]
    GameObject Marker1;
    [SerializeField]
    GameObject Marker2;
    [SerializeField]
    GameObject Icon0;
    [SerializeField]
    GameObject Icon1;
    [SerializeField]
    GameObject Icon2;

    //private variables
    bool menuFirstLoad;
    bool inMenu;
    bool transition;
    bool AxisInUse;
    int menuIndex;
    float moveSpeed;

    void Start() {
        //Show titlescreen
        menuFirstLoad = true;
    }

    // Update is called once per frame
    void Update() {
        //Check if the menu is active or the title is still up
        if (inMenu == true) {
            //Get key down for an axis input for switching menu options
            if (Input.GetAxisRaw("HorizontalPlayer1") != 0) {
                if (AxisInUse == false) {
                    if (Input.GetAxisRaw("HorizontalPlayer1") < 0) {
                        menuIndex -= 1;
                    } else {
                        menuIndex += 1;
                    }
                    AxisInUse = true;
                }
            }
            if (Input.GetAxisRaw("HorizontalPlayer1") == 0) {
                AxisInUse = false;
            }
            //Check index boundries
            if (menuIndex < 0) {
                menuIndex = 2;
            }
            if (menuIndex > 2) {
                menuIndex = 0;
            }

            //Based on menu index spin the menu round to the positions set. and give the player the correct choice to move on from menu
            moveSpeed = 1500.0f * Time.deltaTime;
            switch (menuIndex) {
                case 0:
                    //Play button in front and selectable option
                    if (Icon0.transform.position != Marker0.transform.position) {
                        //Move Icon 0
                        Icon0.transform.position = Vector3.MoveTowards(Icon0.transform.position, Marker0.transform.position, moveSpeed);
                    }
                    if (Icon1.transform.position != Marker1.transform.position) {
                        //Move Icon 1
                        Icon1.transform.position = Vector3.MoveTowards(Icon1.transform.position, Marker1.transform.position, moveSpeed);
                    }
                    if (Icon2.transform.position != Marker2.transform.position) {
                        //Move Icon 2
                        Icon2.transform.position = Vector3.MoveTowards(Icon2.transform.position, Marker2.transform.position, moveSpeed);
                    }
                    if (Input.GetButtonDown("StartPlayer1") || Input.GetButtonDown("ActionPlayer1")) {
                        //Move to lobby screen
                        gameControl.GetComponent<scrGameControl>().moveToLobby = true;
                    }
                    break;
                case 1:
                    //Options button in front and selectable option
                    if (Icon0.transform.position != Marker1.transform.position) {
                        //Move Icon 0
                        Icon0.transform.position = Vector3.MoveTowards(Icon0.transform.position, Marker1.transform.position, moveSpeed);
                    }
                    if (Icon1.transform.position != Marker2.transform.position) {
                        //Move Icon 1
                        Icon1.transform.position = Vector3.MoveTowards(Icon1.transform.position, Marker2.transform.position, moveSpeed);
                    }
                    if (Icon2.transform.position != Marker0.transform.position) {
                        //Move Icon 2
                        Icon2.transform.position = Vector3.MoveTowards(Icon2.transform.position, Marker0.transform.position, moveSpeed);
                    }
                    if (Input.GetButtonDown("StartPlayer1") || Input.GetButtonDown("ActionPlayer1")) {
                        //Move to options screen
                        gameControl.GetComponent<scrGameControl>().changeToOptions = true;
                    }
                    break;
                case 2:
                    //Exit button in front and selectable option
                    if (Icon0.transform.position != Marker2.transform.position) {
                        //Move Icon 0
                        Icon0.transform.position = Vector3.MoveTowards(Icon0.transform.position, Marker2.transform.position, moveSpeed);
                    }
                    if (Icon1.transform.position != Marker0.transform.position) {
                        //Move Icon 1
                        Icon1.transform.position = Vector3.MoveTowards(Icon1.transform.position, Marker0.transform.position, moveSpeed);
                    }
                    if (Icon2.transform.position != Marker1.transform.position) {
                        //Move Icon 2
                        Icon2.transform.position = Vector3.MoveTowards(Icon2.transform.position, Marker1.transform.position, moveSpeed);
                    }
                    if (Input.GetButtonDown("StartPlayer1") || Input.GetButtonDown("ActionPlayer1")) {
                        //Quit game
                        Application.Quit();
                    }
                    break;
            }
        } else {
            if (menuFirstLoad == true) {
                //Show the title screen but hide the menu icons
                startText.active = true;
                menuIndex = 0;
                AxisInUse = false;
                inMenu = false;
                transition = false;
                Icon0.transform.position = Marker0.transform.position;
                Icon1.transform.position = Marker1.transform.position;
                Icon2.transform.position = Marker2.transform.position;
                var initialmenuColour = Icon0.GetComponent<Image>().color;
                initialmenuColour.a = 0.0f;
                Icon0.GetComponent<Image>().color = initialmenuColour;
                Icon1.GetComponent<Image>().color = initialmenuColour;
                Icon2.GetComponent<Image>().color = initialmenuColour;
                menuFirstLoad = false;
            }
            if (Input.GetButtonDown("StartPlayer1")) {
                //Transition between title screen and title menu
                startText.active = false;
                transition = true;
            }
            if (transition == true) {
                //Fade out the alpha of title screen and fade in menu
                var titleColour = titleLogo.GetComponent<Image>().color;
                var menuColour = Icon0.GetComponent<Image>().color;
                titleColour.a -= 1.0f * Time.deltaTime;
                menuColour.a += 1.0f * Time.deltaTime;
                titleLogo.GetComponent<Image>().color = titleColour;

                Icon0.GetComponent<Image>().color = menuColour;
                Icon1.GetComponent<Image>().color = menuColour;
                Icon2.GetComponent<Image>().color = menuColour;

                if (titleColour.a < 0.0f) {
                    transition = false;
                    inMenu = true;
                }
            }
        }
    }
}