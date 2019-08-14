using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class OnlinePlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            
                stream.SendNext(ProfileName);
                stream.SendNext(CarName);
                //stream.SendNext(UserColor);

                stream.SendNext(isDead);
                stream.SendNext(isReadyToStartPlaying);
          
            
        }
        else
        {
            ProfileName = (string)stream.ReceiveNext();

            CarName = (string)stream.ReceiveNext();

           // UserColor = (Color)stream.ReceiveNext();

          

            isDead = (bool)stream.ReceiveNext();

            isReadyToStartPlaying = (bool)stream.ReceiveNext();
        }
    }

    public string ProfileName;

    public string CarName;

    public Color UserColor;


    public bool isDead = true;

    public bool isReadyToStartPlaying = false;

    public GameObject ThisPlayerCar;
    public GameObject ThisPlayerCamera;
    public RoomManager RoomManager;
    public GamePlayScore GamePlayScore;

    public int myConnectionId;
    public bool FirstCreationOfPlayerAleadyDone = false;
    public RoomController RoomController;
    public PhotonView PV;
    
    void Start()
    {

        if (PV.IsMine)
        {
            
            transform.tag = "LocalPlayerPlaceHolder";
            transform.name = ProfileName + " Place Holder";
            if (RoomController == null)
            {
                RoomController = GameObject.Find("RoomController").GetComponent<RoomController>();
            }
        }
        else
        {
            transform.tag = "OtherPlayerPlaceHolder";
            transform.name = ProfileName + " Place Holder";
        }

        if (GameObject.Find("RoomManager"))
        {
            RoomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
            RoomManager.OnlinePlayerManagers.Add(this.gameObject);
        }




    }


    void Update()
    {
        if (RoomController == null)
        {
            RoomController = GameObject.Find("RoomController").GetComponent<RoomController>();
        }
        if (RoomManager != null)
        {
            if (RoomManager.RaceAllowedToStart == true && isDead==true && PV.IsMine)
            {
                Respawn();

            }



            if (isDead == false)
            {
               
                if (ThisPlayerCar == null)
                {
                    ThisPlayerCar = GameObject.Find(ProfileName);
                    if (ThisPlayerCar != null)
                    {
                        ThisPlayerCar.GetComponent<Transform>().parent = this.transform;
                    }
                }
                else if (GamePlayScore == null)
                {
                    GamePlayScore = GetComponentInChildren<GamePlayScore>();
                }
                else
                {
                    if (PV.IsMine  && GamePlayScore.PlayerHealth <= 0)
                    {
                        KillMySelf();
                     
                        StartCoroutine(RespawnCountDown());

                    }
                }
            }
        }
        else
        {
            if (GameObject.Find("RoomManager"))
            {
                RoomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
            }
        }

    }
  
 
    void Respawn()
    {
        ThisPlayerCar = PhotonNetwork.Instantiate(CarName, RoomController.StartPosition[RoomController.CurrentStartPositionIndex].position, RoomController.StartPosition[RoomController.CurrentStartPositionIndex].rotation);
         PV.RPC("IncrementCurrentStartPositionIndex", RpcTarget.MasterClient);
        ThisPlayerCar.GetComponent<CarVisualSync>().InstantiedFromSaveGameForInitializingOnly = false;
        ThisPlayerCar.GetComponent<GamePlayScore>().PlayerName = ProfileName;
        FirstCreationOfPlayerAleadyDone = true;
            isDead = false;
        

    }

    [PunRPC]
    public void IncrementCurrentStartPositionIndex()
    {
       RoomController.CurrentStartPositionIndex++;
        if (RoomController.CurrentStartPositionIndex >= RoomController.StartPosition.Length)
        {
            RoomController.CurrentStartPositionIndex = 0;
        }
        

    }
    
    void Addplayer(int[] EngineSuspensionTiresBrake,string[] CarNameUserColorProfileName)
    {
        


    }
   
  public   void KillMySelf()
    {
        ThisPlayerCar.GetComponent<EngineAudio>().ResetTheScript();
        isDead = true;
        PhotonNetwork.Destroy(ThisPlayerCar);
        Destroy(GameObject.Find("CamParent"));//i did it directly With find function because CarCamScript destroys it at start if it is not local 
                                               // i used Destroy and not PhotonNetwork.Destory because CamParent is not Networked
                                               //should add am dead
        isReadyToStartPlaying = false;
        
    }

    IEnumerator RespawnCountDown()
    {

      
            yield return new WaitForSeconds(RoomManager.PlayerRespawnTime);
            Respawn();

        

    }

    public void SetUnSetReady()
    {
        if (!RoomManager.LockDownActivated)
        {
            isReadyToStartPlaying = !isReadyToStartPlaying;
        }

    }
   
}


