using UnityEngine;
using UnityEngine.UI;

public class CarSelectionMenuScript : MonoBehaviour
{

    public Transform CarSpawnPoint;
    public GameObject TempCar;
    public RawImage[] ThreeImages;
    public int SelectedCarIndexInsidePurchasedList;//just a temp index for start function
    public int LoadedCar = 0;
    public int PreviousLoadedCar;
    public bool[] IsImageActive = new bool[3]; //where the index of image is from 0 to 2
    public SaveGame SaveGame;

    public PoolSystem PoolSystem;

    private void Start()
    {

        if (PoolSystem == null)
        {
            PoolSystem = GameObject.Find("NetworkManager").GetComponent<PoolSystem>();
        }

        if (SaveGame == null)
        {
            SaveGame = GameObject.Find("NetworkManager").GetComponent<SaveGame>();
        }
    }
    void Update()
    {
        SetCarAsSelected();

    }
    public void NextCar()
    {

        if ((LoadedCar + 1) < SaveGame.PurchasedCarsList.Count)
        {
            PreviousLoadedCar = LoadedCar;
            DestroyTheTempCar();
            LoadCar(SaveGame.PurchasedCarsList[LoadedCar + 1].CarName, LoadedCar + 1);
            if (LoadedCar == 0 && SaveGame.PurchasedCarsList.Count >= 2)
            {
                ThreeImages[0].enabled = false;
                IsImageActive[0] = false;
                ThreeImages[1].enabled = true;
                IsImageActive[1] = true;
                ThreeImages[2].enabled = true;
                IsImageActive[2] = true;
            }
            else
            if (LoadedCar == 0 && SaveGame.PurchasedCarsList.Count < 2)
            {
                ThreeImages[0].enabled = false;
                IsImageActive[0] = false;
                ThreeImages[1].enabled = true;
                IsImageActive[1] = true;
                ThreeImages[2].enabled = false;
                IsImageActive[2] = false;
            }
            else
            if (LoadedCar == SaveGame.PurchasedCarsList.Count - 1)
            {
                ThreeImages[0].enabled = true;
                IsImageActive[0] = true;
                ThreeImages[1].enabled = true;
                IsImageActive[1] = true;
                ThreeImages[2].enabled = false;
                IsImageActive[2] = false;
            }
            else if (LoadedCar >= 1)
            {
                ThreeImages[0].enabled = true;
                IsImageActive[0] = true;
                ThreeImages[1].enabled = true;
                IsImageActive[1] = true;
                ThreeImages[2].enabled = true;
                IsImageActive[2] = true;
            }
            ChangeImages();
        }

    }

    public void PreviousCar()
    {
        if ((LoadedCar - 1) >= 0)
        {
            PreviousLoadedCar = LoadedCar;
            DestroyTheTempCar();
            LoadCar(SaveGame.PurchasedCarsList[LoadedCar - 1].CarName, LoadedCar - 1);
            if (LoadedCar == 0 && SaveGame.PurchasedCarsList.Count >= 2)
            {
                ThreeImages[0].enabled = false;
                IsImageActive[0] = false;
                ThreeImages[1].enabled = true;
                IsImageActive[1] = true;
                ThreeImages[2].enabled = true;
                IsImageActive[2] = true;
            }
            else
            if (LoadedCar == 0 && SaveGame.PurchasedCarsList.Count < 2)
            {
                ThreeImages[0].enabled = false;
                IsImageActive[0] = false;
                ThreeImages[1].enabled = true;
                IsImageActive[1] = true;
                ThreeImages[2].enabled = false;
                IsImageActive[2] = false;
            }
            else
            if (LoadedCar == SaveGame.PurchasedCarsList.Count - 1)
            {
                ThreeImages[0].enabled = true;
                IsImageActive[0] = true;
                ThreeImages[1].enabled = true;
                IsImageActive[1] = true;
                ThreeImages[2].enabled = false;
                IsImageActive[2] = false;
            }
            else if (LoadedCar >= 1)
            {
                ThreeImages[0].enabled = true;
                IsImageActive[0] = true;
                ThreeImages[1].enabled = true;
                IsImageActive[1] = true;
                ThreeImages[2].enabled = true;
                IsImageActive[2] = true;
            }
            ChangeImages();
        }
    }


    public void LoadCar(string CarName, int index)
    {
        LoadedCar = index;

       
        TempCar = PoolSystem.GetFromPool(CarName);
        float AppropriateSpawnHeightForThisCar = TempCar.GetComponentInChildren<WheelCollider>().radius * 2f + Mathf.Abs(TempCar.GetComponentInChildren<WheelCollider>().gameObject.transform.localPosition.y) * 2f + TempCar.GetComponentInChildren<WheelCollider>().suspensionDistance * 2f * (1f - TempCar.GetComponentInChildren<WheelCollider>().suspensionSpring.targetPosition);
       
        TempCar.transform.position = new Vector3(CarSpawnPoint.position.x, AppropriateSpawnHeightForThisCar, CarSpawnPoint.position.z);
        TempCar.transform.rotation = CarSpawnPoint.rotation;
        TempCar.transform.name = TempCar.transform.name.Replace("(Clone)", "");
    }
    public void LoadCar()
    {
        SaveGame = GameObject.Find("SaveGame").GetComponent<SaveGame>();//dont remove this it resolve issues when switching back from play scene to main menu
        if (SaveGame.PurchasedCarsList.Count > 0)
        {

            DestroyTheTempCar();

            
            TempCar = PoolSystem.GetFromPool(SaveGame.SelectedCarForPlay.CarName);
            float AppropriateSpawnHeightForThisCar = TempCar.GetComponentInChildren<WheelCollider>().radius * 2f + Mathf.Abs(TempCar.GetComponentInChildren<WheelCollider>().gameObject.transform.localPosition.y) * 2f + TempCar.GetComponentInChildren<WheelCollider>().suspensionDistance * 2f * (1f - TempCar.GetComponentInChildren<WheelCollider>().suspensionSpring.targetPosition);
           
            TempCar.transform.position = new Vector3(CarSpawnPoint.position.x, AppropriateSpawnHeightForThisCar, CarSpawnPoint.position.z);
            TempCar.transform.rotation = CarSpawnPoint.rotation;
            TempCar.transform.name = TempCar.transform.name.Replace("(Clone)", "");
            FindTheIndexOfSelectedCarInPurchasedList();//i did not remove the else and condition for this function
            LoadedCar = SelectedCarIndexInsidePurchasedList;

            if (LoadedCar == 0 && SaveGame.PurchasedCarsList.Count >= 2)
            {
                ThreeImages[0].enabled = false;
                IsImageActive[0] = false;
                ThreeImages[1].enabled = true;
                IsImageActive[1] = true;
                ThreeImages[2].enabled = true;
                IsImageActive[2] = true;
            }
            else
            if (LoadedCar == 0 && SaveGame.PurchasedCarsList.Count < 2)
            {
                ThreeImages[0].enabled = false;
                IsImageActive[0] = false;
                ThreeImages[1].enabled = true;
                IsImageActive[1] = true;
                ThreeImages[2].enabled = false;
                IsImageActive[2] = false;
            }
            else
            if (LoadedCar == SaveGame.PurchasedCarsList.Count - 1)
            {
                ThreeImages[0].enabled = true;
                IsImageActive[0] = true;
                ThreeImages[1].enabled = true;
                IsImageActive[1] = true;
                ThreeImages[2].enabled = false;
                IsImageActive[2] = false;
            }
            else if (LoadedCar >= 1)
            {
                ThreeImages[0].enabled = true;
                IsImageActive[0] = true;
                ThreeImages[1].enabled = true;
                IsImageActive[1] = true;
                ThreeImages[2].enabled = true;
                IsImageActive[2] = true;
            }


            ChangeImages();

        }
        else
        {
           
            DestroyTheTempCar();
            TempCar = PoolSystem.GetFromPool(0);
            float AppropriateSpawnHeightForThisCar = TempCar.GetComponentInChildren<WheelCollider>().radius * 2f + Mathf.Abs(TempCar.GetComponentInChildren<WheelCollider>().gameObject.transform.localPosition.y) * 2f + TempCar.GetComponentInChildren<WheelCollider>().suspensionDistance * 2f * (1f - TempCar.GetComponentInChildren<WheelCollider>().suspensionSpring.targetPosition);
            TempCar.transform.position = new Vector3(CarSpawnPoint.position.x, AppropriateSpawnHeightForThisCar, CarSpawnPoint.position.z);
            TempCar.transform.rotation = CarSpawnPoint.rotation;

            ThreeImages[0].enabled = false;
            IsImageActive[0] = false;
            ThreeImages[1].enabled = false;
            IsImageActive[1] = false;
            ThreeImages[2].enabled = false;
            IsImageActive[2] = false;
        }


    }
    public void SetCarAsSelectedForPlay()
    {
        SaveGame.SelectedCarForPlay = SaveGame.PurchasedCarsList[LoadedCar];

        SaveGame.SavePlayer();
    }
    public void SetCarAsSelected()
    {
        SaveGame.SelectedCarForPlay = SaveGame.PurchasedCarsList[LoadedCar];
    }
    public void ChangeImages()
    {
        if (IsImageActive[0] && IsImageActive[1] && IsImageActive[2] == true)
        {
            ThreeImages[0].texture = Resources.Load(SaveGame.PurchasedCarsList[LoadedCar - 1].CarName + "NameImage") as Texture;
            ThreeImages[1].texture = Resources.Load(SaveGame.PurchasedCarsList[LoadedCar].CarName + "NameImage") as Texture;
            ThreeImages[2].texture = Resources.Load(SaveGame.PurchasedCarsList[LoadedCar + 1].CarName + "NameImage") as Texture;
        }
        if (IsImageActive[0] && IsImageActive[1] == true)
        {
            ThreeImages[0].texture = Resources.Load(SaveGame.PurchasedCarsList[LoadedCar - 1].CarName + "NameImage") as Texture;
            ThreeImages[1].texture = Resources.Load(SaveGame.PurchasedCarsList[LoadedCar].CarName + "NameImage") as Texture;
        }
        if (IsImageActive[1] && IsImageActive[2] == true)
        {
            ThreeImages[1].texture = Resources.Load(SaveGame.PurchasedCarsList[LoadedCar].CarName + "NameImage") as Texture;
            ThreeImages[2].texture = Resources.Load(SaveGame.PurchasedCarsList[LoadedCar + 1].CarName + "NameImage") as Texture;
        }
        if (IsImageActive[1] == true)
        {
            ThreeImages[1].texture = Resources.Load(SaveGame.PurchasedCarsList[LoadedCar].CarName + "NameImage") as Texture;

        }
    }
    public void Sell()
    {
        SaveGame.SaveSellCar(SaveGame.PurchasedCarsList[LoadedCar]);
        DestroyTheTempCar();
        LoadCar();

    }
    public void FindTheIndexOfSelectedCarInPurchasedList()
    {
        for (int i = 0; i < SaveGame.PurchasedCarsList.Count; i++)
        {
            if (SaveGame.PurchasedCarsList[i] == SaveGame.SelectedCarForPlay)
            {

                SelectedCarIndexInsidePurchasedList = i;
                continue;

            }

        }

    }
   
    public void DestroyTheTempCar()
    {
        
        PoolSystem.ReturnToPool(TempCar);
    }
}



