using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrLobbyControl : MonoBehaviour
{
    //each players individual UI sections for customisation
    [SerializeField]
    GameObject playerLobby1;
    [SerializeField]
    GameObject playerLobby2;
    [SerializeField]
    GameObject playerLobby3;
    [SerializeField]
    GameObject playerLobby4;

    //Each players hat is passed in later
    public GameObject player1Hat = null;
    public GameObject player2Hat = null;
    public GameObject player3Hat = null;
    public GameObject player4Hat = null;

    //Each players colour is passed in later
    public Color32 player1Colour;
    public Color32 player2Colour;
    public Color32 player3Colour;
    public Color32 player4Colour;

    //variables for controlling when to start the gameplay state
    int playersNeeded = 0;
    public int playersReady = 0;
    public bool changeToGameplay = false;
    public bool changeToTitle = false;
    public bool gameEnded = false;
    public GameObject[] hats = new GameObject[7];

    void Update() {
        //When the game ends reset the lobby so that players can customise characters again and drop out if wished
        //important so that the results screen doesn't imediately start another game.
        if (gameEnded == true) {
            playerLobby1.GetComponent<scrLobbyUser>().joined = false;
            playerLobby1.GetComponent<scrLobbyUser>().ready = false;
            playerLobby2.GetComponent<scrLobbyUser>().joined = false;
            playerLobby2.GetComponent<scrLobbyUser>().ready = false;
            playerLobby3.GetComponent<scrLobbyUser>().joined = false;
            playerLobby3.GetComponent<scrLobbyUser>().ready = false;
            playerLobby4.GetComponent<scrLobbyUser>().joined = false;
            playerLobby4.GetComponent<scrLobbyUser>().ready = false;

            playerLobby1.GetComponent<scrLobbyUser>().monkey.SetActive(false);
            playerLobby2.GetComponent<scrLobbyUser>().monkey.SetActive(false);
            playerLobby3.GetComponent<scrLobbyUser>().monkey.SetActive(false);
            playerLobby4.GetComponent<scrLobbyUser>().monkey.SetActive(false);

            playerLobby1.GetComponent<scrLobbyUser>().message.SetActive(true);
            playerLobby2.GetComponent<scrLobbyUser>().message.SetActive(true);
            playerLobby3.GetComponent<scrLobbyUser>().message.SetActive(true);
            playerLobby4.GetComponent<scrLobbyUser>().message.SetActive(true);

            gameEnded = false;
        } else {
            //Get ready to start a new game
            if (playerLobby1.GetComponent<scrLobbyUser>().joined == false &&
            playerLobby2.GetComponent<scrLobbyUser>().joined == false &&
            playerLobby3.GetComponent<scrLobbyUser>().joined == false &&
            playerLobby4.GetComponent<scrLobbyUser>().joined == false) {
                //Return to previous screen if no players have joined
                if (Input.GetButtonDown("BackPlayer1")) {
                    playerLobby1.GetComponent<scrLobbyUser>().monkey.SetActive(false);
                    playerLobby2.GetComponent<scrLobbyUser>().monkey.SetActive(false);
                    playerLobby3.GetComponent<scrLobbyUser>().monkey.SetActive(false);
                    playerLobby4.GetComponent<scrLobbyUser>().monkey.SetActive(false);
                    changeToTitle = true;
                }
            } else {
                if (changeToGameplay == false) {
                    //Count how many players are ready and joined
                    if (playerLobby1.GetComponent<scrLobbyUser>().joined != false) {
                        playersNeeded += 1;
                        if (playerLobby1.GetComponent<scrLobbyUser>().ready != false) {
                            playersReady += 1;
                        }
                    }
                    if (playerLobby2.GetComponent<scrLobbyUser>().joined != false) {
                        playersNeeded += 1;
                        if (playerLobby2.GetComponent<scrLobbyUser>().ready != false) {
                            playersReady += 1;
                        }
                    }
                    if (playerLobby3.GetComponent<scrLobbyUser>().joined != false) {
                        playersNeeded += 1;
                        if (playerLobby3.GetComponent<scrLobbyUser>().ready != false) {
                            playersReady += 1;
                        }
                    }
                    if (playerLobby4.GetComponent<scrLobbyUser>().joined != false) {
                        playersNeeded += 1;
                        if (playerLobby4.GetComponent<scrLobbyUser>().ready != false) {
                            playersReady += 1;
                        }
                    }
                    //Start gameplay if everyone is ready
                    if (playersNeeded == playersReady && playersNeeded >= 2 && changeToGameplay == false) {
                        //Set the hats to be spawned with the players in gameplay
                        player1Hat = playerLobby1.GetComponent<scrLobbyUser>().Hat;
                        player2Hat = playerLobby2.GetComponent<scrLobbyUser>().Hat;
                        player3Hat = playerLobby3.GetComponent<scrLobbyUser>().Hat;
                        player4Hat = playerLobby4.GetComponent<scrLobbyUser>().Hat;
                        //Set the colours of the players spawnedin gameplay
                        player1Colour = playerLobby1.GetComponent<scrLobbyUser>().playerColour;
                        player2Colour = playerLobby2.GetComponent<scrLobbyUser>().playerColour;
                        player3Colour = playerLobby3.GetComponent<scrLobbyUser>().playerColour;
                        player4Colour = playerLobby4.GetComponent<scrLobbyUser>().playerColour;

                        //Deactivate and reset the lobby but do not destory to allow quick replays
                        playerLobby1.GetComponent<scrLobbyUser>().monkey.SetActive(false);
                        playerLobby2.GetComponent<scrLobbyUser>().monkey.SetActive(false);
                        playerLobby3.GetComponent<scrLobbyUser>().monkey.SetActive(false);
                        playerLobby4.GetComponent<scrLobbyUser>().monkey.SetActive(false);

                        if (playerLobby1.GetComponent<scrLobbyUser>().Hat != null) {
                            playerLobby1.GetComponent<scrLobbyUser>().Hat.active = false;
                        }
                        if (playerLobby2.GetComponent<scrLobbyUser>().Hat != null) {
                            playerLobby2.GetComponent<scrLobbyUser>().Hat.active = false;
                        }
                        if (playerLobby3.GetComponent<scrLobbyUser>().Hat != null) {
                            playerLobby3.GetComponent<scrLobbyUser>().Hat.active = false;
                        }
                        if (playerLobby4.GetComponent<scrLobbyUser>().Hat != null) {
                            playerLobby4.GetComponent<scrLobbyUser>().Hat.active = false;
                        }
                        // move to gameplay
                        changeToGameplay = true;
                    } else {
                        playersNeeded = 0;
                        playersReady = 0;
                        changeToGameplay = false;
                    }
                }
            }
        }      
    }
}
