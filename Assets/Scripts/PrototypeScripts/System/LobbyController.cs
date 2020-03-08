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
    private int counter = 0;
    private bool tryToDisconnectToChangeSingleMulti = false;
   public void SetPlayModeSingleOrMultiPlayer(bool Singleplayer){//singleplayer button and multiplayer button
      
        GameObject.Find("SaveGame").GetComponent<SaveGame>().OfflineMode = Singleplayer;

        if (PhotonNetwork.OfflineMode != Singleplayer && PhotonNetwork.IsConnected)
        {
            tryToDisconnectToChangeSingleMulti = true;
            PhotonNetwork.Disconnect();    
          
        }
        else if(!PhotonNetwork.IsConnected)
        { 
            ConnectToMainServerPhoton();
        }
       
      
    
    }
    #region CallBacks
    public override void OnConnectedToMaster()
    {
        IsConnected = true;
        IsTryingToConnect = false;
        
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnect called");
        IsConnected = false;
        IsTryingToConnect = false;

        if (tryToDisconnectToChangeSingleMulti)
        {
            counter++;
            if (counter >= 2 && GameObject.Find("SaveGame").GetComponent<SaveGame>().OfflineMode == true)
            {
                counter = 0;
                ConnectToMainServerPhoton();
            }
            if (counter >= 3 && GameObject.Find("SaveGame").GetComponent<SaveGame>().OfflineMode == false)
            {
                counter = 0;
                ConnectToMainServerPhoton();
            }
        }
            

       
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
        tryToDisconnectToChangeSingleMulti = false;
        if (IsTryingToConnect == false)
        {
            IsTryingToConnect = true;
            //apply true to  PhotonNetwork.OfflineMode will create a server thus we dont need to call PhotonNetwork.ConnectUsingSettings()

            PhotonNetwork.OfflineMode= GameObject.Find("SaveGame").GetComponent<SaveGame>().OfflineMode;
            //this code just to ensure that we dont already have an offline server created by the above code
            if (!PhotonNetwork.OfflineMode)
            {
                
                PhotonNetwork.ConnectUsingSettings();
                
                // add a button to retry connection if failed
            }
            PhotonNetwork.AutomaticallySyncScene = true;
        }
       
    }
    public void ResetTheScript()
    {
      //  PhotonNetwork.Disconnect();
    }
}
