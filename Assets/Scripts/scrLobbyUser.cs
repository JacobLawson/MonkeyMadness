using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrLobbyUser : MonoBehaviour
{
    public GameObject LobbyCanvas;

    //The player controlling this lobby
    [SerializeField]
    string playerPrefix;

    //UI elements
    [SerializeField]
    GameObject marker;
    [SerializeField]
    Slider colourSliderR;
    [SerializeField]
    Slider colourSliderG;
    [SerializeField]
    Slider colourSliderB;
    [SerializeField]
    GameObject icon;
    GameObject hat;

    //privare variables
    [SerializeField]
    GameObject readyMessage;
    byte colourR;
    byte colourB;
    byte colourG;
    int selectedRGB = 0;
    int hatIndex = 0;
    bool AxisInUse;

    //public
    public bool joined;
    public bool ready;
    public Color32 playerColour;
    public GameObject Hat;
    public GameObject monkey;
    public GameObject message;

    void Start() {
        //initialise variables
        AxisInUse = false;
        playerColour = new Color32(0, 0, 0, 255);
        hatIndex = 0;
        monkey.SetActive(false);
        joined = false;
        ready = false;
    }

    void Update() {
        //Being joined means the player can activate it's UI elements and create a character
        if (joined == true) {
            //Check if player has finished customisation and ready to join the game
            if (Input.GetButtonDown("StartPlayer" + playerPrefix)) {
                if (ready == true) {
                    ready = false;
                    readyMessage.SetActive(false);
                } else {
                    ready = true;
                    readyMessage.SetActive(true);
                }
                
            }

            //When ready the player cannot customise their player
            if (ready != true) {
                //Hat swap
                if (Input.GetButtonDown("ActionPlayer" + playerPrefix)) {
                    UpdateHats();
                }
                //Drop out of game
                if (Input.GetButtonDown("BackPlayer" + playerPrefix)) {
                    monkey.SetActive(false);
                    message.SetActive(true);
                    joined = false;
                }
                //Activate the players UI elements if they are playing
                marker.SetActive(true);
                colourSliderR.gameObject.SetActive(true);
                colourSliderG.gameObject.SetActive(true);
                colourSliderB.gameObject.SetActive(true);
                icon.SetActive(true);
                if (hat != null) {
                    hat.SetActive(true);
                }
                ColourPick();
                SetColours();
            }  
        } else {
            //Deactivate the players UI elements if they are not playing
            hatIndex = 0;
            readyMessage.SetActive(false);
            marker.SetActive(false);
            colourSliderR.gameObject.SetActive(false);
            colourSliderG.gameObject.SetActive(false);
            colourSliderB.gameObject.SetActive(false);
            icon.SetActive(false);
            //Remove the hat if not playing
            if (Hat != null) {
                Destroy(Hat);
            }
            if (Input.GetButtonDown("StartPlayer" + playerPrefix)) {
                joined = true;
                monkey.SetActive(true);
                message.SetActive(false);
            }
        }
    }

    void ColourPick() {
        //Get key down for an axis input for switching menu options
        if (Input.GetAxisRaw("VerticalPlayer" + playerPrefix) != 0) {
            if (AxisInUse == false) {
                if (Input.GetAxisRaw("VerticalPlayer" + playerPrefix) < 0) {
                    //Down
                    selectedRGB += 1;
                } else {
                    //Up
                    selectedRGB -= 1;
                }
                AxisInUse = true;
            }
        }

        //Make sure that the player doens't surpass the limits of the 3 RGB sliders
        if (selectedRGB < 0) {
            selectedRGB = 2;
        }
        if (selectedRGB > 2) {
            selectedRGB = 0;
        }

        if (Input.GetAxisRaw("VerticalPlayer" + playerPrefix) == 0) {
            AxisInUse = false;
        }
        switch (selectedRGB) {
            case 0:
                //Adjst position and player colour based on Red Component of slider
               marker.transform.position = new Vector3(colourSliderR.transform.position.x - 96.0f, colourSliderR.transform.position.y, marker.transform.position.z);
               colourSliderR.value += (Input.GetAxis("HorizontalPlayer" + playerPrefix) * 50) * Time.deltaTime;
               colourR = Convert.ToByte(colourSliderR.value);
               colourSliderR.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color32(colourR, 0, 0, 255);
               break;          
            case 1:
                //Adjst position and player colour based on Green Component of slider
                marker.transform.position = new Vector3(colourSliderG.transform.position.x - 96.0f, colourSliderG.transform.position.y, marker.transform.position.z);
                colourSliderG.value += (Input.GetAxis("HorizontalPlayer" + playerPrefix) * 50) * Time.deltaTime;
                colourG = Convert.ToByte(colourSliderG.value);
                colourSliderG.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color32(0, colourG, 0, 255);
                break;
            case 2:
                //Adjst position and player colour based on Blue Component of slider
                marker.transform.position = new Vector3(colourSliderB.transform.position.x - 96.0f, colourSliderB.transform.position.y, marker.transform.position.z);
                colourSliderB.value += (Input.GetAxis("HorizontalPlayer" + playerPrefix) * 50) * Time.deltaTime;
                colourB = Convert.ToByte(colourSliderB.value);
                colourSliderB.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color32(0, 0, colourB, 255);
                break;
        }
    }

    void SetColours() {
        //Set the colours of the players based on the values of the RGB sliders
        colourR = Convert.ToByte(colourSliderR.value);
        colourG = Convert.ToByte(colourSliderG.value);
        colourB = Convert.ToByte(colourSliderB.value);
        playerColour = new Color32(colourR, colourG, colourB, 255);
        icon.GetComponent<Image>().color = playerColour;
        monkey.GetComponent<Renderer>().material.color = playerColour;
    }

    void UpdateHats() {
        //First space for hats is empty so destroy the hat
        if (Hat != null) {
            Destroy(Hat);
        }
        hatIndex += 1;
        //Make sure player is picking hats within array limits
        if (hatIndex > LobbyCanvas.GetComponent<scrLobbyControl>().hats.Length) {
            hatIndex = 0;
        }
        if (hatIndex != 0) {
            //Create hats and set their position atop the players head
            Hat = Instantiate(LobbyCanvas.GetComponent<scrLobbyControl>().hats[hatIndex]);
            Hat.transform.position = new Vector3(monkey.transform.position.x, monkey.transform.position.y + 0.2f + transform.localScale.y, monkey.transform.position.z);
        }    
    }
}