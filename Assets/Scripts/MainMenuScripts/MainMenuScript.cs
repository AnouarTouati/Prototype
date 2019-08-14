using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuScript :MonoBehaviour {

    
    public GameObject[] Menus;
    public Transform CarSpawnPoint;
    public CarSelectionMenuScript CarSelectioMenuScript;
    public BuyCarMenuScript BuyCarMenuScript;

    public SaveGame SaveGame;
    public LobbyController LobbyController;
    public void LoadNewGame()//just temporary
    {
        SceneManager.LoadScene(1);
        

    }
   
    public void SwitchToMainMenu()
    {
       
        BuyCarMenuScript.DestroyTheTempCar();///we do this to prevent showing two cars on the same time while switching between menus
        CarSelectioMenuScript.LoadCar();///we do this to prevent showing two cars on the same time while switching between menus
        for (int i = 0; i < Menus.Length; i++)
        {
            if (i != 0)
            {
                Menus[i].SetActive(false);
            }
            else
            {
                Menus[i].SetActive(true);
            }
        }
        SaveGame = GameObject.Find("SaveGame").GetComponent<SaveGame>();
        if (SaveGame.JustReturnedFromRoomToLobby )
        {

            SwitchToNetworkMenu();
            GameObject.Find("LobbyController").GetComponent<LobbyController>().CalledFromButtonsOfPlay(SaveGame.OfflineMode);
            SaveGame.JustReturnedFromRoomToLobby = false;
            
        }


    }
    public void SwitchToCarSelectionMenu()
    {


        BuyCarMenuScript.DestroyTheTempCar();///we do this to prevent showing two cars on the same time while switching between menus
        CarSelectioMenuScript.LoadCar();
        for (int i = 0; i < Menus.Length; i++)
        {
            if (i != 1)
            {
                Menus[i].SetActive(false);
            }
            else
            {
                Menus[i].SetActive(true);
            }
        }

    }

    public void SwitchToBuyCarMenu()
    {
        CarSelectioMenuScript.DestroyTheTempCar();///we do this to prevent showing two cars on the same time while switching between menus
        BuyCarMenuScript.LoadCar();
        for (int i = 0; i < Menus.Length; i++)
        {
            if (i != 2)
            {
                Menus[i].SetActive(false);
            }
            else
            {
                Menus[i].SetActive(true);
            }
        }

    }
    public void SwitchToCustomizationMenu()
    {
        
        for (int i = 0; i < Menus.Length; i++)
        {
            if (i != 3)
            {
                Menus[i].SetActive(false);
            }
            else
            {
                Menus[i].SetActive(true);
            }
        }

    }
    public void SwitchToProfileMenu()
    {

        for (int i = 0; i < Menus.Length; i++)
        {
            if (i != 4)
            {
                Menus[i].SetActive(false);
            }
            else
            {
                Menus[i].SetActive(true);
                Menus[i].GetComponent<ProfileMenuScript>().FillTheButtons();
            }
        }

    }
    public void SwitchToNetworkMenu()
    { 

        for (int i = 0; i < Menus.Length; i++)
        {
            if (i != 5)
            {
                Menus[i].SetActive(false);
            }
            else
            {
                Menus[i].SetActive(true);
            }
        }

    }

    public void Exit()
    {
        Application.Quit();
    }
    

}

