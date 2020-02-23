using UnityEngine;
using UnityEngine.UI;

public class CustomizationMenu : MonoBehaviour {

    public Button[] ColorButtons;
    public GameObject[] CustomizationSubMenusAndButtons;
    public GameObject[] EngineUpgradeButtons;
    public GameObject[] SuspensionUpgradeButtons;
    public GameObject[] TiresUpgradeButtons;
    public GameObject[] BrakesUpgradeButtons;
     Perfermance Perfermance;
    private SaveGame SaveGame;
    public GameObject BodyMenu;
    
    private 
    void Start()
    {
       
        
        SaveGame = GameObject.Find("SaveGame").GetComponent<SaveGame>();
        Perfermance = GameObject.FindGameObjectWithTag("Player").GetComponent<Perfermance>();
        EnableAvailibleUpgradeButtonsForThisCar();
        LoadInteractibilityOfButtons();
       
        
    }
   
    void EnableAvailibleUpgradeButtonsForThisCar()
    {
        for (int i = 0; i < EngineUpgradeButtons.Length; i++)
        {
            if (i <= Perfermance.MaxEngineUpgrade)
            {
                EngineUpgradeButtons[i].SetActive(true);
            }
            else
            {
                EngineUpgradeButtons[i].SetActive(false);
            }
        }

        for (int i = 0; i < SuspensionUpgradeButtons.Length; i++)
        {
            if (i <= Perfermance.MaxSuspensionUpgrade)
            {
                SuspensionUpgradeButtons[i].SetActive(true);
            }
            else
            {
                SuspensionUpgradeButtons[i].SetActive(false);
            }
        }

        for (int i = 0; i < TiresUpgradeButtons.Length; i++)
        {
            if (i <= Perfermance.MaxTiresUpgrade )
            {
                TiresUpgradeButtons[i].SetActive(true);
            }
            else
            {
                TiresUpgradeButtons[i].SetActive(false);
            }
        }

        for (int i = 0; i < BrakesUpgradeButtons.Length; i++)
        {
            if (i <= Perfermance.MaxBrakesUpgrade)
            {
                BrakesUpgradeButtons[i].SetActive(true);
            }
            else
            {
                BrakesUpgradeButtons[i].SetActive(false);
            }
        }
    }
	public void CallSaveCustomizationFromSaveGame()
    {
        SaveGame.SaveCustomization();
        
    }
   public void SwitchToPerfermanceMenu()
    {
        for (int i = 0; i < CustomizationSubMenusAndButtons.Length; i++)
        {
            if (i != 0 )
            {
                CustomizationSubMenusAndButtons[i].SetActive(false);
            }
            else
            {
                CustomizationSubMenusAndButtons[i].SetActive(true);
            }
        }
        LoadInteractibilityOfButtons();
    }
   public void SwitchToVisualsMenu()
    {
        for (int i = 0; i < CustomizationSubMenusAndButtons.Length; i++)
        {
            if (i != 1)
            {
                CustomizationSubMenusAndButtons[i].SetActive(false);
            }
            else
            {
                CustomizationSubMenusAndButtons[i].SetActive(true);
            }
        }
        LoadInteractibilityOfButtons();
    }
    public void SwitchToCustomizationMenu()
    {
        for (int i = 0; i < CustomizationSubMenusAndButtons.Length; i++)
        {
            if (i != 2)
            {
                CustomizationSubMenusAndButtons[i].SetActive(false);
            }
            else
            {
                CustomizationSubMenusAndButtons[i].SetActive(true);
            }
        }
        LoadInteractibilityOfButtons();
    }

    public void ChangeEngineLevel(int a)
    {
        if (a <= Perfermance.MaxEngineUpgrade)
        {
            SaveGame.SelectedCarForPlay.EngineUpgrade = a;
            for (int i = 0; i < EngineUpgradeButtons.Length; i++)
            {
                if (i == a )
                {
                    EngineUpgradeButtons[i].GetComponent<Button>().interactable = false;
                }
                else
                {
                    EngineUpgradeButtons[i].GetComponent<Button>().interactable = true;
                }

            }
        }
        
    }
    public void ChangeSuspensionLevel(int a)
    {
        if (a <= Perfermance.MaxSuspensionUpgrade)
        {
            SaveGame.SelectedCarForPlay.SuspensionUpgrade = a;
            for (int i = 0; i < SuspensionUpgradeButtons.Length; i++)
            {
                if (i == a )
                {
                    SuspensionUpgradeButtons[i].GetComponent<Button>().interactable = false;
                }
                else
                {
                    SuspensionUpgradeButtons[i].GetComponent<Button>().interactable = true;
                }

            }
        }
       
    }
    public void ChangeTiresLevel(int a)
    {
        if (a <= Perfermance.MaxTiresUpgrade)
        {
            SaveGame.SelectedCarForPlay.TiersUpgrade = a;
            for (int i = 0; i < TiresUpgradeButtons.Length; i++)
            {
                if (i == a )
                {
                    TiresUpgradeButtons[i].GetComponent<Button>().interactable = false;
                }
                else
                {
                    TiresUpgradeButtons[i].GetComponent<Button>().interactable = true;
                }

            }
        }
       
    }
    public void ChangeBrakesLevel(int a)
    {
        if (a <= Perfermance.MaxBrakesUpgrade)
        {
            SaveGame.SelectedCarForPlay.BrakeUpgrade = a;
            for (int i = 0; i < BrakesUpgradeButtons.Length; i++)
            {
                if (i == a )
                {
                    BrakesUpgradeButtons[i].GetComponent<Button>().interactable = false;
                }
                else
                {
                    BrakesUpgradeButtons[i].GetComponent<Button>().interactable = true;
                }

            }
        }
       
    }
    public void ChangeColor(int ButtonIndex)
    {

        if (GameObject.Find("BodyMenu"))
        {
            SaveGame.SelectedCarForPlay.BodyColor[0] = ColorButtons[ButtonIndex].GetComponent<Image>().color.r;
            SaveGame.SelectedCarForPlay.BodyColor[1] = ColorButtons[ButtonIndex].GetComponent<Image>().color.g;
            SaveGame.SelectedCarForPlay.BodyColor[2] = ColorButtons[ButtonIndex].GetComponent<Image>().color.b;
            SaveGame.SelectedCarForPlay.BodyColor[3] = ColorButtons[ButtonIndex].GetComponent<Image>().color.a;
        }else if (GameObject.Find("SideViewMirrorsMenu"))
        {
            SaveGame.SelectedCarForPlay.SideViewMirrorsColor[0] = ColorButtons[ButtonIndex].GetComponent<Image>().color.r;
            SaveGame.SelectedCarForPlay.SideViewMirrorsColor[1] = ColorButtons[ButtonIndex].GetComponent<Image>().color.g;
            SaveGame.SelectedCarForPlay.SideViewMirrorsColor[2] = ColorButtons[ButtonIndex].GetComponent<Image>().color.b;
            SaveGame.SelectedCarForPlay.SideViewMirrorsColor[3] = ColorButtons[ButtonIndex].GetComponent<Image>().color.a;
        }
        else if (GameObject.Find("HoodMenu"))
        {
            SaveGame.SelectedCarForPlay.HoodColor[0] = ColorButtons[ButtonIndex].GetComponent<Image>().color.r;
            SaveGame.SelectedCarForPlay.HoodColor[1] = ColorButtons[ButtonIndex].GetComponent<Image>().color.g;
            SaveGame.SelectedCarForPlay.HoodColor[2] = ColorButtons[ButtonIndex].GetComponent<Image>().color.b;
            SaveGame.SelectedCarForPlay.HoodColor[3] = ColorButtons[ButtonIndex].GetComponent<Image>().color.a;
        }
        else if (GameObject.Find("RimsMenu"))
        {
            SaveGame.SelectedCarForPlay.RimsColor[0] = ColorButtons[ButtonIndex].GetComponent<Image>().color.r;
            SaveGame.SelectedCarForPlay.RimsColor[1] = ColorButtons[ButtonIndex].GetComponent<Image>().color.g;
            SaveGame.SelectedCarForPlay.RimsColor[2] = ColorButtons[ButtonIndex].GetComponent<Image>().color.b;
            SaveGame.SelectedCarForPlay.RimsColor[3] = ColorButtons[ButtonIndex].GetComponent<Image>().color.a;
        }


    }
    void LoadInteractibilityOfButtons()
    {
        ChangeEngineLevel(SaveGame.SelectedCarForPlay.EngineUpgrade);
        ChangeSuspensionLevel(SaveGame.SelectedCarForPlay.SuspensionUpgrade);
        ChangeTiresLevel(SaveGame.SelectedCarForPlay.TiersUpgrade);
        ChangeBrakesLevel(SaveGame.SelectedCarForPlay.BrakeUpgrade);

        
    }
}

