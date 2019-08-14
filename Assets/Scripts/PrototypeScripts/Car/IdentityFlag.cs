using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
public class IdentityFlag : MonoBehaviourPunCallbacks {

    
  
    public Behaviour[] AllBehavioursInCarPrefab;
    public GameObject CarCam;
    
    void Start () {
       string  ThisSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
       
        
   
        if (ThisSceneName == "MainMenu")
        {
            CarCam.SetActive(false);
            for (int i = 0; i < AllBehavioursInCarPrefab.Length; i++)
            {
                AllBehavioursInCarPrefab[i].enabled = false;
            }
            this.enabled = false;
        }
       
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
