using UnityEngine;
using UnityEngine.UI;

public class BuyCarMenuScript : MonoBehaviour
{

    public Transform CarSpawnPoint;
    private GameObject TempCar;
    public RawImage[] ThreeImages;
    public string SelectedCarNameToShowBuyInfo;
    public int LoadedCar = 0;
    public int PreviousLoadedCar;
    public bool[] IsImageActive = new bool[3]; //where the index of image is from 0 to 2
   
    //public AllCars AllCars;
    public CarSelectionMenuScript CarSelectionMenuScript;
    public SaveGame SaveGame;
  
    public PoolSystem PoolSystem;
   
    void Start()
    {
    
        if (PoolSystem == null)
        {
            PoolSystem = GameObject.Find("PoolSystem").GetComponent<PoolSystem>();
        }


        if (SaveGame == null)
        {
            SaveGame = GameObject.Find("SaveGame").GetComponent<SaveGame>();
        }
        if (SaveGame.AllCarsPrefabs != null)
        {
            LoadCar();
            SelectedCarNameToShowBuyInfo = SaveGame.AllCarsPrefabs[0].name;
        }
        ThreeImages[0].enabled = false;
        ThreeImages[1].texture = Resources.Load(SaveGame.AllCarsPrefabs[0].name + "NameImage") as Texture;
        ThreeImages[2].texture = Resources.Load(SaveGame.AllCarsPrefabs[1].name + "NameImage") as Texture;
        IsImageActive[0] = false;
        IsImageActive[1] = true;
        IsImageActive[2] = true;

        
    }


    
    public void NextCar()
    {

        if ((LoadedCar + 1) < SaveGame.AllCarsPrefabs.Length)
        {
            PreviousLoadedCar = LoadedCar;
            DestroyTheTempCar();
            LoadCar(SaveGame.AllCarsPrefabs[LoadedCar + 1].name, LoadedCar + 1);
            if (LoadedCar == 1)
            {
                ThreeImages[0].enabled = true;
                IsImageActive[0] = true;
            }
            if (LoadedCar == SaveGame.AllCarsPrefabs.Length - 1)
            {
                ThreeImages[2].enabled = false;
                IsImageActive[2] = false;
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
            LoadCar(SaveGame.AllCarsPrefabs[LoadedCar - 1].name, LoadedCar - 1);
            if (LoadedCar == 0)
            {
                ThreeImages[0].enabled = false;
                IsImageActive[0] = false;
            }
            if (LoadedCar == (SaveGame.AllCarsPrefabs.Length - 2))
            {
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
       float AppropriateSpawnHeightForThisCar = TempCar.GetComponentInChildren<WheelCollider>().radius * 2f + Mathf.Abs(TempCar.GetComponentInChildren<WheelCollider>().gameObject.transform.localPosition.y) * 2f + TempCar.GetComponentInChildren<WheelCollider>().suspensionDistance * 2f *(1f - TempCar.GetComponentInChildren<WheelCollider>().suspensionSpring.targetPosition);
      
        TempCar.transform.position = new Vector3(CarSpawnPoint.position.x, AppropriateSpawnHeightForThisCar, CarSpawnPoint.position.z);
        TempCar.transform.rotation = CarSpawnPoint.rotation;
        TempCar.transform.name = TempCar.transform.name.Replace("(Clone)", "");
    }
    public void LoadCar()
    {
      
           DestroyTheTempCar();
       
   
        TempCar = PoolSystem.GetFromPool(0);
       float AppropriateSpawnHeightForThisCar = TempCar.GetComponentInChildren<WheelCollider>().radius * 2f + Mathf.Abs(TempCar.GetComponentInChildren<WheelCollider>().gameObject.transform.localPosition.y) * 2f + TempCar.GetComponentInChildren<WheelCollider>().suspensionDistance * 2f * (1f-TempCar.GetComponentInChildren<WheelCollider>().suspensionSpring.targetPosition);
       
        TempCar.transform.position = new Vector3(CarSpawnPoint.position.x, AppropriateSpawnHeightForThisCar, CarSpawnPoint.position.z);
        TempCar.transform.rotation = CarSpawnPoint.rotation;
        TempCar.transform.name = TempCar.transform.name.Replace("(Clone)", "");

    }
    public void SetCarAsSelectedForBuyInfo()
    {
        SelectedCarNameToShowBuyInfo = SaveGame.AllCarsPrefabs[LoadedCar].name;
    }
    public void ChangeImages()
    {
        if (IsImageActive[0] && IsImageActive[1] && IsImageActive[2] == true)
        {
            ThreeImages[0].texture = Resources.Load(SaveGame.AllCarsPrefabs[LoadedCar - 1].name + "NameImage") as Texture;
            ThreeImages[1].texture = Resources.Load(SaveGame.AllCarsPrefabs[LoadedCar].name + "NameImage") as Texture;
            ThreeImages[2].texture = Resources.Load(SaveGame.AllCarsPrefabs[LoadedCar + 1].name + "NameImage") as Texture;
        }
        if (IsImageActive[0] && IsImageActive[1] == true)
        {
            ThreeImages[0].texture = Resources.Load(SaveGame.AllCarsPrefabs[LoadedCar - 1].name + "NameImage") as Texture;
            ThreeImages[1].texture = Resources.Load(SaveGame.AllCarsPrefabs[LoadedCar].name + "NameImage") as Texture;
        }
        if (IsImageActive[1] && IsImageActive[2] == true)
        {
            ThreeImages[1].texture = Resources.Load(SaveGame.AllCarsPrefabs[LoadedCar].name + "NameImage") as Texture;
            ThreeImages[2].texture = Resources.Load(SaveGame.AllCarsPrefabs[LoadedCar + 1].name + "NameImage") as Texture;
        }
    }
    public void Buy()
    {
        SaveGame.SavePurchaseCar(SelectedCarNameToShowBuyInfo);
       
    }
    public void DestroyTheTempCar()
    {
      
            PoolSystem.ReturnToPool(TempCar);
        
    }
   
}



