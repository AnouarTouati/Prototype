using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIGamePlay : MonoBehaviour
{

    public Text Speed;
    public Text Gear;
    public Text RPM;
    public Text CountDownText;
    public Text PositionText;
    public Text HealthText;

    public Text RaceStartsInText;
   
    public Button ReadyButton;
    private Motor Motor;
    public GameObject PreRaceGUI;

    public GameObject RaceGUI;
    public GameObject PostRaceGUI;
    public GameObject RoomManagerGUI;
    public GameObject[] RoomManagerPlayersInfoGUI;
    public GameObject PauseMenuGUI;
    [HideInInspector]
    public GamePlayScore GamePlayScore;
    public RoomManager RoomManager;
    public RaceSystem RaceSystem;
    public RectTransform CanvasRectTransform;
    [HideInInspector]
    public OnlinePlayerManager myonlinePlayerManager;
    public string PlayerName;

    float RaceStartWaitTimeModiefiableCopy;

    public GameObject localPlaceHolder;
    public AICarSpawner AICarSpawner;
    GameObject[] OtherPlayerPlaceHolder;
    public int PreviousFrameNumberOfPlayersInRoom=-1;
    public RoomController RoomController;
    public GameObject LobbyCameraVariant;

    



    void Start()
    {
       

        if (GameObject.FindGameObjectWithTag("Player"))
        {
            Motor = GameObject.FindGameObjectWithTag("Player").GetComponent<Motor>();
            GamePlayScore = GameObject.FindGameObjectWithTag("Player").GetComponent<GamePlayScore>();
            myonlinePlayerManager = GameObject.FindGameObjectWithTag("LocalPlayerPlaceHolder").GetComponent<OnlinePlayerManager>();
        }



        RoomController = GameObject.Find("RoomController").GetComponent<RoomController>();



        PreRaceGUI.SetActive(false);
        RaceGUI.SetActive(false);
        PostRaceGUI.SetActive(false);
        PauseMenuGUI.SetActive(false);
        RoomManagerGUI.SetActive(true);
    }


    void Update()
    {

        UpdateLobbyGUI();


        if (Motor == null && GameObject.FindGameObjectWithTag("Player"))
            Motor = GameObject.FindGameObjectWithTag("Player").GetComponent<Motor>();

        if (myonlinePlayerManager == null && GameObject.FindGameObjectWithTag("LocalPlayerPlaceHolder"))
            myonlinePlayerManager = GameObject.FindGameObjectWithTag("LocalPlayerPlaceHolder").GetComponent<OnlinePlayerManager>();


        if (GamePlayScore == null && GameObject.FindGameObjectWithTag("Player"))
            GamePlayScore = GameObject.FindGameObjectWithTag("Player").GetComponent<GamePlayScore>();
        if (Motor != null && RoomManager != null && RaceSystem != null && GamePlayScore != null && GamePlayScore != null)
        {
            Speed.text = "Speed :" + Motor.Speed;
            RPM.text = "RPM :" + Motor.RPM;
            Gear.text = "Gear :" + Motor.Gears;
            HealthText.text = "Health :" + Mathf.RoundToInt(GamePlayScore.PlayerHealth);
            CountDownText.text = "" + Mathf.RoundToInt(RaceSystem.CountDownTime);
            PositionText.text = "" + GamePlayScore.MyPosition;


            if (GamePlayScore.FinishedTheRace == true)
            {
                if (PostRaceGUI.activeSelf == false/* added to make the code runs once*/)
                {
                    EnablePostRaceGUI();
                }

            }
            else if (RaceSystem.RaceStarted == true)
            {
                if (RaceGUI.activeSelf == false && PauseMenuGUI.activeSelf == false)
                {
                    EnableRaceGUI();
                }

            }

            if (Input.GetKeyDown(KeyCode.Escape) && PauseMenuGUI.activeSelf == false && GamePlayScore.FinishedTheRace == false)
            {
                EnablePauseMenuGUI();
            }
            else if ((Input.GetKeyDown(KeyCode.Escape) && PauseMenuGUI.activeSelf == true) && GamePlayScore.FinishedTheRace == false)
            {
                EnableRaceGUI();
            }
        }


    }
    public void LoadMainMenu()
    {
        RoomController.ReturnToLobby();
    }
    public void EnablePreRaceGUI()
    {
        PreRaceGUI.SetActive(true);
        RaceGUI.SetActive(false);
        PostRaceGUI.SetActive(false);
        PauseMenuGUI.SetActive(false);
        RoomManagerGUI.SetActive(false);
    }
    public void EnableRaceGUI()
    {
        PreRaceGUI.SetActive(false);
        RaceGUI.SetActive(true);
        PostRaceGUI.SetActive(false);
        PauseMenuGUI.SetActive(false);
        RoomManagerGUI.SetActive(false);

    }
    public void EnablePostRaceGUI()
    {
        PreRaceGUI.SetActive(false);
        RaceGUI.SetActive(false);
        PostRaceGUI.SetActive(true);
        PauseMenuGUI.SetActive(false);
        RoomManagerGUI.SetActive(false);


    }
    public void EnablePauseMenuGUI()
    {
        PreRaceGUI.SetActive(false);
        RaceGUI.SetActive(false);
        PostRaceGUI.SetActive(false);
        PauseMenuGUI.SetActive(true);
        RoomManagerGUI.SetActive(false);

    }
    public void EnableRoomManagerGUI()
    {
        PreRaceGUI.SetActive(false);
        RaceGUI.SetActive(false);
        PostRaceGUI.SetActive(false);
        PauseMenuGUI.SetActive(false);
        RoomManagerGUI.SetActive(true);
    }
   public void SetUnSetReady()
    {
        myonlinePlayerManager.SetUnSetReady();
       
        ReadyButton.interactable = !RoomManager.LockDownActivated;
       
        
       
        
    }
    private void UpdateLobbyGUI()
    {

        if (localPlaceHolder != null)
        {
            HandleFreeCamera();
            if (RoomManager.OnlinePlayerManagers.Count != PreviousFrameNumberOfPlayersInRoom)
            {
                OtherPlayerPlaceHolder = GameObject.FindGameObjectsWithTag("OtherPlayerPlaceHolder");
            }
            
            if (!RoomManager.LockDownActivated && !RaceSystem.RaceStarted )//we only detect players Before the race starts for perfermance considerations FindGameObject is SLOW
            {

                PreviousFrameNumberOfPlayersInRoom = RoomManager.OnlinePlayerManagers.Count;

                if (myonlinePlayerManager.isReadyToStartPlaying)
                {
                    ReadyButton.GetComponentInChildren<Text>().text = "UnReady";
                }
                else
                {
                    ReadyButton.GetComponentInChildren<Text>().text = "Ready";
                }

                //i did create this list to facilitate munipilation so Just KEEP it this way
                List<GameObject> OnlinePlayerManagersListVersion = new List<GameObject>();//total players in game

                OnlinePlayerManagersListVersion.Add(localPlaceHolder);
                for (int i = 0; i < OtherPlayerPlaceHolder.Length; i++)
                {
                    OnlinePlayerManagersListVersion.Add(OtherPlayerPlaceHolder[i]);
                }
                //go through RoomManagerPlayerInfoGUI(Texts  name car and ping ect..) enable the appropriate number of them
                for (int i = 0; i < RoomManagerPlayersInfoGUI.Length; i++)
                {
                    if (i < OnlinePlayerManagersListVersion.Count && RoomManagerPlayersInfoGUI[i].activeSelf == false)
                    {
                        RoomManagerPlayersInfoGUI[i].SetActive(true);
                    }
                    else if (i >= OnlinePlayerManagersListVersion.Count && RoomManagerPlayersInfoGUI[i].activeSelf == true)
                    {
                        RoomManagerPlayersInfoGUI[i].SetActive(false);
                    }
                }
                for (int i = 0; i < OnlinePlayerManagersListVersion.Count; i++)
                {
                    ///as the time of writing this code there are 3 texts name car and ping and there is a Toggle
                    // Debug.Log(OnlinePlayerManagersListVersion.Count);
                    if (i < RoomManagerPlayersInfoGUI.Length)
                    {
                        Text[] OneSinglePlayerTexts = RoomManagerPlayersInfoGUI[i].GetComponentsInChildren<Text>();
                        if (OneSinglePlayerTexts.Length >= 1)
                            OneSinglePlayerTexts[0].text = OnlinePlayerManagersListVersion[i].GetComponent<OnlinePlayerManager>().ProfileName;
                        if (OneSinglePlayerTexts.Length >= 2)
                            OneSinglePlayerTexts[1].text = OnlinePlayerManagersListVersion[i].GetComponent<OnlinePlayerManager>().CarName;
                        //toggle text should not be changed this iss why we skipped index 2
                        if (OneSinglePlayerTexts.Length >= 3)
                            OneSinglePlayerTexts[3].text = "FixPinng";


                        RoomManagerPlayersInfoGUI[i].GetComponentInChildren<Toggle>().isOn = OnlinePlayerManagersListVersion[i].GetComponent<OnlinePlayerManager>().isReadyToStartPlaying;
                    }



                }
               }
                if (RoomManager.LockDownActivated && !RaceSystem.RaceStarted && !RaceStartsInText.enabled)
                {
                    if (!RaceStartsInText.enabled)
                    {
                        RaceStartsInText.enabled = true;
                    }

                    RaceStartWaitTimeModiefiableCopy = RoomManager.LockDownWaitTime;
                    RaceStartsInText.text = "" + Mathf.RoundToInt(RaceStartWaitTimeModiefiableCopy);

                }
                else if (RoomManager.LockDownActivated && !RaceSystem.RaceStarted)
                {
                    RaceStartWaitTimeModiefiableCopy -= Time.deltaTime;
                    RaceStartsInText.text = "" + Mathf.RoundToInt(RaceStartWaitTimeModiefiableCopy);

                }
                else if (RaceStartsInText.enabled)
                {
                    RaceStartsInText.enabled = false;
                }


            }
            else
            {
                if (GameObject.FindGameObjectWithTag("LocalPlayerPlaceHolder"))
                {
                    localPlaceHolder = GameObject.FindGameObjectWithTag("LocalPlayerPlaceHolder");
                }
            }


        }
    public void ReturnToRoom()
    {
        myonlinePlayerManager.KillMySelf();
        AICarSpawner.KillAllAICars();
        GamePlayScore.ResetAllVariables();
        RaceSystem.ResetTheScript();
        RoomManager.ResetTheScript();
      
        EnableRoomManagerGUI();
      
    }
    void HandleFreeCamera()
    {
        if (RoomManager.RaceAllowedToStart == true && LobbyCameraVariant.activeSelf)
        {
            //Debug.Log("Camera off called");
            LobbyCameraVariant.SetActive(false);
        }else if(RoomManager.RaceAllowedToStart == false && !LobbyCameraVariant.activeSelf)
        {
            LobbyCameraVariant.SetActive(true);
        }
    }
}

    
        
    

