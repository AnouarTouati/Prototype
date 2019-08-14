/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class ServerControlledBehaviour : MonoBehaviour {

     GameObject TurboItem;
     GameObject WeaponItem;
    GameObject ShieldItem;

     CustomNetworkManager CustomNetworkManager;
     GameObject ChosenItemToSpawn;

    public Transform TurboItemSpawnPoint;
    public GameObject instance;
    public GamePlayScore GamePlayScore;
    public bool IhaveItemOnMe=false;
    public RaceSystem RaceSystem;
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            this.enabled = false;
        }
        else
        {
            CustomNetworkManager = GameObject.Find("NetworkManager").GetComponent<CustomNetworkManager>();
            for (int i = 0; i < CustomNetworkManager.PickableItems.Length; i++)
            {
                switch (CustomNetworkManager.PickableItems[i].name)
                {
                    case "TurboItem": TurboItem = CustomNetworkManager.PickableItems[i]; break;
                    case "WeaponItem": WeaponItem = CustomNetworkManager.PickableItems[i]; break;
                    case "ShieldItem": ShieldItem = CustomNetworkManager.PickableItems[i]; break;
                    default: Debug.LogError("An Extra Item Is Availble in CustomNetworkManager Add it in other scripts like ServerCtrolledBehaviour"); break;
                }
            }
            GamePlayScore = GetComponent<GamePlayScore>();
            RaceSystem = GameObject.Find("RaceSystem").GetComponent<RaceSystem>();
        }
       
   
    }
  
    
       [Command]
     void CmdAddTheItemToPlayer()
    {
        if (isServer && RaceSystem.GameMode=="DeathMatch")
        {
            instance = Instantiate(ChosenItemToSpawn);
            //the item will attach it self to the player throu Item  script
            instance.GetComponent<Items>().NameOfOwningPlayer = transform.name;
            NetworkServer.Spawn(instance);
        }
            
        
        
            
        
    }
    [Command]
    void CmdRemoveItemFromPlayer()
    {

        if (isServer && RaceSystem.GameMode == "DeathMatch")
        {
            NetworkServer.UnSpawn(instance);
            Destroy(instance);
        }
            
          
        
       
    }
    public IEnumerator StartAutoDestructionForItem(string NameOfTheItem)
    {
        if (isServer && RaceSystem.GameMode == "DeathMatch")
        {
            float ItemLifeTime=0;

            if (IhaveItemOnMe == false)
            {

                switch (NameOfTheItem)
                {
                    case "Turbo" : ItemLifeTime = TurboItem.GetComponent<Items>().LifeTime; ChosenItemToSpawn = TurboItem; break;
                    case "Weapon": ItemLifeTime = WeaponItem.GetComponent<Items>().LifeTime; ChosenItemToSpawn = WeaponItem; break;
                    case "Shield": ItemLifeTime = ShieldItem.GetComponent<Items>().LifeTime; ChosenItemToSpawn = ShieldItem; break;
                }

            
                CmdAddTheItemToPlayer();
                IhaveItemOnMe = true;
            }
            yield return new WaitForSeconds(ItemLifeTime);
            GetComponent<Perfermance>().ModifiableMaxTorqueByItems = GetComponent<Perfermance>().OriginalMaxTorque;//we reverted the effect of the item be for killing it
            CmdRemoveItemFromPlayer();
            IhaveItemOnMe = false;
        }
    }
   
  
}
*/