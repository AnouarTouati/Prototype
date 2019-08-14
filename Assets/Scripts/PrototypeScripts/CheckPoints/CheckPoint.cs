﻿using UnityEngine;
using Photon.Pun;

public class CheckPoint : MonoBehaviourPunCallbacks,IPunObservable {
    public PhotonView PV;
   
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
       if(stream.IsWriting)
        {
            //used just to sync from server to other players which Checkpoints are active and which are not and the final checkpoint
            //by default scene object are controlled by the server so no need to check for PhotonNetwork.IsMasterClient
                stream.SendNext(MyIndex);
                stream.SendNext(AmITheFinalCheckPoint);
            

        }
        else
        {
            MyIndex = (int)stream.ReceiveNext();
            AmITheFinalCheckPoint = (bool)stream.ReceiveNext();
        }
    }
  
    public int MyIndex;
 
    public bool AmITheFinalCheckPoint = false;
    public MeshRenderer[] MeshRenderes;

    private void Update()
    {
        if(AmITheFinalCheckPoint == true && GetComponentInChildren<TextMesh>().text != "Finish")
        {
            GetComponentInChildren<TextMesh>().text = "Finish";
        }
       
    }
    void OnTriggerEnter(Collider info)
    {
      
     
           
            if (info.tag == "Player" || info.tag == "OtherPlayer" || info.tag=="AIDriving")
            {
                
                if (MyIndex == info.GetComponent<GamePlayScore>().NumberOfCheckPointsICrossed)
                {
                    
                    info.GetComponent<GamePlayScore>().NumberOfCheckPointsICrossed++;
                    if (AmITheFinalCheckPoint==true)
                    {
                        info.GetComponent<GamePlayScore>().FinishedTheRace = true;
                    }
                  //  RPC_DisableMySelf(info.transform.name);
                    PV.RPC("RPC_DisableMySelf", RpcTarget.All, info.transform.name);
                   
                }
            

        }

    }
    [PunRPC]
    void RPC_DisableMySelf(string name)
    {
        //we find the game where a player tagged "Player"  and have the name of the one that trigeered this function to make sure we only disable this checkpoint for that specific player while it is still showing for others
        if (GameObject.FindGameObjectWithTag("Player").transform.name == name)
        {
            for (int i = 0; i < MeshRenderes.Length; i++)
            {
                MeshRenderes[i].enabled = false;
            }

           
            //we must not destroy the check points  in server because check for clients is dependent on the server
        }
    }

   
    
}
