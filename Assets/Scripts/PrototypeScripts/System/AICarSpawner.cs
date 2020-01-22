using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class AICarSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public RoomController RoomController;
    private bool AlreadyExecuted=false;
    private List<GameObject> AICars=new List<GameObject>();
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(!AlreadyExecuted){
            
            if(PhotonNetwork.OfflineMode){
                if(GameObject.FindGameObjectWithTag("LocalPlayerPlaceHolder")){
                    OnlinePlayerManager localOnlinePlayerManager=GameObject.FindGameObjectWithTag("LocalPlayerPlaceHolder").GetComponent<OnlinePlayerManager>();
                    GameObject AICar =  PhotonNetwork.Instantiate("Car"+Random.Range(1,3), RoomController.StartPosition[RoomController.CurrentStartPositionIndex].position, RoomController.StartPosition[RoomController.CurrentStartPositionIndex].rotation);
                    AICar.GetComponent<Motor>().AIDriving=true;
                    AICars.Add(AICar);
                    localOnlinePlayerManager.IncrementCurrentStartPositionIndex();
                    AlreadyExecuted=true;
                } 
            }
        
        }
    }
    public void KillAllAICars(){
            for (int i = 0; i < AICars.Count; i++)
            {
                GameObject temp=AICars[i];
                AICars.Remove(temp);
                Destroy(temp);
            }
            AlreadyExecuted=false;
           
    }
}
