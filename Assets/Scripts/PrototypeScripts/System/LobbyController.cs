using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class LobbyController : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public string SelectedPlaySceneName;
    [HideInInspector]
    public string SelectedGameMode;
    public List<string> AllGameModes;
    public SaveGame SaveGame;
  
    public bool IsTryingToConnect = false;
    public bool IsConnected = false;
    void Start()
    {
      
       
    }
   
    void Update()
    {
       

     
    }
   public void CalledFromButtonsOfPlay(bool Singleplayer){//singleplayer button and multiplayer button
      IsConnected=false;
      GameObject.Find("SaveGame").GetComponent<SaveGame>().OfflineMode=Singleplayer;
      ConnectToMainServerPhoton();
    }
    #region CallBacks
    public override void OnConnectedToMaster()
    {
        IsConnected = true;
        IsTryingToConnect = false;
        Debug.Log("Connected To MasterServer");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        IsConnected = false;
        IsTryingToConnect = false;

    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
       
        CreateaRoom();
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        //should ask for player decision
        CreateaRoom();
    }
    public override void OnJoinedRoom()
    {
        //only master is allowed to change scene since   PhotonNetwork.AutomaticallySyncScene is set to true in start function client will auto sync to that scene
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }
    #endregion
    void CreateaRoom()
    {
        int RoomRandomNumber = Random.Range(0, 1000);
        RoomOptions RoomOptions = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = 4 };
        PhotonNetwork.CreateRoom("Room" + RoomRandomNumber, RoomOptions);
    }
    public void StartPlaying()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public void ConnectToMainServerPhoton()
    {
        if (IsTryingToConnect == false)
        {
          PhotonNetwork.OfflineMode= GameObject.Find("SaveGame").GetComponent<SaveGame>().OfflineMode;
         
            if (!IsConnected)
            {
                IsTryingToConnect = true;
                PhotonNetwork.ConnectUsingSettings();
                // add a button to retry connection if failed
            }
            PhotonNetwork.AutomaticallySyncScene = true;
        }
       
    }
}
