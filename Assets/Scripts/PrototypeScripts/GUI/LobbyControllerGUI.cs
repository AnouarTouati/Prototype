using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyControllerGUI :MonoBehaviour  {

    public LobbyController LobbyController;
    public Dropdown OnlineScenesDropDown;
    public Dropdown GameModeDropDown;
    public int  number;
    List<string> SceneNames = new List<string>();

    public GameObject OnlineGUI;
    public GameObject OfflineGUI;
    public Button RetryConnectionToServerPhoton;
    private void Start()
    {
        OnlineGUI.SetActive(false);
        LobbyController = GameObject.Find("LobbyController").GetComponent<LobbyController>();

       LoadAvailbleSceneNamesToTheDropDown();
        LoadAvailbleGameModes();
       
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu" && GameObject.Find("NetworkManager"))
        {
            LobbyController = GameObject.Find("LobbyController").GetComponent<LobbyController>();
        }

        if (!LobbyController.IsConnected)
        {

            OnlineGUI.SetActive(false);
            OfflineGUI.SetActive(true);
        }
        else if (LobbyController.IsConnected)
        {

            OnlineGUI.SetActive(true);
            OfflineGUI.SetActive(false);
        }
        if (LobbyController.IsTryingToConnect)
        {
            RetryConnectionToServerPhoton.interactable = false;
            RetryConnectionToServerPhoton.GetComponentInChildren<Text>().text = "Connecting To Master Server ...";
        }
        else
        {
            RetryConnectionToServerPhoton.interactable = true;
            RetryConnectionToServerPhoton.GetComponentInChildren<Text>().text = "Retry Connection";
        }
            
        
    }
    public void StartPlaying()
    {
        LobbyController.StartPlaying();

    }
   
    public void LoadSelectedOnlineScene()
    {
        LobbyController.SelectedPlaySceneName = SceneNames[OnlineScenesDropDown.value];
    }
    public void LoadSelectedGameMode()
    {
        LobbyController.SelectedGameMode = LobbyController.AllGameModes[ GameModeDropDown.value];
    }
    void LoadAvailbleSceneNamesToTheDropDown()
    {
        OnlineScenesDropDown.ClearOptions();
        //   OnlineScenesDropDown.AddOptions(customNetworkManager.OnlineScenesNames);
        SceneNames.Clear();
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            SceneNames.Add(System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i)));
        }
        OnlineScenesDropDown.AddOptions(SceneNames);
    }
    void LoadAvailbleGameModes()
    {
        GameModeDropDown.ClearOptions();
        GameModeDropDown.AddOptions(LobbyController.AllGameModes);
    }

    
}
