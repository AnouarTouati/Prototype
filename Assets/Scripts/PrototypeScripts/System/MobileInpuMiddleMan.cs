using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInpuMiddleMan : MonoBehaviour {

    //I MADE THIS SCRIPT BECAUSE BUTTONS NEED GAME OBJECT TO WORK NAD THE CAR(MOTOR SCRIPT) IS NOT AVAILBLE ALL TIME
    private Motor Motor;
   void Update()
    {
        if (Motor == null && GameObject.FindGameObjectWithTag("Player"))
        {
            Motor = GameObject.FindGameObjectWithTag("Player").GetComponent<Motor>();
        }
    }
    public void AccelerationButtonPressed()
    {
        
            Motor.AccelerationButtonPressed = true;
        
    }
    public void AccelerationButtonUnPressed()
    {
       
            Motor.AccelerationButtonPressed = false;
        
    }
    public void BrakingButtonPressed()
    {
        
            Motor.BrakingButtonPressed = true;
       
    }
    public void BrakingButtonUnPressed()
    {
        
            Motor.BrakingButtonPressed = false;
       
    }
}
