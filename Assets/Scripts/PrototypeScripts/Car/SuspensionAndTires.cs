/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspensionAndTires : MonoBehaviour {
    // public SaveGame SaveGame;
   // private Motor Motor;
   
    [Range(0.1f,100)]
    public float FrontWheelsMass;
    [Range(0.1f, 100)]
    public float RearWheelsMass;
    [Space]

    [Range(0.1f, 10)]
    public float FrontWheelsRadius;
    [Range(0.1f, 10)]
    public float RearWheelsRadius;
    [Space]

    [Range(0.1f, 1000)]
    public float FrontWheelsDampingRate;
    [Range(0.1f, 1000)]
    public float RearWheelsDampingRate;

    [Space]
    [Range(0.1f, 5)]
    public float FrontWheelsSuspensionDistance;
    [Range(0.1f, 5)]
    public float RearWheelsSuspensionDistance;

    [Space]
    [Space]
    [Header("FRICTIONS")]
    public float FFORWARDExtremumSlip;
    public float RFORWARDExtremumSlip;
    [Space]
    public float FFORWARDExtremumValue;
    public float RFORWARDExtremumValue;
   
    [Space]
    public float FFORWARDAsymptoteSlip;
    public float RFORWARDAsymptoteSlip;
    [Space]
    public float FFORWARDAsymptoteValue;
    public float RFORWARDAsymptoteValue;
    [Space]
    public float FFORWARDExtremumStiffness;
    public float RFORWARDExtremumStifness;

    [Space]
    [Space]
    public float FSIDEWAYSExtremumSlip;
    public float RSIDEWAYSExtremumSlip;
    [Space]
    public float FSIDEWAYSExtremumValue;
    public float RSIDEWAYSExtremumValue;
    [Space]
    public float FSIDEWAYSAsymptoteSlip;
    public float RSIDEWAYSAsymptoteSlip;
    [Space]
    public float FSIDEWAYSAsymptoteValue;
    public float RSIDEWAYSAsymptoteValue;
    [Space]
    public float FSIDEWAYSExtremumStiffness;
    public float RSIDEWAYSExtremumStifness;


    [Space]
    [Space]

    public WheelFrictionCurve FrontWheelsForwardFriction;
    public WheelFrictionCurve RearWheelsForwardFriction;

    public WheelFrictionCurve FrontWheelsSideWaysFriction;
    public WheelFrictionCurve RearWheelsSideWaysdFriction;

    
    void Start()
    {
       // Motor = GetComponent<Motor>();
        FrontWheelsForwardFriction.extremumSlip = FFORWARDExtremumSlip;
        FrontWheelsForwardFriction.extremumValue = FFORWARDExtremumValue;
        FrontWheelsForwardFriction.asymptoteSlip = FFORWARDAsymptoteSlip;
        FrontWheelsForwardFriction.asymptoteValue = FFORWARDAsymptoteValue;
        FrontWheelsForwardFriction.stiffness = FFORWARDExtremumStiffness;

        RearWheelsForwardFriction.extremumSlip = RFORWARDExtremumSlip;
        RearWheelsForwardFriction.extremumValue = RFORWARDExtremumValue;
        RearWheelsForwardFriction.asymptoteSlip = RFORWARDAsymptoteSlip;
        RearWheelsForwardFriction.asymptoteValue = RFORWARDAsymptoteValue;
        RearWheelsForwardFriction.stiffness = RFORWARDExtremumStifness;


       FrontWheelsSideWaysFriction .extremumSlip = FSIDEWAYSExtremumSlip;
        FrontWheelsSideWaysFriction.extremumValue = FSIDEWAYSExtremumValue;
        FrontWheelsSideWaysFriction.asymptoteSlip = FSIDEWAYSAsymptoteSlip;
        FrontWheelsSideWaysFriction.asymptoteValue = FSIDEWAYSAsymptoteValue;
        FrontWheelsSideWaysFriction.stiffness = FSIDEWAYSExtremumStiffness;

        RearWheelsSideWaysdFriction.extremumSlip = RSIDEWAYSExtremumSlip;
        RearWheelsSideWaysdFriction.extremumValue = RSIDEWAYSExtremumValue;
        RearWheelsSideWaysdFriction.asymptoteSlip = RSIDEWAYSAsymptoteSlip;
        RearWheelsSideWaysdFriction.asymptoteValue = RSIDEWAYSAsymptoteValue;
        RearWheelsSideWaysdFriction.stiffness = RSIDEWAYSExtremumStifness;

    //    Motor.ApplyChanges();
    }

    
}
*/