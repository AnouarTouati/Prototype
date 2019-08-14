/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PickableItemsContainersScript : NetworkBehaviour {

    public string NameOfTheItem;
    public int PickableItemsAvailbeInSceneID;
    private RaceSystem RaceSystem;
    void Start()
    {
        if (!isServer)
        {
          
            this.enabled = false;
        }
        else
        {
          
            RaceSystem = GameObject.Find("RaceSystem").GetComponent<RaceSystem>();
        }
           
      
    }
    void OnTriggerEnter (Collider info)
    {
        if (isServer)
        {
            if (info.transform.tag == "Player"|| info.transform.tag == "OtherPlayer")
            {
                if (info.GetComponent<ServerControlledBehaviour>().IhaveItemOnMe == false)
                {
                 
                   // Debug.Log("A player hit Item     " + info.GetComponent<NetworkIdentity>().netId);
                    RaceSystem.PickableItemsContainerInScene.Remove(this.gameObject);
                    RaceSystem.PickableItemsContainerAvailbeInScene.Remove(PickableItemsAvailbeInSceneID);

                    info.GetComponent<ServerControlledBehaviour>().StartCoroutine("StartAutoDestructionForItem", NameOfTheItem);

                    RaceSystem.StartCoroutine("PickableItemAutoRespawn");
                    NetworkServer.UnSpawn(this.gameObject);
                    Destroy(this.gameObject);
                }
               
            }
        }
    }
    
}
*/