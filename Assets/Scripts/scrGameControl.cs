using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrGameControl : MonoBehaviour
{
    //Game states
    enum GameState
    {
        TITLE,
        MENU,
        OPTIONS,
        GAMEPLAY,
        RESULTS
    }
    GameState state;

    //cameras
    [SerializeField]
    public Camera menuCamera;
    [SerializeField]
    Camera gameplayCamera;

    //string gameRule = "Timer";
    public float timerLength = 90f;
    [SerializeField]
    Text timerCurrentValue;
    float timer = 0f;
    public bool matchStarted = false;

    //Gameplay controls thar can be adjusted in options
    [SerializeField]
    GameObject playfield;
    public bool platformMoves = true;
    public bool bombsOn = true;

    //Canvases
    [SerializeField]
    GameObject menuBackground;
    [SerializeField]
    GameObject titleCanvas;
    [SerializeField]
    GameObject lobbyCanvas;
    [SerializeField]
    GameObject optionsCanvas;
    [SerializeField]
    GameObject gameplayCanvas;
    [SerializeField]
    GameObject resultsCanvas;

    //Bools to keep track of state transitions
    public bool changeToOptions;
    public bool moveToMenu;
    public bool moveToLobby;

    [SerializeField]
    GameObject monkeyLobby;

    //Reference the model and mask prefab for characters
    [SerializeField]
    GameObject MonkeyModel;
    [SerializeField]
    GameObject FunkeyModel;
    [SerializeField]
    GameObject MonkeyMask;
    //keep track of which player is which (mask and model)
    private GameObject player1_Mask;
    private GameObject player1_Model;
    public Color32 player1_Colour;
    public GameObject player1_hat;
    public Slider superMonkeyMeter1;
    public Text player1_Score;
    private GameObject player2_Mask;
    private GameObject player2_Model;
    public Color32 player2_Colour;
    public GameObject player2_hat;
    public Slider superMonkeyMeter2;
    public Text player2_Score;
    private GameObject player3_Mask;
    private GameObject player3_Model;
    public Color32 player3_Colour;
    public GameObject player3_hat;
    public Slider superMonkeyMeter3;
    public Text player3_Score;
    private GameObject player4_Mask;
    private GameObject player4_Model;
    public Color32 player4_Colour;
    public GameObject player4_hat;
    public Slider superMonkeyMeter4;
    public Text player4_Score;

    //Player
    public int numberOfPlayers = 2;
    public int[] playerScores = { 0, 0, 0, 0 };
    public int duration;

    //Gameplay UI
    public GameObject GameplayIcon1;
    public GameObject GameplayIcon2;
    public GameObject GameplayIcon3;
    public GameObject GameplayIcon4;

    // Start is called before the first frame update
    void Start() {
        //set the initals game state
        state = GameState.TITLE;

        //This script also controls gameplay canvas so make sure to hide that
        GameplayIcon1.active = false;
        GameplayIcon2.active = false;
        GameplayIcon3.active = false;
        GameplayIcon4.active = false;
        playfield.GetComponent<scrSpawn>().spawnerActive = false;

        changeToOptions = false;
        moveToMenu = false;
    }

    // Update is called once per frame
    void Update() {
        //Close game on escape
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        //Game states 
        switch (state) {
            case GameState.TITLE:
                //Initialsise title screen and fix the playfield in place
                titleCanvas.active = true;
                playfield.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
                //Control transitions between sub menus lobby and options
                if (changeToOptions == true) {
                    optionsCanvas.active = true;
                    state = GameState.OPTIONS;
                    changeToOptions = false;
                    titleCanvas.active = false;
                }
                if (moveToLobby == true) {
                    monkeyLobby.active = true;
                    lobbyCanvas.active = true;
                    state = GameState.MENU;
                    moveToLobby = false;
                    titleCanvas.active = false;
                }
                break;
            case GameState.MENU:
                //Lobby state
                //initialsie and reset confilicting variables
                resultsCanvas.active = false;
                playfield.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY;
                if (lobbyCanvas.GetComponent<scrLobbyControl>().changeToGameplay == true) {
                    //Set each players properties for gameplay
                    player1_hat = lobbyCanvas.GetComponent<scrLobbyControl>().player1Hat;
                    player2_hat = lobbyCanvas.GetComponent<scrLobbyControl>().player2Hat;
                    player3_hat = lobbyCanvas.GetComponent<scrLobbyControl>().player3Hat;
                    player4_hat = lobbyCanvas.GetComponent<scrLobbyControl>().player4Hat;

                    player1_Colour = lobbyCanvas.GetComponent<scrLobbyControl>().player1Colour;
                    player2_Colour = lobbyCanvas.GetComponent<scrLobbyControl>().player2Colour;
                    player3_Colour = lobbyCanvas.GetComponent<scrLobbyControl>().player3Colour;
                    player4_Colour = lobbyCanvas.GetComponent<scrLobbyControl>().player4Colour;

                    GameplayIcon1.GetComponent<Image>().color = player1_Colour;
                    GameplayIcon2.GetComponent<Image>().color = player2_Colour;
                    GameplayIcon3.GetComponent<Image>().color = player3_Colour;
                    GameplayIcon4.GetComponent<Image>().color = player4_Colour;
                    numberOfPlayers = lobbyCanvas.GetComponent<scrLobbyControl>().playersReady;

                    lobbyCanvas.active = false;
                    optionsCanvas.active = false;
                    matchStarted = false;
                    state = GameState.GAMEPLAY;
                } 
                else if(lobbyCanvas.GetComponent<scrLobbyControl>().changeToTitle == true) {
                    //reset for transitions
                    titleCanvas.active = true;
                    optionsCanvas.active = false;
                    state = GameState.TITLE;
                    lobbyCanvas.active = false;
                    monkeyLobby.active = false;
                    lobbyCanvas.GetComponent<scrLobbyControl>().changeToTitle = false;
                }
                break;
            case GameState.OPTIONS:
                //Options Menu
                if (moveToMenu == true) {
                    //Control and reset variables on transtion back to main menu
                    titleCanvas.active = true;
                    state = GameState.TITLE;
                    moveToMenu = false;
                    optionsCanvas.active = false;                                       
                }
                break;
            case GameState.GAMEPLAY:
                //Gameplay state
                if (matchStarted == false) {
                    //Initialise gameplay (only ran at start of gameplay)
                    timer = timerLength;
                    //Spawn player objects
                    CreatePlayers();
                    menuBackground.active = false;
                    gameplayCanvas.active = true;
                    menuCamera.gameObject.active = false;
                    gameplayCamera.gameObject.active = true;
                    //Setup the game rules based on options
                    playfield.GetComponent<scrSpawn>().spawnerActive = true;
                    playfield.GetComponent<scrSpawn>().spawnBombs = bombsOn;
                    if (platformMoves == true)
                    {
                        playfield.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    }
                    matchStarted = true;
                } 
                else if (timer > 0f) {
                    //Normal moment to moment gameplay
                    //Output score
                    player1_Score.text = playerScores[0].ToString();
                    player2_Score.text = playerScores[1].ToString();
                    player3_Score.text = playerScores[2].ToString();
                    player4_Score.text = playerScores[3].ToString();
                    //Output timer
                    timerCurrentValue.text = timer.ToString();
                    //decrement timer
                    timer -= Time.deltaTime;
                } else {       
                    //Gameplay has ended so setup results screen and reset the gameplay state
                    menuBackground.active = true;
                    menuCamera.gameObject.active = true;
                    gameplayCamera.gameObject.active = false;
                    gameplayCanvas.active = false;
                    resultsCanvas.active = true;
                    playfield.GetComponent<scrSpawn>().spawnerActive = false;
                    resultsCanvas.GetComponent<scrResultsControl>().numberOfPodiums = numberOfPlayers;
                    resultsCanvas.GetComponent<scrResultsControl>().playerColours = new Color32[] { player1_Colour, player2_Colour, player3_Colour, player4_Colour };

                    //Pass player scores into the results screen for sorting and display
                    for (int i = 0; i < 4; i++) {
                        resultsCanvas.GetComponent<scrResultsControl>().initialPlayerScores[i] = playerScores[i];
                        resultsCanvas.GetComponent<scrResultsControl>().results[i] = playerScores[i];
                    }
                    //Pass player hats into the results screen to be worn by the player objects instantiated
                    resultsCanvas.GetComponent<scrResultsControl>().playerHats[0] = player1_hat;
                    resultsCanvas.GetComponent<scrResultsControl>().playerHats[1] = player2_hat;
                    resultsCanvas.GetComponent<scrResultsControl>().playerHats[2] = player3_hat;
                    resultsCanvas.GetComponent<scrResultsControl>().playerHats[3] = player4_hat;
                    //Safety resets for results screen variables to avoid bugs
                    resultsCanvas.GetComponent<scrResultsControl>().moveToLobby = false;
                    resultsCanvas.GetComponent<scrResultsControl>().GenerateResults = true;
                   
                    //Destroy gameplay players and all objects associated
                    Destroy(player1_Mask);
                    if (player1_Model != null && player1_Model.GetComponent<scrPlayerKeepOrientation>().funkyMonkey != null) {
                        Destroy(player1_Model.GetComponent<scrPlayerKeepOrientation>().funkyMonkey);
                        Destroy(player1_Model);
                    }          
                    Destroy(player2_Mask);
                    if (player2_Model != null && player2_Model.GetComponent<scrPlayerKeepOrientation>().funkyMonkey != null) {
                        Destroy(player2_Model.GetComponent<scrPlayerKeepOrientation>().funkyMonkey);
                        Destroy(player2_Model);
                    }
                    Destroy(player3_Mask);
                    if (player3_Model != null && player3_Model.GetComponent<scrPlayerKeepOrientation>().funkyMonkey != null) {
                        Destroy(player3_Model.GetComponent<scrPlayerKeepOrientation>().funkyMonkey);
                        Destroy(player3_Model);
                    }
                    Destroy(player4_Mask);
                    if (player4_Model != null && player4_Model.GetComponent<scrPlayerKeepOrientation>().funkyMonkey != null) {
                        Destroy(player4_Model.GetComponent<scrPlayerKeepOrientation>().funkyMonkey);
                        Destroy(player4_Model);
                    }
                    //Change to results state
                    state = GameState.RESULTS;
                }
                break;
            case GameState.RESULTS:
                //Control and reset variables on the transition back to lobby
                if (resultsCanvas.GetComponent<scrResultsControl>().moveToLobby == true) {
                    resultsCanvas.active = false;
                    lobbyCanvas.active = true;
                    lobbyCanvas.GetComponent<scrLobbyControl>().gameEnded = true;
                    lobbyCanvas.GetComponent<scrLobbyControl>().changeToGameplay = false;
                    playerScores = new int[] { 0, 0, 0, 0 };
                    //Destroy player podium objects
                    if (player1_hat != null) {
                        Destroy(player1_hat);
                    }
                    if (player2_hat != null) {
                        Destroy(player2_hat);
                    }
                    if (player3_hat != null) {
                        Destroy(player3_hat);
                    }
                    if (player4_hat != null) {
                        Destroy(player4_hat);
                    }
                    state = GameState.MENU;
                }
                break;
        }
    }

    void CreatePlayers() {
        //Spawn players at the start of gameplay
        for (int i = 0; i < numberOfPlayers; i++) {
            //spawn player 1
            if (i == 0) {
                player1_Mask = Instantiate(MonkeyMask, new Vector3(7.0f, 30.0f, 0.0f), Quaternion.identity);        //initialise the mask first as the model requires it for repositioning
                player1_Model = Instantiate(MonkeyModel, new Vector3(7.0f, 30.0f, 0.0f), Quaternion.identity);      //initialise the regular monkey mdoel
                player1_Model.GetComponent<scrPlayerKeepOrientation>().funkyMonkey = Instantiate(FunkeyModel);      //initiales the funky monkey powered up model for the player
                player1_Mask.GetComponent<scrPlayer>().instanceID = i;                                              //Give the player an ID so it knows which player it is and which controller prefix to use
                player1_Mask.GetComponent<scrPlayer>().gameController = gameObject;                                 //give the player a reference to the game controller
                player1_Mask.GetComponent<scrPlayer>().model = player1_Model;                                       //give the player a reference to it's model
                player1_Model.GetComponent<scrPlayerKeepOrientation>().mask = player1_Mask;                         //tell the player's model which mask is it's
                var player1_ModelMaterialPoint = player1_Model.transform.Find("Monkey");
                player1_ModelMaterialPoint.GetComponent<Renderer>().material.color = player1_Colour;
                //Instantiate the players hat chosen during the game setup lobby
                if (player1_hat != null) {
                    player1_hat = Instantiate(player1_hat, new Vector3(7.0f, 30.0f, 0.0f), Quaternion.identity);
                }
                GameplayIcon1.active = true;
            }
            //spawn player 2
            else if (i == 1)  {
                player2_Mask = Instantiate(MonkeyMask, new Vector3(-7.0f, 30.0f, 0.0f), Quaternion.identity);
                player2_Model = Instantiate(MonkeyModel, new Vector3(-7.0f, 30.0f, 0.0f), Quaternion.identity);
                player2_Model.GetComponent<scrPlayerKeepOrientation>().funkyMonkey = Instantiate(FunkeyModel);
                player2_Mask.GetComponent<scrPlayer>().instanceID = i;
                player2_Mask.GetComponent<scrPlayer>().gameController = gameObject;
                player2_Mask.GetComponent<scrPlayer>().model = player2_Model;
                player2_Model.GetComponent<scrPlayerKeepOrientation>().mask = player2_Mask;
                var player2_ModelMaterialPoint = player2_Model.transform.Find("Monkey");
                player2_ModelMaterialPoint.GetComponent<Renderer>().material.color = player2_Colour;
                //Set Mass of the players to componsate for more players
                player1_Mask.GetComponent<Rigidbody>().mass = 3.0f;
                player2_Mask.GetComponent<Rigidbody>().mass = 3.0f;
                //Instantiate the players hat chosen during the game setup lobby
                if (player2_hat != null) {
                    player2_hat = Instantiate(player2_hat, new Vector3(7.0f, 30.0f, 0.0f), Quaternion.identity);
                }
                GameplayIcon2.active = true;
            }
            //spawn player 3
            else if (i == 2) {
                player3_Mask = Instantiate(MonkeyMask, new Vector3(0.0f, 30.0f, 7.0f), Quaternion.identity);
                player3_Model = Instantiate(MonkeyModel, new Vector3(0.0f, 30.0f, 7.0f), Quaternion.identity);
                player3_Model.GetComponent<scrPlayerKeepOrientation>().funkyMonkey = Instantiate(FunkeyModel);
                player3_Mask.GetComponent<scrPlayer>().instanceID = i;
                player3_Mask.GetComponent<scrPlayer>().gameController = gameObject;
                player3_Mask.GetComponent<scrPlayer>().model = player3_Model;
                player3_Model.GetComponent<scrPlayerKeepOrientation>().mask = player3_Mask;
                var player2_ModelMaterialPoint = player2_Model.transform.Find("Monkey");
                player2_ModelMaterialPoint.GetComponent<Renderer>().material.color = player3_Colour;

                //Set Mass of players to componsate for more players
                player1_Mask.GetComponent<Rigidbody>().mass = 2.0f;
                player2_Mask.GetComponent<Rigidbody>().mass = 2.0f;
                player3_Mask.GetComponent<Rigidbody>().mass = 2.0f;
                //Instantiate the players hat chosen during the game setup lobby
                if (player3_hat != null) {
                    player3_hat = Instantiate(player3_hat, new Vector3(7.0f, 30.0f, 0.0f), Quaternion.identity);
                }
                GameplayIcon3.active = true;
            }
            //spawn player 4
            else if (i == 3) {
                player4_Mask = Instantiate(MonkeyMask, new Vector3(0.0f, 30.0f, -7.0f), Quaternion.identity);
                player4_Model = Instantiate(MonkeyModel, new Vector3(0.0f, 30.0f, -7.0f), Quaternion.identity);
                player4_Model.GetComponent<scrPlayerKeepOrientation>().funkyMonkey = Instantiate(FunkeyModel);
                player4_Mask.GetComponent<scrPlayer>().instanceID = i;
                player4_Mask.GetComponent<scrPlayer>().gameController = gameObject;
                player4_Mask.GetComponent<scrPlayer>().model = player4_Model;
                player4_Model.GetComponent<scrPlayerKeepOrientation>().mask = player4_Mask;
                var player2_ModelMaterialPoint = player2_Model.transform.Find("Monkey");
                player2_ModelMaterialPoint.GetComponent<Renderer>().material.color = player4_Colour;
                //Set mass of the players to componsate for more players
                player1_Mask.GetComponent<Rigidbody>().mass = 1.5f;
                player2_Mask.GetComponent<Rigidbody>().mass = 1.5f;
                player3_Mask.GetComponent<Rigidbody>().mass = 1.5f;
                player4_Mask.GetComponent<Rigidbody>().mass = 1.5f;
                //Instantiate the players hat chosen during the game setup lobby
                if (player4_hat != null) {
                    player4_hat = Instantiate(player4_hat, new Vector3(7.0f, 30.0f, 0.0f), Quaternion.identity);
                }
                GameplayIcon4.active = true;
            }
        }
    }
}