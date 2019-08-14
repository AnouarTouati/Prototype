using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class RaceSystem : MonoBehaviourPunCallbacks,IPunObservable {
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
        //    if(PhotonNetwork.IsMasterClient )
          //  {
               
                stream.SendNext(RaceStarted);
                stream.SendNext(CountDownTime);
          //  }
        }
        else
        {
           
                RaceStarted = (bool)stream.ReceiveNext();
            CountDownTime = (float)stream.ReceiveNext();
        }
    }
    public PhotonView PV;
    public GameObject[] CheckPoints;
    
    public Transform[] WayPoints;
    
    
   
    public bool RaceStarted = false;
   
    
    
    public float CountDownTime = 3;
    public GUIGamePlay GUIGamePlay;
    public Color LineColor;

 
    public GameObject prefab;
    public Mesh ms;
    [HideInInspector]
    public Vector3[] AllVertices;

    [Header("SpawningItems")]
    public const float RespawnItemDelay=10f;
    public Transform[] PickableItemsContainersSpawnPoints;
    private Transform AvailableSpawnPoint;
   // public GameObject[] PickableItemsContainerToSpawn;
   [HideInInspector]
    public List<GameObject> PickableItemsContainerInScene=new List<GameObject>();
    [HideInInspector]
    public List<int> PickableItemsContainerAvailbeInScene;
    public RoomManager RoomManager;
    
    public RoomController RoomController;
  
  

    Predicate<int> NumberRepetetion = (int p) => { return p == TheIDofPickableItemToCheckItsRepeition; };

    public static int TheIDofPickableItemToCheckItsRepeition;

    void Start ()
    {
        RoomController = GameObject.Find("RoomController").GetComponent<RoomController>();
        if (GameObject.Find("RoomManager"))
        {
            RoomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
        }
        
        GUIGamePlay = GameObject.Find("GUI").GetComponent<GUIGamePlay>();
      

        
        
        if (ms != null&& RoomController.GameMode=="Racing")
        {
            AllVertices = ms.vertices;
            RandomizeWayPoints();
        }
        
    /*   if( GameMode == "DeathMatch")
        {
            for (int i = 0; i < RoomController.PickableContainers.Length; i++)
            {
                RoomController.PickableContainers[i].GetComponent<PickableItemsContainersScript>().PickableItemsAvailbeInSceneID = i;
            }
            HandleSpawningPickableItemsContainer(false);
              DeActivateTheCheckPoints();
           
        }
       */

       
    }
	
	
	void Update ()

    {
        if (GUIGamePlay == null)
        {
            GUIGamePlay = GameObject.Find("GUI").GetComponent<GUIGamePlay>();
            
        }
        if (RoomController == null)
        {
            RoomController = GameObject.Find("RoomController").GetComponent<RoomController>();
            
        }
        else{
           
        }
           
        
       
        if(GUIGamePlay != null && RoomManager.RaceAllowedToStart)
        {

            if ( CountDownTime > 0 && GUIGamePlay != null )
            {
                if (CountDownTime == 3)
                {
                    GUIGamePlay.EnablePreRaceGUI();
                }

                CountDownTime -= Time.deltaTime;
            }
            if (CountDownTime <= 0 /*&& RaceStarted == false*/ && GUIGamePlay != null && RaceStarted==false)
            {
                RaceStarted = true;
                if (RoomController.GameMode == "Racing")
                {
                    ReActivateTheCheckPoints();
                }
               
                GUIGamePlay.EnableRaceGUI();
            }
        }
       
       
       
        
    }
   /* void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        for (int i = 1; i < WayPoints.Length; i++)
        {
          
            Vector3 current = WayPoints[i].transform.position;
            Vector3 previous = WayPoints[i - 1].transform.position;
            Gizmos.DrawLine(current, previous);
            Gizmos.DrawSphere(current, 1f);
            // Gizmos.DrawLine(ms.vertices[i], ms.vertices[i-1]);
        //    Gizmos.DrawSphere(ms.vertices[i], 0.3f);
          //  Debug.Log(ms.vertices[i]);
        }
      
        
    }*/
    
    public void DeActivateTheCheckPoints()
    {
        if (RoomController.GameMode == "DeathMatch")
        {
            for (int i = 0; i < CheckPoints.Length; i++)
            {
                CheckPoints[i].SetActive(false);
            }
        }
    }
    public void ReActivateTheCheckPoints()
    {
        if (RoomController.GameMode == "Racing")
        {
            for (int i = 0; i < CheckPoints.Length; i++)
            {

                CheckPoints[i].GetComponent<CheckPoint>().MyIndex = i;
                for (int j= 0; j < CheckPoints[i].GetComponent<CheckPoint>().MeshRenderes.Length; j++)
                {
                    CheckPoints[i].GetComponent<CheckPoint>().MeshRenderes[j].enabled = true;
                }
              
                if (i != CheckPoints.Length - 1)
                {
                    CheckPoints[i].GetComponent<CheckPoint>().AmITheFinalCheckPoint = false;
                }
                else
                {
                    CheckPoints[i].GetComponent<CheckPoint>().AmITheFinalCheckPoint = true;

                }


            }
        }
        

    }

   public void RandomizeWayPoints()
    {
        float temp=0.5f;
        for (int i = 1; i < WayPoints.Length; i++)
        {
          
          
            Vector3 previous = WayPoints[i - 1].transform.position;
            if (i % 4 == 0)
            {
                if (temp > 0)
                {
                    temp = -0.5f;
                }
                else
                {
                    temp = 0.5f;
                }
               
            }
            else
            {
                temp = temp * 2;
            }
            WayPoints[i].transform.position +=  new Vector3(temp, 0, 0);
        }

    }
   [PunRPC]
    void RPC_PickableItemsContainer(GameObject PickableItemsContainerToSpawn, Vector3 SpawnPointPosition,Quaternion SpawnPointRotation)
    {
        if (PhotonNetwork.IsMasterClient && RoomController.GameMode =="DeathMatch")
        {
           
            GameObject instance = PhotonNetwork.Instantiate(PickableItemsContainerToSpawn.name, SpawnPointPosition, SpawnPointRotation);
            PickableItemsContainerInScene.Add(instance);
           
        }
    }
    public void HandleSpawningPickableItemsContainer(bool AutoRespawn)
    {
        if(RoomController.GameMode == "DeathMatch")
        {
            if (PhotonNetwork.IsMasterClient && AutoRespawn == false)
            {
                for (int i = 0; i < PickableItemsContainersSpawnPoints.Length; i++)
                {
                    int a = UnityEngine.Random.Range(0, RoomController.PickableContainers.Length);

                    RPC_PickableItemsContainer(RoomController.PickableContainers[a], PickableItemsContainersSpawnPoints[i].position, PickableItemsContainersSpawnPoints[i].rotation);
                    PickableItemsContainerAvailbeInScene.Add(a);


                }

            }
            if (PhotonNetwork.IsMasterClient && AutoRespawn == true)
            {
                int TheLeastAvailbleItem = 0;
                List<int> NumberOfRepetetionOfItems = new List<int>();
                for (int i = 0; i < RoomController.PickableContainers.Length; i++)
                {
                    TheIDofPickableItemToCheckItsRepeition = i;
                    NumberOfRepetetionOfItems.Add(PickableItemsContainerAvailbeInScene.FindAll(NumberRepetetion).Count);
                }



                int TheSmallestRepetetion = NumberOfRepetetionOfItems[0];
                TheLeastAvailbleItem = 0;
                for (int i = 0; i < RoomController.PickableContainers.Length; i++)
                {

                    if (NumberOfRepetetionOfItems[i] < TheSmallestRepetetion)
                    {
                        TheSmallestRepetetion = NumberOfRepetetionOfItems[i];
                        TheLeastAvailbleItem = i;
                    }

                }
                DetermineWhichSpawnPointIsEmpty();

                RPC_PickableItemsContainer(RoomController.PickableContainers[TheLeastAvailbleItem], AvailableSpawnPoint.position, AvailableSpawnPoint.rotation);
                PickableItemsContainerAvailbeInScene.Add(TheLeastAvailbleItem);

            }
        }
        
    }
  IEnumerator PickableItemAutoRespawn()
    {
        if (PhotonNetwork.IsMasterClient && RoomController.GameMode == "DeathMatch")
        {
            
           yield return new WaitForSeconds(RespawnItemDelay);
            HandleSpawningPickableItemsContainer(true);
        }
       
    }
    void DetermineWhichSpawnPointIsEmpty()
    {


        List<Transform> CloneOfPickableItemSpwanPoints = new List<Transform>(PickableItemsContainersSpawnPoints);
         
            for (int i = 0; i < PickableItemsContainerInScene.Count; i++)
            {

            CloneOfPickableItemSpwanPoints.RemoveAll(item => item.position == PickableItemsContainerInScene[i].transform.position);
                  
            }
        AvailableSpawnPoint = CloneOfPickableItemSpwanPoints[0];

      //  Debug.Log(AvailableSpawnPoint.position);

      
    }
    
    public void ResetTheScript()
    {
         ReActivateTheCheckPoints();
         CountDownTime = 3f;
        RaceStarted = false;
        PickableItemsContainerAvailbeInScene.Clear();
        PickableItemsContainerInScene.Clear();
    }
        
     
    

    
}
