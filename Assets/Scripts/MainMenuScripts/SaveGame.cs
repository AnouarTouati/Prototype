using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;


public class  SaveGame :MonoBehaviour {

    public List<PurshacedCarsAndThierUserSettings> PurchasedCarsList;//we should change inside this unless it is the same
    public PurshacedCarsAndThierUserSettings SelectedCarForPlay;
   // public PurshacedCarsAndThierUserSettings SelectedCar;
    public int j; //just temp number fo iteration
 
    public GameObject[] AllCarsPrefabs;
    public string ProfileName;
    public CarSelectionMenuScript CarSelectionMenuScript;
    public MainMenuScript MainMenuScript;
 //   [HideInInspector]
    public List<string> NamesOfAvailibleProfiles;
    public string GameMode;
   
    public static SaveGame SaveGameSingleton;
     float[] tempBodyColorArray=new float[4];
  float[] tempSideViewColorArray =new float[4];
      float[] tempHoodColorArray = new float[4];
     float[] tempRimsColorArray = new float[4];
    GameObject TheSelectedCarForTempColors;
    [Header("Scene Switch Sync Data")]
    public bool PlayerAlreadyLoadedAProfileNoNeedToReload = false;
        public bool JustReturnedFromRoomToLobby = false;
        public bool OfflineMode = false;
    void Start () {
        if (SaveGameSingleton != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            SaveGameSingleton = this;
        }
       DontDestroyOnLoad(this.gameObject);/*this will make sure that the network manager always have access to the savegame whenever we change scne 
       // take in consideration that other players can munipilate the values of this local player savegame*/
        
      

       
       // LoadTheListOfAvailbleProfiles();

        



        /*   for (int i = 0; i < PurchasedCarsList.Count; i++)
           {
               PurchasedCarsList[i].UserColor = "blue";
           }
           */
    }
   
	void Update()
    {
     //   Debug.Log(Application.persistentDataPath);
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {

        if (CarSelectionMenuScript == null && GameObject.Find("CarSelectionGUI"))
            {
                CarSelectionMenuScript = GameObject.Find("CarSelectionGUI").GetComponent<CarSelectionMenuScript>();
            }

            if (MainMenuScript == null && GameObject.Find("MainMenu"))
            {
                MainMenuScript = GameObject.Find("MainMenu").GetComponent<MainMenuScript>();
            }
       
        }
        else
        {
            PlayerAlreadyLoadedAProfileNoNeedToReload = true;
            JustReturnedFromRoomToLobby = true;
          
        }
        
    }
    public void SaveCustomization()
    {
        SavePlayer();
    }
    public void SavePurchaseCar(string PurchasedCarName)
    {
        // if we puchase a car we have to set it as selected in carselection menu so when we go back to main menu it loads correctly
        for (int i = 0; i<AllCarsPrefabs.Length; i++)
        {
            if (PurchasedCarName == AllCarsPrefabs[i].name)
            {
                TheSelectedCarForTempColors =Instantiate( AllCarsPrefabs[i]);
                TheSelectedCarForTempColors.GetComponent<CarVisualSync>().InstantiedFromSaveGameForInitializingOnly = true;
                break;
            }
        }
        
        LoadTempColors();
        
        SelectedCarForPlay = new PurshacedCarsAndThierUserSettings(PurchasedCarName,tempBodyColorArray,tempSideViewColorArray,tempHoodColorArray,tempRimsColorArray);
        PurchasedCarsList.Add(SelectedCarForPlay);
        SavePlayer();
        
    }
    public void SaveSellCar(PurshacedCarsAndThierUserSettings SoldCar)
    {
      

            PurchasedCarsList.Remove(SoldCar);

            if (PurchasedCarsList.Count > 0)
            {
                SelectedCarForPlay = PurchasedCarsList[0];
                SavePlayer();
                CarSelectionMenuScript.LoadCar();
            }
            else
            {

            TheSelectedCarForTempColors = Instantiate(AllCarsPrefabs[0]);
            TheSelectedCarForTempColors.GetComponent<CarVisualSync>().InstantiedFromSaveGameForInitializingOnly = true;
            LoadTempColors();
            SelectedCarForPlay =new PurshacedCarsAndThierUserSettings(AllCarsPrefabs[0].name, tempBodyColorArray, tempSideViewColorArray, tempHoodColorArray, tempRimsColorArray);
                SavePlayer();
            }
        
       
        
    }
    public void SavePlayer()
    {
        
        BinaryFormatter bf = new BinaryFormatter();
        
        FileStream streamOpen = new FileStream(Application.persistentDataPath + "/Player.sav", FileMode.Open);
        PlayerData dataSingleProfile = new PlayerData(PurchasedCarsList, SelectedCarForPlay, ProfileName,GameMode);
        List<PlayerData> data = bf.Deserialize(streamOpen) as List<PlayerData>;
        streamOpen.Close();
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i].ProfileName == ProfileName)
            {
                data[i] = dataSingleProfile;
                break;
            }
        }
        FileStream streamCreate = new FileStream(Application.persistentDataPath + "/Player.sav", FileMode.Create);
        bf.Serialize(streamCreate, data);
         streamCreate.Close();
      
        
    }
    public void LoadPlayer(string ProfileNameToLoad)
    {
       
            BinaryFormatter bf = new BinaryFormatter();
            FileStream streamOpen = new FileStream(Application.persistentDataPath + "/Player.sav", FileMode.Open);
            List<PlayerData> data = bf.Deserialize(streamOpen) as List<PlayerData>;
        TheSelectedCarForTempColors = Instantiate(AllCarsPrefabs[0]);
        TheSelectedCarForTempColors.GetComponent<CarVisualSync>().InstantiedFromSaveGameForInitializingOnly = true;
        LoadTempColors();
        PlayerData dataSinglepProfile=new PlayerData(new List<PurshacedCarsAndThierUserSettings>() ,new PurshacedCarsAndThierUserSettings(AllCarsPrefabs[0].name, tempBodyColorArray, tempSideViewColorArray, tempHoodColorArray, tempRimsColorArray),ProfileNameToLoad,GameMode);//just to instantiate
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i].ProfileName == ProfileNameToLoad)
            {
                dataSinglepProfile = data[i];
                break;
            }
         
        }
       
            streamOpen.Close();
             PurchasedCarsList= dataSinglepProfile.PurchasedCarsBinary;
            SelectedCarForPlay= dataSinglepProfile.SelectedCarForPlayBinary;
            ProfileName = dataSinglepProfile.ProfileName;
        GameMode = dataSinglepProfile.GameMode;
        streamOpen.Close();

       
    }
    public void RegisterNewPlayerProfile(string NewProfileName)
    {
      
        if (File.Exists(Application.persistentDataPath + "/Player.sav")){
            Debug.Log("RegisterNewProfile in SaveGame was called");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream streamOpen = new FileStream(Application.persistentDataPath + "/Player.sav", FileMode.Open);
            List<PlayerData> data = bf.Deserialize(streamOpen) as List<PlayerData>;
            streamOpen.Close();
            TheSelectedCarForTempColors = Instantiate(AllCarsPrefabs[0]);
            TheSelectedCarForTempColors.GetComponent<CarVisualSync>().InstantiedFromSaveGameForInitializingOnly = true;
            LoadTempColors();
            PurshacedCarsAndThierUserSettings newTempSelectedCarForPlay=new PurshacedCarsAndThierUserSettings(AllCarsPrefabs[0].name, tempBodyColorArray, tempSideViewColorArray, tempHoodColorArray, tempRimsColorArray);
            List<PurshacedCarsAndThierUserSettings> newTempPurshacedCarsList=new  List<PurshacedCarsAndThierUserSettings>();
            newTempPurshacedCarsList.Add(newTempSelectedCarForPlay);

            data.Add(new PlayerData(newTempPurshacedCarsList, newTempSelectedCarForPlay, NewProfileName,/*gameMode*/"Racing"));
           
            
            FileStream streamCreate = new FileStream(Application.persistentDataPath + "/Player.sav", FileMode.Create);
            bf.Serialize(streamCreate, data);
            streamCreate.Close();
          //  Debug.Log("New Profile Created");
            LoadPlayer(NewProfileName);
            LoadTheListOfAvailbleProfiles();
           
        }
        else
        {
           
            BinaryFormatter bf = new BinaryFormatter();
            FileStream streamCreate = new FileStream(Application.persistentDataPath + "/Player.sav", FileMode.Create);
            List<PlayerData> data = new List<PlayerData>();
            TheSelectedCarForTempColors = Instantiate(AllCarsPrefabs[0]);
            TheSelectedCarForTempColors.GetComponent<CarVisualSync>().InstantiedFromSaveGameForInitializingOnly = true;
            LoadTempColors();
            PurshacedCarsAndThierUserSettings newTempSelectedCarForPlay = new PurshacedCarsAndThierUserSettings(AllCarsPrefabs[0].name, tempBodyColorArray, tempSideViewColorArray, tempHoodColorArray, tempRimsColorArray);
            List<PurshacedCarsAndThierUserSettings> newTempPurshacedCarsList = new List<PurshacedCarsAndThierUserSettings>();
            newTempPurshacedCarsList.Add(new PurshacedCarsAndThierUserSettings(AllCarsPrefabs[0].name, tempBodyColorArray, tempSideViewColorArray, tempHoodColorArray, tempRimsColorArray));
            data.Add(new PlayerData(newTempPurshacedCarsList, newTempSelectedCarForPlay, NewProfileName,/*gameMode*/"Racing"));
           
            bf.Serialize(streamCreate, data);
            streamCreate.Close();
            LoadPlayer(NewProfileName);
            LoadTheListOfAvailbleProfiles();
        }
     
    }
    public void DeletePlayerProfile(int Index)
    {
        if (File.Exists(Application.persistentDataPath + "/Player.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream streamOpen = new FileStream(Application.persistentDataPath + "/Player.sav", FileMode.Open);
            List<PlayerData> data = bf.Deserialize(streamOpen) as List<PlayerData>;
            data.Remove(data[Index]);
            streamOpen.Close();
            FileStream streamCreate = new FileStream(Application.persistentDataPath + "/Player.sav", FileMode.Create);
            bf.Serialize(streamCreate, data);
            streamCreate.Close();

         
        }
    }
    public void LoadTheListOfAvailbleProfiles()
    {
       
        if (File.Exists(Application.persistentDataPath + "/Player.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream streamOpen = new FileStream(Application.persistentDataPath + "/Player.sav", FileMode.Open);
            List<PlayerData> data = bf.Deserialize(streamOpen) as List<PlayerData>;
            if (data.Count == 0)
            {
                NamesOfAvailibleProfiles.Clear();
                MainMenuScript.SwitchToProfileMenu();
            }
            else
            {
                NamesOfAvailibleProfiles.Clear();
                for (int i = 0; i < data.Count; i++)
                {
                    NamesOfAvailibleProfiles.Add(data[i].ProfileName);
                }
                MainMenuScript.SwitchToProfileMenu();
            }
            
            streamOpen.Close();
        }
       

    }
   void LoadTempColors()
    {
       

        tempBodyColorArray[0]=TheSelectedCarForTempColors.GetComponent<CarVisualSync>().DefaultBodyColor.r;
        tempBodyColorArray[1]= TheSelectedCarForTempColors.GetComponent<CarVisualSync>().DefaultBodyColor.g;
        tempBodyColorArray[2] = TheSelectedCarForTempColors.GetComponent<CarVisualSync>().DefaultBodyColor.b;
        tempBodyColorArray[3] = TheSelectedCarForTempColors.GetComponent<CarVisualSync>().DefaultBodyColor.a;

        tempSideViewColorArray[0] = TheSelectedCarForTempColors.GetComponent<CarVisualSync>().DefaultSideViewMirrorsColor.r;
        tempSideViewColorArray[1] = TheSelectedCarForTempColors.GetComponent<CarVisualSync>().DefaultSideViewMirrorsColor.g;
        tempSideViewColorArray[2] = TheSelectedCarForTempColors.GetComponent<CarVisualSync>().DefaultSideViewMirrorsColor.b;
        tempSideViewColorArray[3] = TheSelectedCarForTempColors.GetComponent<CarVisualSync>().DefaultSideViewMirrorsColor.a;

        tempHoodColorArray[0] = TheSelectedCarForTempColors.GetComponent<CarVisualSync>().DefaultHoodColor.r;
        tempHoodColorArray[1] = TheSelectedCarForTempColors.GetComponent<CarVisualSync>().DefaultHoodColor.g;
        tempHoodColorArray[2] = TheSelectedCarForTempColors.GetComponent<CarVisualSync>().DefaultHoodColor.b;
        tempHoodColorArray[3] = TheSelectedCarForTempColors.GetComponent<CarVisualSync>().DefaultHoodColor.a;

        tempRimsColorArray[0] = TheSelectedCarForTempColors.GetComponent<CarVisualSync>().DefaultRimsColor.r;
        tempRimsColorArray[1] = TheSelectedCarForTempColors.GetComponent<CarVisualSync>().DefaultRimsColor.g;
        tempRimsColorArray[2] = TheSelectedCarForTempColors.GetComponent<CarVisualSync>().DefaultRimsColor.b;
        tempRimsColorArray[3] = TheSelectedCarForTempColors.GetComponent<CarVisualSync>().DefaultRimsColor.a;
        Destroy(TheSelectedCarForTempColors);
    }

}
[Serializable]
public class PlayerData
{
    public List<PurshacedCarsAndThierUserSettings> PurchasedCarsBinary;
    public PurshacedCarsAndThierUserSettings SelectedCarForPlayBinary;
    public string ProfileName;

    [Header("Achievements")]
    public float TotalTimePlayed;
    public int TotalWins;
    public int NumberOfRaceWins;
    public int NumberOfDeathMatchWins;

    [Header("ServerSettings")]
    public string GameMode;


    //u should add unlock parts achievments and so on
    public PlayerData(List<PurshacedCarsAndThierUserSettings> PurchasedCarsNames, PurshacedCarsAndThierUserSettings SelectedCarName,string NewProfileName,string gameMode)
    {
        PurchasedCarsBinary = PurchasedCarsNames;
        SelectedCarForPlayBinary = SelectedCarName;
        ProfileName = NewProfileName;
        GameMode = gameMode;
        for (int i = 0; i < PurchasedCarsBinary.Count; i++)
        {
            if (PurchasedCarsBinary[i].CarName == SelectedCarName.CarName)
            {
                PurchasedCarsBinary[i] = SelectedCarName;
                break;
            }
        }
       
        
    }
}
[Serializable]
public class PurshacedCarsAndThierUserSettings
{
    //setting on this car
    public string CarName;
    public float[] BodyColor;
    public float[] SideViewMirrorsColor ;
    public float[] HoodColor;
    public float[] RimsColor ;


    //////////////////////////////////////////////////////////

    public int EngineUpgrade=0;
    public int SuspensionUpgrade=0;
    public int TiersUpgrade=0;
    public int BrakeUpgrade= 0;

    //////////////////////////////////////////////////////////
    

    public PurshacedCarsAndThierUserSettings(string name,float[] DefaultBodyColorArray,float[] DefaultSideViewMirrorsColorArray,float[] DefaultHoodColorArray,float[] DefaultRimsColorArray)
    {
        CarName = name;
        BodyColor = DefaultBodyColorArray;
        SideViewMirrorsColor = DefaultSideViewMirrorsColorArray;
        HoodColor = DefaultHoodColorArray;
        RimsColor = DefaultRimsColorArray;
      

    }
    
}

