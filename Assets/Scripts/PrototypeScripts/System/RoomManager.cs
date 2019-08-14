using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks,IPunObservable {

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
         
                stream.SendNext(NumberOfPlayersInLobby);
                stream.SendNext(LockDownActivated);
                stream.SendNext(RaceAllowedToStart);
           
         
        }
        else
        {
            NumberOfPlayersInLobby = (int)stream.ReceiveNext();
            LockDownActivated = (bool)stream.ReceiveNext();
           RaceAllowedToStart= (bool)stream.ReceiveNext();
         
        }
    }
    public List<GameObject> OnlinePlayerManagers;
   
    public int NumberOfPlayersInLobby;
    public float PlayerRespawnTime;
    public bool LockDownActivated = false;
   
    public bool RaceAllowedToStart = false;
    public float LockDownWaitTime = 3f;
    public int MinNumberOfPlayersToStartRace=2;
    public RaceSystem RaceSystem;
    
    public GUIGamePlay GUIGamePlay;
  // public  List<GameObject> gameObjectsListVersion = new List<GameObject>();
    private void Start()
    {
       
        if (GameObject.Find("RaceSystem")&&PhotonNetwork.IsMasterClient)
        {
            RaceSystem = GameObject.Find("RaceSystem").GetComponent<RaceSystem>();
        }
       
    }
    private void Update()
    {
       
         
         
       
        if (RaceSystem == null)
        {
            if (GameObject.Find("RaceSystem"))
            {
                RaceSystem = GameObject.Find("RaceSystem").GetComponent<RaceSystem>();
            }
        }else
       
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (!RaceSystem.RaceStarted)
                {
                    KeepCheckingForPlayersToReadyUP();
                }
                if(PhotonNetwork.OfflineMode){
                    RaceAllowedToStart=true;
                }
            }
           
        }
        
      
         
        
      
    }
    
  

  
    void KeepCheckingForPlayersToReadyUP()
    {
       if (PhotonNetwork.IsMasterClient &&  OnlinePlayerManagers.Count>=MinNumberOfPlayersToStartRace)
        {
         
            for (int i = 0; i < OnlinePlayerManagers.Count; i++)
            {

                if (OnlinePlayerManagers[i].GetComponent<OnlinePlayerManager>().isReadyToStartPlaying == false)
                {
                    return;
                }
            }
           
                PrepareForRaceStart();
          

        }


    }
    void PrepareForRaceStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            LockDownActivated = true;
            StartCoroutine(StartRace());
        }
            
        
      
    }
    IEnumerator StartRace()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            yield return new WaitForSeconds(LockDownWaitTime);
            RaceAllowedToStart = true;

        }


    }
   
    public void ResetTheScript()
    {
       RaceAllowedToStart = false;
        LockDownActivated = false;
    }
  
}
