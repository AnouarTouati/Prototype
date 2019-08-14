using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class GamePlayScore : MonoBehaviourPunCallbacks,IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            
                stream.SendNext(PlayerName);
                stream.SendNext(PlayerHealth);
                stream.SendNext(NumberOfCheckPointsICrossed);
                stream.SendNext(FinishedTheRace);
                stream.SendNext(myLatestPriority);
                
            
        }
        else
        {


            PlayerName = (string)stream.ReceiveNext();
            PlayerHealth = (float)stream.ReceiveNext();
            NumberOfCheckPointsICrossed = (int)stream.ReceiveNext(); ;
            FinishedTheRace = (bool)stream.ReceiveNext();
            myLatestPriority = (int)stream.ReceiveNext();
        }
    }
   
    public string PlayerName;
    
    public float PlayerHealth = 100;
    
    public int NumberOfCheckPointsICrossed;
   
    public bool FinishedTheRace = false;
    
    public float FinishingTime;
    public int myLatestPriority;


    public int FinshingPosition;
    
    public int TotalNumberOfFrames=0;//used to calculate the average speed all frames since race start not game start
    
    public float TopSpeed=0;
    
    public float AverageSpeed;

    public static string[,] WaitingListOfTexts = new string[4,5];

    
    private Motor Motor;
    private RaceSystem RaceSystem;
    private GUIGamePlay GUIGamePlay;
    private GameObject[] Standings=new GameObject[4];
   
    public int MakeSure=0;
    public string ThisSceneName;
    public static int LastFilledStandings=0;

   
   
    public int MyPosition=0;
    public GameObject[] ArrayOfOtherPlayers;
    public PhotonView PV;
    void Start () {
        
        Standings[0] = GameObject.Find("Row1");
        Standings[1] = GameObject.Find("Row2");
        Standings[2] = GameObject.Find("Row3");
        Standings[3] = GameObject.Find("Row4");


       
       
    }
	
	
	void Update () {

        if (transform.name != PlayerName)
        {
            transform.name = PlayerName;// we can make this better  by moving it out of the Update method
        }
        if (GUIGamePlay == null)
        {
            GUIGamePlay = GameObject.Find("GUI").GetComponent<GUIGamePlay>();
        }
        if (PV.IsMine)

        {
          
            if (Motor == null&& GameObject.FindGameObjectWithTag("Player"))
                Motor = GameObject.FindGameObjectWithTag("Player").GetComponent<Motor>();
            if (RaceSystem == null)
                RaceSystem = GameObject.Find("RaceSystem").GetComponent<RaceSystem>();
          
            if ( Motor != null && RaceSystem != null && GUIGamePlay != null)
            {





                RacePosition();// we should choose another function for GameMode that is not Racing
               

                if (RaceSystem.RaceStarted == true && FinishedTheRace == false)
                {
                    TotalNumberOfFrames += 1;
                    FinishingTime += Time.deltaTime;// do this temporarly
                    AverageSpeed += Motor.Speed;
                    if (Motor.Speed > TopSpeed)
                    {
                        TopSpeed = Motor.Speed;
                    }

                }
                else if (FinishedTheRace == true && MakeSure == 0)
                 {
                    AverageSpeed = AverageSpeed / TotalNumberOfFrames;
                    PV.RPC ("RPC_AddHimToStandings",RpcTarget.All, PlayerName, FinishingTime, TopSpeed, AverageSpeed);
                    MakeSure++;

                 }
            }
        }
            
        
	}
  [PunRPC]
    void  RPC_AddHimToStandings(string PlayerName, float FinishingTime, float TopSpeed, float AverageSpeed)
    {
        if (GUIGamePlay.PostRaceGUI.activeSelf == false)
        {
            WaitingListOfTexts[LastFilledStandings, 0] = PlayerName;
            WaitingListOfTexts[LastFilledStandings, 1] = Mathf.Round(FinishingTime).ToString();
            WaitingListOfTexts[LastFilledStandings, 2] = Mathf.Round(TopSpeed).ToString();
            WaitingListOfTexts[LastFilledStandings, 3] = Mathf.Round(AverageSpeed).ToString();
            LastFilledStandings++;
        }
        else
        {


            WaitingListOfTexts[LastFilledStandings, 0]= PlayerName;
            WaitingListOfTexts[LastFilledStandings, 1]=   Mathf.Round(FinishingTime).ToString();
            WaitingListOfTexts[LastFilledStandings, 2]=  Mathf.Round(TopSpeed).ToString();
            WaitingListOfTexts[LastFilledStandings, 3]=  Mathf.Round(AverageSpeed).ToString();
            LastFilledStandings++;

            Standings[0] = GameObject.Find("Row1");
            Standings[1] = GameObject.Find("Row2");
            Standings[2] = GameObject.Find("Row3");
            Standings[3] = GameObject.Find("Row4");

            
            for (int j = 0; j < 4; j++)
            {
                Text[] temp = Standings[j].GetComponentsInChildren<Text>();

                for (int i = 0; i < 5; i++)
                {

                    temp[i].text = WaitingListOfTexts[j, i];
                }
            }
        }

       
    }
   
   
    
    void RacePosition()
    {
        for (int i = 0; i < RaceSystem.AllVertices.Length; i++)
        {
            if (RaceSystem.AllVertices[i].x > transform.position.x && RaceSystem.AllVertices[i].z > transform.position.z)
            {
                myLatestPriority = i - 1;

                break;
            }

        }
        ArrayOfOtherPlayers = GameObject.FindGameObjectsWithTag("OtherPlayer");//we do this in update just because we currently accept late join

        int[] ArrayOfAllPlayersPriorities = new int[ArrayOfOtherPlayers.Length + 1];//+1 add space for my priority
        ArrayOfAllPlayersPriorities[0] = myLatestPriority;

        for (int i = 1; i < ArrayOfOtherPlayers.Length + 1; i++)
        {
            ArrayOfAllPlayersPriorities[i] = ArrayOfOtherPlayers[i - 1].GetComponent<GamePlayScore>().myLatestPriority;
        }

        for (int i = 0; i < ArrayOfAllPlayersPriorities.Length; i++)//we are sorting the priorites from HIGHEST to LOWEST
        {
            for (int j = i; j < ArrayOfAllPlayersPriorities.Length; j++)
            {
                if (ArrayOfAllPlayersPriorities[i] < ArrayOfAllPlayersPriorities[j])
                {
                    int swaptemporay = ArrayOfAllPlayersPriorities[i];
                    ArrayOfAllPlayersPriorities[i] = ArrayOfAllPlayersPriorities[j];
                    ArrayOfAllPlayersPriorities[j] = swaptemporay;
                }
            }

        }

        for (int i = 0; i < ArrayOfOtherPlayers.Length + 1; i++)
        {
            if (myLatestPriority == ArrayOfAllPlayersPriorities[i])//here we determine our position by the position of our priority in the list
            {
                MyPosition = i + 1;
            }
        }

    }
    public void ResetAllVariables()
    {
        MyPosition = 0;
        PlayerHealth = 100;
        NumberOfCheckPointsICrossed=0;
        FinishedTheRace = false;
        FinishingTime=0;
        myLatestPriority=0;
        FinshingPosition = 0;
        TotalNumberOfFrames = 0;//used to calculate the average speed all frames since race start not game start
        TopSpeed = 0;
        AverageSpeed=0;
        WaitingListOfTexts = null;
        WaitingListOfTexts=  new string[4, 5];
        LastFilledStandings = 0;
}
}
