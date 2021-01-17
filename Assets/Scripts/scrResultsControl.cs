using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrResultsControl : MonoBehaviour
{
    public bool moveToLobby;
    public bool GenerateResults;
    public int numberOfPodiums;

    private float timer;

    public Color32[] playerColours;
    Color32 defaultColour;
    public int[] initialPlayerScores;
    public int[] results;
    public int[] playerRankPointers;

    public GameObject returnIcon;
    public GameObject Podium;
    public GameObject PodiumMonkey4;
    public GameObject PodiumMonkey3;
    public GameObject Podium3;
    public GameObject firstPlaceMonkey;
    public GameObject secondPlaceMonkey;
    public GameObject thirdPlaceMonkey;
    public GameObject FourthPlaceMonkey;

    //Hats
    public GameObject[] playerHats;
    public GameObject player1Hat;
    public GameObject player2Hat;
    public GameObject player3Hat;
    public GameObject player4Hat;

    [SerializeField]
    Text firstPlaceScoreOutcome;
    [SerializeField]
    Text secondPlaceScoreOutcome;
    [SerializeField]
    Text thirdPlaceScoreOutcome;
    [SerializeField]
    Text fourthPlaceScoreOutcome;
    [SerializeField]
    Text thirdPlace;
    [SerializeField]
    Text fourthPlace;

    //UI
    [SerializeField]
    GameObject winnerAnouncement;
    //UI player outcomes
    public Sprite Player1Title;
    public Sprite Player2Title;
    public Sprite Player3Title;
    public Sprite Player4Title;

    void Start() {
        //Initalise variables
        moveToLobby = false;
        defaultColour = new Color32(43, 0, 34, 255);
       playerRankPointers = new int[] { 0, 0, 0, 0 };
    }

    void Update() {
        //At the start of the results screen sort and place each player
        if (GenerateResults == true) {
            GetResults();

            //output the results of each player in the right poddium position
            firstPlaceScoreOutcome.text = results[0].ToString();
            secondPlaceScoreOutcome.text = results[1].ToString();
            thirdPlaceScoreOutcome.text = results[2].ToString();
            fourthPlaceScoreOutcome.text = results[3].ToString();

            moveToLobby = false;
            timer = 0.0f;
            returnIcon.active = false;

            //activate the podium for the players that are playing (not a good idea to show all 4 possible players if they haven't played)
            Podium.active = true;
            if (numberOfPodiums < 4) {
                PodiumMonkey4.active = false;
                fourthPlace.text = " ";
            }
            if (numberOfPodiums < 3) {
                PodiumMonkey3.active = false;
                Podium3.active = false;
                thirdPlace.text = " ";
                thirdPlaceScoreOutcome.text = " ";
            }
            //et the winner title based on the player who won
            if (playerRankPointers[0] == 0) {
                winnerAnouncement.GetComponent<Image>().sprite = Player1Title;
            }
            else if (playerRankPointers[0] == 1) {
                winnerAnouncement.GetComponent<Image>().sprite = Player2Title;
            }
            else if (playerRankPointers[0] == 2) {
                winnerAnouncement.GetComponent<Image>().sprite = Player3Title;
            }
            else if (playerRankPointers[0] == 3) {
                winnerAnouncement.GetComponent<Image>().sprite = Player4Title;
            }
            //Give the title the colouring of the player
            winnerAnouncement.GetComponent<Image>().color = playerColours[playerRankPointers[0]];
            GenerateResults = false;
        }
        //After a period of time allow the player to return to the lobby
        if (timer > 5.0f) {
            returnIcon.active = true;
            //Deactivate Resutls screen and destroy hats
            if (Input.GetButtonDown("BackPlayer1") && returnIcon.active == true) {
                Podium.active = false;
                if (player1Hat != null) {
                    Destroy(player1Hat);
                }
                if (player2Hat != null) {
                    Destroy(player2Hat);
                }
                if (player3Hat != null) {
                    Destroy(player3Hat);
                }
                if (player4Hat != null) {
                    Destroy(player4Hat);
                }
                moveToLobby = true;
            }
        } else {
            timer += Time.deltaTime;
        }
    }

    void GetResults() {
        //Sort the player results into first to last
        for (int i = 0; i < 4 - 1; i++) {
            for (int j = 0; j < 4 - i - 1; j++) {
                if (results[j] < results[j + 1]) {
                    int temp = results[j];
                    results[j] = results[j + 1];
                    results[j + 1] = temp;
                }
            }
        }
        //Compare the position a player has ranked to the ID of the player to work out whom came in that place
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                if (results[i] == initialPlayerScores[j]) {
                    playerRankPointers[i] = j;
                    initialPlayerScores[j] = -1;
                    break;
                }
            }
        }
        //Set the monkeys on the podium to mimic the properties of their players customisation
        //Colours
        firstPlaceMonkey.GetComponent<Renderer>().material.color = playerColours[playerRankPointers[0]];
        secondPlaceMonkey.GetComponent<Renderer>().material.color = playerColours[playerRankPointers[1]];
        thirdPlaceMonkey.GetComponent<Renderer>().material.color = playerColours[playerRankPointers[2]];
        FourthPlaceMonkey.GetComponent<Renderer>().material.color = playerColours[playerRankPointers[3]];
        //Hats
        if (playerHats[0] != null) {
            player1Hat = Instantiate(playerHats[playerRankPointers[0]]);
            player1Hat.transform.position = new Vector3(firstPlaceMonkey.transform.position.x, firstPlaceMonkey.transform.position.y + 1.2f + firstPlaceMonkey.transform.localScale.y, firstPlaceMonkey.transform.position.z);
        }
        if (playerHats[1] != null) {
            player2Hat = Instantiate(playerHats[playerRankPointers[1]]);
            player2Hat.transform.position = new Vector3(secondPlaceMonkey.transform.position.x, secondPlaceMonkey.transform.position.y + 1.2f + secondPlaceMonkey.transform.localScale.y, secondPlaceMonkey.transform.position.z);
        }
        if (playerHats[2] != null) {
            player3Hat = Instantiate(playerHats[playerRankPointers[2]]);
            player3Hat.transform.position = new Vector3(thirdPlaceMonkey.transform.position.x, thirdPlaceMonkey.transform.position.y + 1.2f + thirdPlaceMonkey.transform.localScale.y, thirdPlaceMonkey.transform.position.z);
        }
        if (playerHats[3] != null) {
            player4Hat = Instantiate(playerHats[playerRankPointers[3]]);
            player4Hat.transform.position = new Vector3(FourthPlaceMonkey.transform.position.x, FourthPlaceMonkey.transform.position.y + 1.2f + FourthPlaceMonkey.transform.localScale.y, FourthPlaceMonkey.transform.position.z);
        }
    }
}