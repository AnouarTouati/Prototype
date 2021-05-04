using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class RoomController : MonoBehaviourPunCallbacks,IPunObservable
{
    public Transform[] StartPosition;
    public int CurrentStartPositionIndex=0;
    public GameObject[] PickableContainers;
    public string GameMode;//try getting it from savegame of the host
    public RoomManager RoomManager;
    public SaveGame SaveGame;
    public PhotonView PV;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(GameMode);
            stream.SendNext(CurrentStartPositionIndex);
        }
        else
        {
            GameMode = (string)stream.ReceiveNext();
            CurrentStartPositionIndex = (int)stream.ReceiveNext();
        }
    }

    void Start()
    {
        if (RoomManager == null)
        {
            RoomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
        }
        if (SaveGame == null)
        {
            SaveGame = GameObject.Find("SaveGame").GetComponent<SaveGame>();
        }
        if (PhotonNetwork.IsMasterClient)
        {
            GameMode = SaveGame.GameMode;
        }

        AddOnlinePlayerManager();
       
    }

  
   void AddOnlinePlayerManager()
    {
        GameObject PlaceHolder = PhotonNetwork.Instantiate("PlayerPlaceHolder", Vector3.zero, Quaternion.identity);
        PlaceHolder.GetComponent<OnlinePlayerManager>().CarName = SaveGame.SelectedCarForPlay.CarName;
       // PlaceHolder.GetComponent<OnlinePlayerManager>().UserColor = SaveGame.SelectedCarForPlay.UserColor; 
        PlaceHolder.GetComponent<OnlinePlayerManager>().ProfileName = SaveGame.ProfileName;
       
    }
    void Update()
    {
        
    }
    public void ReturnToLobby()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<EngineAudio>().ResetTheScript();
      /* GameObject[] OtherPlayers=GameObject.FindGameObjectsWithTag("OtherPlayer");
        foreach (GameObject OtherPlayer in OtherPlayers)
        {
            OtherPlayer.GetComponent<EngineAudio>().ResetTheScript();
        }*/  
        PhotonNetwork.LeaveRoom();
     
    }
    #region CallBacks
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0);
    }
    
    #endregion
   
    }


