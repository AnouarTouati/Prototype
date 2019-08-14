using System.Collections.Generic;
using UnityEngine;


public class PoolSystem : MonoBehaviour
{

    List<Queue<GameObject>> SingleCarPoolList = new List<Queue<GameObject>>();//each car has it own pool
    public SaveGame SaveGame;
    void Start()
    {
        CreateThePool();
    }
   void CreateThePool()
    {
        //we did create the pool and not grow the pool because in our case all itme of the pool will be destroyed when load a play scene
        for (int i = 0; i <SaveGame.AllCarsPrefabs.Length; i++)
        {
            Queue<GameObject> CarPool = new Queue<GameObject>();
            for (int l = 0; l < 1; l++)
            {
                GameObject go = Instantiate(SaveGame.AllCarsPrefabs[i]);
                go.GetComponent<CarVisualSync>().InstantiedFromSaveGameForInitializingOnly = false;
                go.SetActive(false);
                CarPool.Enqueue(go);
            }


            SingleCarPoolList.Add(CarPool);
        }
    }
  public   void ReturnToPool(GameObject CarToBeReturnedToPool)
    {
        if (CarToBeReturnedToPool != null)
        {
            CarToBeReturnedToPool.SetActive(false);
            SingleCarPoolList[GetIndexOfTheCarInAllCars(CarToBeReturnedToPool.name)].Enqueue(CarToBeReturnedToPool);
        }
       

    }
   int GetIndexOfTheCarInAllCars(string CarName)
    {
        int index = -1;
        for (int i = 0; i <SaveGame.AllCarsPrefabs.Length; i++)
        {
            if (CarName ==SaveGame.AllCarsPrefabs[i].name)
            {
                index = i;
                break;
            }
        }
        return index;
    }
  public GameObject GetFromPool(int CarIndexInAllCars)
    {
        if (SingleCarPoolList.Count==0)
        {
            CreateThePool();//we did create the pool and not grow the pool because in our case all itme of the pool will be destroyed when load a play scene
        }
        GameObject go=  SingleCarPoolList[CarIndexInAllCars].Dequeue();
        go.SetActive(true);
        return go;
    }
    public GameObject GetFromPool(string CarName)
    {
        if (SingleCarPoolList.Count==0)
        {
            CreateThePool();//we did create the pool and not grow the pool because in our case all itme of the pool will be destroyed when load a play scene
        }
        GameObject go = SingleCarPoolList[GetIndexOfTheCarInAllCars(CarName)].Dequeue();
        go.SetActive(true);
        return go;
    }
}
