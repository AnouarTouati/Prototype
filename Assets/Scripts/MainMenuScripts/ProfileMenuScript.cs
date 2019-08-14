using UnityEngine;
using UnityEngine.UI;

public class ProfileMenuScript : MonoBehaviour {

    public InputField InputField;

    public string temp;
    public SaveGame SaveGame;
    public MainMenuScript MainMenuScript;
    public GameObject[] ChildsObjects;
    public Button CreateProfileButton;
    public Button BackButton;
    public Button[] DeleteButtons;
    void Start() {



        LoadAndFillTheButtons();


    }
    // private void OnEnable()
    //{
    //   LoadAndFillTheButtons();
    //}
    // Update is called once per frame
    void Update() {


    }
   
    public void FillTheButtons()
    {
        if (SaveGame.NamesOfAvailibleProfiles.Count >= 2)
        {
            CreateProfileButton.interactable = false;
        }
        else
        {
            CreateProfileButton.interactable = true;
        }
        if (SaveGame.ProfileName == "")//we do this to check if it is the first time we enter profile menu since the game load
        {
            BackButton.gameObject.SetActive(false);
        }
        else
        {
            if (BackButton.gameObject.activeSelf == false)
            {
                BackButton.gameObject.SetActive(true);
            }
        }
        for (int i = 0; i < ChildsObjects.Length; i++)
        {
            if (ChildsObjects[i].activeSelf)
            {
                ChildsObjects[i].GetComponentInChildren<Text>().text = "Profile " + (i + 1);
            }
        }

            for (int i = 0; i < SaveGame.NamesOfAvailibleProfiles.Count; i++)
            {

            DeleteButtons[i].GetComponent<Button>().interactable = true;

            ChildsObjects[i].GetComponentInChildren<Text>().text = SaveGame.NamesOfAvailibleProfiles[i];
                if (SaveGame.ProfileName == SaveGame.NamesOfAvailibleProfiles[i])
                {
                    ChildsObjects[i].GetComponent<Button>().interactable = false;

                }
                else
                {
                    ChildsObjects[i].GetComponent<Button>().interactable = true;
                }

            }
        for (int i = SaveGame.NamesOfAvailibleProfiles.Count; i < ChildsObjects.Length; i++)
        {
            ChildsObjects[i].GetComponent<Button>().interactable = false;
            DeleteButtons[i].GetComponent<Button>().interactable = false;

        }


    }
        void LoadAndFillTheButtons()
        {
            SaveGame = GameObject.Find("SaveGame").GetComponent<SaveGame>();

            if (SaveGame.PlayerAlreadyLoadedAProfileNoNeedToReload)
            {
                MainMenuScript.SwitchToMainMenu();
            }
            else
            {
            
                SaveGame.LoadTheListOfAvailbleProfiles();
                FillTheButtons();

            }
        }
        public void CallTheRegisterNewPlayerProfile(string a)
        {

            temp = InputField.text;
            SaveGame.RegisterNewPlayerProfile(a);
            InputField.gameObject.SetActive(false);
            MainMenuScript.SwitchToMainMenu();

        }

        public void CallTheLoadPlayer(int index)
        {
            SaveGame.LoadPlayer(SaveGame.NamesOfAvailibleProfiles[index]);
            MainMenuScript.SwitchToMainMenu();
        }
    public void CallTheLoadPlayerBackFromCustomizations()
    {
        SaveGame.LoadPlayer(SaveGame.ProfileName);//reloads the current profile
    }
        public void SwitchToRegistrationMenu()
        {
            InputField.gameObject.SetActive(true);

        }
        public void CallDeleteProfile(int Index)
        {
            SaveGame.DeletePlayerProfile(Index);
            LoadAndFillTheButtons();
            Debug.Log("Am I Called");

        }
    }
