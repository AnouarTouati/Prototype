using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.Collections;

public class Motor : MonoBehaviourPunCallbacks
{
    float percentageToUseFromTorqueAccelerating = 0f;
    float percentageToUseFromTorqueBraking = 0f;
    public WheelCollider[] WheelColliders;
    public GameObject[] WheelMeshes;
    [Tooltip("The vehicle's speed when the physics engine can use different amount of sub-steps (in m/s).")]
    public float criticalSpeed = 5f;
    [Tooltip("Simulation sub-steps when the speed is above critical.")]
    public int stepsBelow = 5;
    [Tooltip("Simulation sub-steps when the speed is below critical.")]
    public int stepsAbove = 1;

    public float Percentage;

    private float WheelRPM;
    public float RPM;
    [SerializeField]
    private float Torque;
    [SerializeField]
    private float AppliedTorque;
    [SerializeField]
    private float EngineTorque;
    public int Gears = 1;

    private float WheelRPMtoEngineRPMmultiplier;

    
    public int BeginingOfLastLookAheadAnglePositionInWayPoints = -1;
    /* public bool IsAccelerating = false;//leave public for tail lights
     public bool IsBraking = false;*/
    public bool AccelerationButtonPressed = false;
    public bool BrakingButtonPressed = false;
    private GraphOverlay graphOverlay;



    private Perfermance Perfermance;
  //  private SuspensionAndTires SuspensionAndTires;
    public WheelFrictionCurve weeelcurve = new WheelFrictionCurve();

    private GamePlayScore GamePlayScore;
    private RaceSystem RaceSystem;

    public int CurrentWayPointIndex = 0;
    public GameObject MiniMapIcon;
    public GameObject MiniMapIconOther;


    [Header("AI Section")]
    public float SensorsLength;
    public float SideSensorLength;
    public Transform FrontSensorStartTransform;
    public Transform FrontRightSensorStartTransform;
    public Transform FrontLeftSensorStartTransform;
    public Transform FrontRightAngleSensorStartTransform;
    public Transform FrontLeftAngleSensorStartTransform;
    public Transform SideRightSensorStartTransform;
    public Transform SideLeftSensorStartTransform;
    public bool AIDriving = false;
    public bool Avoiding = false;
    public bool AIAccelerates = true;
    public bool AIBrakes = false;
     bool AICarPoistionIsBeingReset = false;
    public float AvoidMultiplier = 0;
    public float AIAccelerationValue = 1;//between 0 and 1
    public float LookAheadAngle = 0;
    public float Angle;
    public int LastLookAheadAnglePositionInWayPoints = -7;

    public float CurveRadius;
    public Vector3 CentralPoint;
    public float Speed;
    public float LimitSpeedForCornerInKM;
    public float LimitSpeedForCorner;

    public float EntryPointCriteria = 4f;
    public float ExitPointCriteria = 1.5f;


    public float MaxSpeed;//is set from inspector but should be calculated from MAXRPM and wheel radius and torque


    [Header("OnlineControl")]
  //  [SyncVar]
    public bool IsAcceleratingCommandBool;
 //   [SyncVar]
    public bool IsBrakingCommanBool;
  //  [SyncVar]
    public float SteeringCommandFloat;

    void Start()
    {
       
        GamePlayScore = GetComponent<GamePlayScore>();
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            this.enabled = false;
        }
        else
        {
            graphOverlay = GameObject.Find("GraphicOverlay").GetComponent<GraphOverlay>();
            if (photonView.IsMine || AIDriving)
            {

                Perfermance = GetComponent<Perfermance>();
              //  SuspensionAndTires = GetComponent<SuspensionAndTires>();
                RaceSystem = GameObject.Find("RaceSystem").GetComponent<RaceSystem>();
                GamePlayScore = GetComponent<GamePlayScore>();
                if (!AIDriving)
                {
                    MiniMapIcon.SetActive(true);
                    MiniMapIconOther.SetActive(false);
                   

                }
                else
                {

                    MiniMapIcon.SetActive(false);
                    MiniMapIconOther.SetActive(true);
                    
                    GetComponentInParent<Transform>().transform.tag = "AIDriving";
                }

            }
            else
            {


                GetComponentInParent<Transform>().transform.tag = "OtherPlayer";
                MiniMapIcon.SetActive(false);
                MiniMapIconOther.SetActive(true);
                
                for (int i = 0; i < WheelColliders.Length; i++)
                {
                 //   WheelColliders[i].enabled = false;//we turn off wheelcolliders so in online the Car will not JUMP AND FLY
                  //  WheelMeshes[i].GetComponent<MeshCollider>().enabled = true;//WE TURN ON Wheel Meshs so the car doesnt sink into the ground
                }

                this.enabled = false;
                //change the layer to OtherCars
            }
        }




        //  ApplyChanges();

    }


    void  Update()
    {
        WheelColliders[0].ConfigureVehicleSubsteps(criticalSpeed, stepsBelow, stepsAbove);

        if (!AIDriving)
        {
            if (SceneManager.GetActiveScene().name != "MainMenu" && photonView.IsMine)
            {

               
                
                    if (Input.GetKeyDown(KeyCode.R))
                    {
                    ResetCarPosition();
                    }

                    UpdateCalculations();
                



            }
        }
        else if (AIDriving)
        {
            if (RaceSystem.RaceStarted && !GamePlayScore.FinishedTheRace &&  !AICarPoistionIsBeingReset && !WheelColliders[0].isGrounded && !WheelColliders[1].isGrounded && !WheelColliders[2].isGrounded && !WheelColliders[3].isGrounded)
            {
                AICarPoistionIsBeingReset = true;
            //    Debug.Log("reset car");
                ResetCarPosition();
                StartCoroutine("AICheckIfCarIsReset");
            }
            
        
      
            UpdateCalculationsForAI();
        }



    }
   

    
    void UpdateCalculationsForAI()
    {
    
        Sensor();
        AIAccelerationBrakingSystem();
        if (RaceSystem == null)
            RaceSystem = GameObject.Find("RaceSystem").GetComponent<RaceSystem>();
        if (RaceSystem != null)
        {
            SharedCodeBetweenPlayerAndAI();


            if (GamePlayScore.FinishedTheRace == false && RaceSystem.RaceStarted == true)
            {


                if (AIAccelerates)
                {

                    if (WheelColliders[0].rpm < 0)
                    {
                        IsAcceleratingCommandBool = false;
                        IsBrakingCommanBool = true;
                        Brakes();

                    }
                    else
                    {
                        IsAcceleratingCommandBool = true;
                        IsBrakingCommanBool = false;

                        if (Gears < 1)
                        {
                            SwitchGears(1);
                        }

                        Accelerates();


                    }
                }

                else if (AIBrakes)
                {
                    if (WheelColliders[0].rpm > 0)
                    {
                        IsAcceleratingCommandBool = false;
                        IsBrakingCommanBool = true;
                        Brakes();
                    }
                    else
                    {
                        if (Gears != -1)
                        {
                            SwitchGears(-1);
                        }

                        Accelerates(/*AIAccelerationValue*/);
                        IsAcceleratingCommandBool = true;
                        IsBrakingCommanBool = false;
                    }
                }
                else
                {
                    ApplyTorque(0);
                    Brake(0);
                }

                Steer();
            }
            else
            {
                Brakes();
            }

            RotateTheWheelMeshes();
        }
    }
    void AIAccelerationBrakingSystem()
    {

        if (CurrentWayPointIndex + 10 < RaceSystem.WayPoints.Length)
        {


            Vector3 CurveEntryPoint = new Vector3(0, 0, 0);
            Vector3 CurveEntryPointVector = new Vector3(0, 0, 0);

            Vector3 CurveExitPoint = new Vector3(0, 0, 0);
            Vector3 CurveExitPointVector = new Vector3(0, 0, 0);
            bool FoundACurve = false;
            if (CurrentWayPointIndex > LastLookAheadAnglePositionInWayPoints)
            {
               int HowManyCheckPointToCheckNext=15;
                int LimitAccessIndex;
                if(CurrentWayPointIndex+HowManyCheckPointToCheckNext <RaceSystem.WayPoints.Length - 2){
                    LimitAccessIndex=CurrentWayPointIndex+HowManyCheckPointToCheckNext;
                }else{ LimitAccessIndex= RaceSystem.WayPoints.Length - 2;}
                for (int i = CurrentWayPointIndex; i < LimitAccessIndex ; i++)
                {
                    Vector3 CheckVector = (RaceSystem.WayPoints[i + 1].position - RaceSystem.WayPoints[i].position) - (RaceSystem.WayPoints[i + 2].position - RaceSystem.WayPoints[i + 1].position);

                    if (CurveEntryPoint == Vector3.zero && CurveExitPoint == Vector3.zero && (Mathf.Abs(CheckVector.x) > EntryPointCriteria || Mathf.Abs(CheckVector.z) > EntryPointCriteria))
                    {

                        BeginingOfLastLookAheadAnglePositionInWayPoints = i;
                        CurveEntryPoint = RaceSystem.WayPoints[i].position;
                        CurveEntryPointVector = RaceSystem.WayPoints[i + 1].position - RaceSystem.WayPoints[i].position;
                    }
                    else if (CurveEntryPoint != Vector3.zero && Mathf.Abs(CheckVector.x) < ExitPointCriteria && Mathf.Abs(CheckVector.z) < ExitPointCriteria)
                    {

                        CurveExitPoint = RaceSystem.WayPoints[i].position;
                        CurveExitPointVector = RaceSystem.WayPoints[i + 1].position - RaceSystem.WayPoints[i].position;
                        LastLookAheadAnglePositionInWayPoints = i + 2;
                        FoundACurve = true;
                       Debug.DrawRay(CurveEntryPoint, CurveEntryPointVector * 10, Color.red, 50, false);

                        Debug.DrawRay(CurveExitPoint, CurveExitPointVector * 10, Color.green, 50, false);
                        break;
                    }


                }
                if (FoundACurve)
                {


                    float CurveEntryPointInverseSlope = -CurveEntryPointVector.z / CurveEntryPointVector.x;
                    float BofCurveEntryPointLineEquation = CurveEntryPoint.z - CurveEntryPointInverseSlope * CurveEntryPoint.x;


                    float CurveExitPointInverseSlope = -CurveExitPointVector.z / CurveExitPointVector.x;
                    float BofCurveExitPointLineEquation = CurveExitPoint.z - CurveExitPointInverseSlope * CurveExitPoint.x;

                    float CentralPointX = (BofCurveExitPointLineEquation - BofCurveEntryPointLineEquation) / (CurveEntryPointInverseSlope - CurveExitPointInverseSlope);
                    float CentralPointY = CurveEntryPointInverseSlope * CentralPointX + BofCurveEntryPointLineEquation;
                    CentralPoint = new Vector3(CentralPointX, 0, CentralPointY);
                    CurveRadius = Mathf.Sqrt((CurveEntryPoint.x - CentralPointX) * (CurveEntryPoint.x - CentralPointX) + (CurveEntryPoint.z - CentralPointY) * (CurveEntryPoint.z - CentralPointY));

                }



            }


            float friction = 0.68f;
            float gravity = 9.8f;
            LimitSpeedForCorner = Mathf.Sqrt(gravity * CurveRadius * friction);//circular motion equation found it on youtube https://www.youtube.com/watch?v=fvDR-gIf1fo
            LimitSpeedForCornerInKM = LimitSpeedForCorner / 1000 * 3600;

            float ActualKinitecEnergy = 0.5f * GetComponent<Rigidbody>().mass * (Speed / 3600 * 1000) * (Speed / 3600 * 1000);
            ActualKinitecEnergy = ActualKinitecEnergy * 2;
            float CornerKinitecEnergy = 0.5f * GetComponent<Rigidbody>().mass * LimitSpeedForCorner * LimitSpeedForCorner;
            float MaxForceOfBraking = Perfermance.BrakingTorque / WheelColliders[0].radius; //Perfermance.BrakingTorque might get modified from settings

            float TheWorkNeededToLowerSpeed = ActualKinitecEnergy - CornerKinitecEnergy;
            float DistanceNeededWhenApplyingFullBrake = TheWorkNeededToLowerSpeed / MaxForceOfBraking;

            if (TheWorkNeededToLowerSpeed <= 0 || CurrentWayPointIndex > LastLookAheadAnglePositionInWayPoints)//meaning we are below the limitCurveSpeed or NoCurveFound at the end of track
            {
                AIAccelerates = true;
                AIBrakes = false;
                AIAccelerationValue = 1;
            }
            else
            if ((LastLookAheadAnglePositionInWayPoints - CurrentWayPointIndex) * 10 > DistanceNeededWhenApplyingFullBrake)
            {
                AIAccelerates = true;
                AIBrakes = false;
                AIAccelerationValue = 1;
            }
            else if ((LastLookAheadAnglePositionInWayPoints - CurrentWayPointIndex) * 10 < DistanceNeededWhenApplyingFullBrake)
            {
                AIAccelerates = false;
                AIBrakes = true;
                AIAccelerationValue = 0;
            }



        }
    }
    void UpdateCalculations()
    {

        if (RaceSystem == null)
            RaceSystem = GameObject.Find("RaceSystem").GetComponent<RaceSystem>();
        if (RaceSystem != null)
        {

            SharedCodeBetweenPlayerAndAI();


            if (GamePlayScore.FinishedTheRace == false && RaceSystem.RaceStarted == true)
            {

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (Input.GetKey(KeyCode.Q))
                {
                    if (WheelColliders[0].rpm < 0)
                    {
                        IsAcceleratingCommandBool = false;
                        IsBrakingCommanBool = true;
                        Brakes();
                        
                    }
                    else
                    {
                        IsAcceleratingCommandBool = true;
                        IsBrakingCommanBool = false;

                        if (Gears < 1)
                        {
                            SwitchGears(1);
                        }
                       
                        Accelerates();
                     

                    }
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    if (WheelColliders[0].rpm > 0)
                    {
                        IsAcceleratingCommandBool = false;
                        IsBrakingCommanBool = true;
                        Brakes();
                    }
                    else
                    {
                        if (Gears != -1)
                        {
                            SwitchGears(-1);
                        }
                        else
                        {
                          
                            Accelerates();
                            IsAcceleratingCommandBool = true;
                            IsBrakingCommanBool = false;
                        }


                    }
                }
                else
                {
                    ApplyTorque(0);
                    Brake(0);
                }
               
                Steer();
#endif
#if UNITY_ANDROID && !UNITY_EDITOR


                if (AccelerationButtonPressed)
                { 
                    if (WheelColliders[0].rpm < 0)
                    {
                        IsAcceleratingCommandBool = false;
                        IsBrakingCommanBool = true;
                        Brakes();
                    }
                    else
                    {
                        IsAcceleratingCommandBool = true;
                        IsBrakingCommanBool = false;

                        if (Gears < 1)
                        {
                            SwitchGears(1);
                        }

                        Accelerates();

                    }
                }
                else if (BrakingButtonPressed)
                { 
                    if (WheelColliders[0].rpm > 0)
                    {
                        IsAcceleratingCommandBool = false;
                        IsBrakingCommanBool = true;
                        Brakes();
                    }
                    else
                    {
                        if (Gears != -1)
                        {
                            SwitchGears(-1);
                        }
                        else
                        {
                            Accelerates();
                            IsAcceleratingCommandBool = true;
                            IsBrakingCommanBool = false;
                        }


                    }
                }
                else
                { 
                    ApplyTorque(0);
                    Brake(0);
                }

                Steer();

#endif





            }
            else
            {
                Brakes();
            }

            RotateTheWheelMeshes();
        }
    }
    void SharedCodeBetweenPlayerAndAI()
    {
        for (int i = 0; i < 4; i++)
        {
            if (WheelColliders[i].isGrounded)
            {
                WheelRPM = WheelColliders[i].rpm;
                //  Speed = Mathf.RoundToInt(WheelColliders[i].rpm * (WheelColliders[0].radius * 2 * 100) * 0.001885f);//https://www.easycalculation.com/unit-conversion/rpm-conversion.php look the formula in website
                Speed = CalculateSpeed(WheelColliders[i].rpm);
                break;
            }

        }


        RPM = WheelRPM * WheelRPMtoEngineRPMmultiplier;


        RPM = Mathf.Abs(RPM);



        RPM = Mathf.Clamp(RPM, Perfermance.MinEngineRPM, Perfermance.MaxEngineRPM);

        RPM = Mathf.RoundToInt(RPM);
        //  Torque = Perfermance.ModifiableMaxTorqueByItems - (Perfermance.ModifiableMaxTorqueByItems / Perfermance.MaxEngineRPM * RPM);

        Torque = Perfermance.ModifiableMaxTorqueByItems - ((Perfermance.ModifiableMaxTorqueByItems / (Perfermance.MaxEngineRPM - Perfermance.MinEngineRPM)) * (RPM - Perfermance.MinEngineRPM));
        if (Gears != -1 && Gears != Perfermance.WRPMtoERPMmultiplierByGears.Length)
        {
            //i excluded -1  and the topgear because if we dont speed will keep increasing
            //we added 10 percent torque so that at max engine rpm we still have some power to gear up
            Torque += Perfermance.ModifiableMaxTorqueByItems * 0.1f;
        }

        /*   Torque = ((Perfermance.EngineTorqueAtMinEngineRPM-Perfermance.MinEngineTorque)/Mathf.Pow(Perfermance.MinEngineRPM-Perfermance.RPMatMinEngineTorque,2)) * Mathf.Pow((RPM - Perfermance.RPMatMinEngineTorque), 2) + Perfermance.MinEngineTorque;                       //        https://www.desmos.com/calculator/8keeh1pt5l parabolas vertex form  
           Percentage = (1 - ((RPM - Perfermance.MinEngineRPM) / (Perfermance.MaxEngineRPM - Perfermance.MinEngineRPM))) * 100;
           Torque = Torque * (1 -( (RPM-Perfermance.MinEngineRPM ) / (Perfermance.MaxEngineRPM-Perfermance.MinEngineRPM)) );*/
        /*  if (RPM>=Perfermance.MaxEngineRPM &&  ( Gears == Perfermance.WRPMtoERPMmultiplierByGears.Length - 1||Gears==-1) )
           {
               Torque = 0;
           }*/
        switch (Gears)
        {
            case -1:
                AppliedTorque = -Perfermance.ReverseWRPMtoERPMmultiplier * (Torque/10);
                WheelRPMtoEngineRPMmultiplier = Perfermance.ReverseWRPMtoERPMmultiplier;
                break;
            case 0:
                AppliedTorque = 0;
                //you should apply brake here in case of Slope Road
                break;
            case 1:
                AppliedTorque = Perfermance.WRPMtoERPMmultiplierByGears[Gears] * Torque;
                WheelRPMtoEngineRPMmultiplier = Perfermance.WRPMtoERPMmultiplierByGears[Gears];
                break;
            case 2:
                AppliedTorque = Perfermance.WRPMtoERPMmultiplierByGears[Gears] * Torque;
                WheelRPMtoEngineRPMmultiplier = Perfermance.WRPMtoERPMmultiplierByGears[Gears];
                break;
            case 3:
                AppliedTorque = Perfermance.WRPMtoERPMmultiplierByGears[Gears] * Torque;
                WheelRPMtoEngineRPMmultiplier = Perfermance.WRPMtoERPMmultiplierByGears[Gears];
                break;
            case 4:
                AppliedTorque = Perfermance.WRPMtoERPMmultiplierByGears[Gears] * Torque;
                WheelRPMtoEngineRPMmultiplier = Perfermance.WRPMtoERPMmultiplierByGears[Gears];
                break;
            case 5:
                AppliedTorque = Perfermance.WRPMtoERPMmultiplierByGears[Gears] * Torque;
                WheelRPMtoEngineRPMmultiplier = Perfermance.WRPMtoERPMmultiplierByGears[Gears];
                break;
            case 6:
                AppliedTorque = Perfermance.WRPMtoERPMmultiplierByGears[Gears] * Torque;
                WheelRPMtoEngineRPMmultiplier = Perfermance.WRPMtoERPMmultiplierByGears[Gears];
                break;
            case 7:
                AppliedTorque = Perfermance.WRPMtoERPMmultiplierByGears[Gears] * Torque;
                WheelRPMtoEngineRPMmultiplier = Perfermance.WRPMtoERPMmultiplierByGears[Gears];
                break;
            case 8:
                AppliedTorque = Perfermance.WRPMtoERPMmultiplierByGears[Gears] * Torque;
                WheelRPMtoEngineRPMmultiplier = Perfermance.WRPMtoERPMmultiplierByGears[Gears];
                break;


        }
        if (Gears != 0 && Gears != -1)
        {
            if (Gears + 1 < Perfermance.WRPMtoERPMmultiplierByGears.Length)
            {
                if (WheelRPM * Mathf.Abs(Perfermance.WRPMtoERPMmultiplierByGears[Gears + 1]) > Perfermance.GearUpRPMNextGear[Gears] && Speed > 0)
                {

                    SwitchGears(1);

                }
                else
        if (Gears >= 2 && WheelRPM * Mathf.Abs(Perfermance.WRPMtoERPMmultiplierByGears[Gears - 1]) < Perfermance.GearDownRPMpreviousGear[Gears])
                {

                    SwitchGears(-1);
                }

            }
            else
             if (Gears >= 2 && WheelRPM * Perfermance.WRPMtoERPMmultiplierByGears[Gears - 1] < Perfermance.GearDownRPMpreviousGear[Gears])
            {

                SwitchGears(-1);
            }
        }




    }
    public void ResetCarPosition()
    {
        //adjust the waypoints in scene so that the Z axis points forward
        ApplyTorque(0);
        Brake(0);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        float MinDistance = 9999999999;
        int MinDistanceIndex = 0;
        for (int i = 0; i < RaceSystem.WayPoints.Length; i++)
        {
            if (Vector3.Distance(RaceSystem.WayPoints[i].transform.position, transform.position) < MinDistance)
            {
                MinDistance = Vector3.Distance(RaceSystem.WayPoints[i].transform.position, transform.position);
                MinDistanceIndex = i;
            }
        }
        MinDistanceIndex--;//so that the ResetAngle is correct since we dont know if mindistance WayPoint is ahead/behind of us
        
        Vector2 FirstPoint = new Vector2(RaceSystem.WayPoints[MinDistanceIndex].transform.position.x, RaceSystem.WayPoints[MinDistanceIndex].transform.position.z);
        Vector2 SecondPoint= new Vector2(RaceSystem.WayPoints[MinDistanceIndex+1].transform.position.x, RaceSystem.WayPoints[MinDistanceIndex+1].transform.position.z);
        Vector2 desiredDirectionVector = SecondPoint-FirstPoint;
        //  desiredDirectionVector = desiredDirectionVector.normalized;
        desiredDirectionVector.y = 0;

        /*
        
        float ResetAngle = Vector2.SignedAngle(transform.forward,desiredDirectionVector);
     
        transform.position = RaceSystem.WayPoints[MinDistanceIndex].transform.position + new Vector3(0, 3, 0);

        Quaternion rotation = Quaternion.AngleAxis(-ResetAngle, Vector3.up);
        transform.rotation *= rotation;
        transform.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, 0);*/
        Quaternion newRotation = Quaternion.LookRotation(desiredDirectionVector);
        transform.rotation = newRotation;

    }
   
    IEnumerator AICheckIfCarIsReset()
    {
       
        yield return new WaitForSeconds(3f);
        if (WheelColliders[0].isGrounded && WheelColliders[1].isGrounded && WheelColliders[2].isGrounded && WheelColliders[3].isGrounded)
        {
            AICarPoistionIsBeingReset = false;
        }
        else
        {
            StartCoroutine("AICheckIfCarIsReset");
        }
    }
    public float CalculateSpeed(float WheelRPM)
    {
      return  Mathf.RoundToInt(WheelRPM * 2 * Mathf.PI * WheelColliders[0].radius * 0.001f *60);
       
    }
    public void ApplyTorque(float a)
    {
        /*

        if (Gears != -1)
        {
            if (IsAcceleratingCommandBool)
            {

                if (graphOverlay.longData >= 0.3 || graphOverlay.latData >= 0.3 || graphOverlay.latData <= -0.3)
                {
                    percentageToUseFromTorqueAccelerating -= 0.01f;
                }
                else
                {
                    percentageToUseFromTorqueAccelerating += 0.01f;
                }
            }
            else { percentageToUseFromTorqueAccelerating = 0; }
        }
        else
        {
            if (IsAcceleratingCommandBool)
            {

                if (graphOverlay.longData <= -0.3)
                {
                    percentageToUseFromTorqueAccelerating -= 0.01f;
                }
                else
                {
                    percentageToUseFromTorqueAccelerating += 0.01f;
                }
            }
            else { percentageToUseFromTorqueAccelerating = 0; }
        }
           
           percentageToUseFromTorqueAccelerating= Mathf.Clamp(percentageToUseFromTorqueAccelerating, 0, 1);
           */
        percentageToUseFromTorqueAccelerating = 1;
            foreach (WheelCollider wheel in WheelColliders)
            {
         
                if (wheel.transform.localPosition.z < 0 && Perfermance.FrontWheelDrive != true)
                {
                    wheel.motorTorque = a * percentageToUseFromTorqueAccelerating;
                }

                if (wheel.transform.localPosition.z >= 0 && Perfermance.RearWheelDrive != true)
                {
                    wheel.motorTorque = a *percentageToUseFromTorqueAccelerating;
                }
            }




    }
    public void Accelerates()
    {
      
        ApplyTorque(AppliedTorque);
        Brake(0);
    }
    public void Brakes()
    {
        ApplyTorque(0);
        Brake(Perfermance.BrakingTorque);
    }
    public void Brake(int a)
    {
        /*
        if (IsBrakingCommanBool)
        {
            if (graphOverlay.longData <= -0.1)
            {
                percentageToUseFromTorqueBraking -= 0.01f;
            }
            else 
            {
                percentageToUseFromTorqueBraking += 0.01f;
            }
        }
        else { percentageToUseFromTorqueBraking = 0; }

        percentageToUseFromTorqueBraking = Mathf.Clamp(percentageToUseFromTorqueBraking, 0, 1);
        */
        percentageToUseFromTorqueBraking = 1;
        foreach (WheelCollider wheel in WheelColliders)
        {
                wheel.brakeTorque = a * percentageToUseFromTorqueBraking;
           

        }

    }
    private void RotateTheWheelMeshes()
    {
        WheelMeshes[0].transform.localRotation = Quaternion.Euler(new Vector3(WheelColliders[0].rpm / 60 * 360, WheelColliders[0].steerAngle, 0));
        WheelMeshes[1].transform.localRotation = Quaternion.Euler(new Vector3(WheelColliders[1].rpm / 60 * 360, WheelColliders[1].steerAngle, 0));

        WheelMeshes[2].transform.localRotation = Quaternion.Euler(new Vector3(WheelColliders[2].rpm / 60 * 360, 0, 0));
        WheelMeshes[3].transform.localRotation = Quaternion.Euler(new Vector3(WheelColliders[3].rpm / 60 * 360, 0, 0));
    }
    public void SwitchGears(int a)
    {


        if (Gears > -1 && a < 0)
        {
            Gears += a;
        }
        if (Gears < Perfermance.WRPMtoERPMmultiplierByGears.Length - 1 && a > 0)
        {
            Gears += a;
        }

    }
    public void Steer()
    {
        if (AIDriving)
        {

/*  //should be used in circuits
            if (CurrentWayPointIndex > RaceSystem.WayPoints.Length - 5)
            {
                CurrentWayPointIndex = 1;
                LastLookAheadAnglePositionInWayPoints = -1;
            }
*/
            for (int i = CurrentWayPointIndex; i < CurrentWayPointIndex + 4; i++)
            {
                if (i < RaceSystem.WayPoints.Length)
                {
                    if (Vector3.Distance(transform.position, RaceSystem.WayPoints[i].position) < Vector3.Distance(transform.position, RaceSystem.WayPoints[CurrentWayPointIndex].position))
                    {
                        CurrentWayPointIndex = i;//closest way point
                    }
                }

            }


            Vector3 relativeVector = Vector3.zero;
            if (RaceSystem.WayPoints.Length != 0 && CurrentWayPointIndex<RaceSystem.WayPoints.Length)
            {
                if (Vector3.Distance(transform.position, RaceSystem.WayPoints[CurrentWayPointIndex].position) < 10 || (Vector3.Distance(transform.position, RaceSystem.WayPoints[CurrentWayPointIndex].position) < 30 && Avoiding))
                {
                    CurrentWayPointIndex++;
                }
            }


            if (Avoiding)
            {


                //   WheelColliders[0].steerAngle = AvoidMultiplier * 25;
                //   WheelColliders[1].steerAngle = AvoidMultiplier * 25;
                ApplySteeringAngle(AvoidMultiplier * 25);
            }
            else if (RaceSystem.WayPoints.Length != 0 && CurrentWayPointIndex<RaceSystem.WayPoints.Length)
            {


                relativeVector = transform.InverseTransformPoint(RaceSystem.WayPoints[CurrentWayPointIndex].position);
                relativeVector = relativeVector / relativeVector.magnitude;

                //   WheelColliders[0].steerAngle = relativeVector.x * 25;
                //   WheelColliders[1].steerAngle = relativeVector.x * 25;
                ApplySteeringAngle(relativeVector.x * 25);
            }



        }
        else
        {

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            if (Input.GetAxis("Horizontal") != 0)
            {
           //     float SteerAngle = (Perfermance.ZeroSpeedSteerAngle - Mathf.Clamp((Speed / Perfermance.MaxSpeedToEachGear[Perfermance.MaxSpeedToEachGear.Length - 1] * Perfermance.ZeroSpeedSteerAngle), 0, Perfermance.ZeroSpeedSteerAngle));
           //     SteerAngle = Mathf.Clamp(SteerAngle, Perfermance.MaxSpeedSteerAngle, Perfermance.ZeroSpeedSteerAngle);
                //  WheelColliders[0].steerAngle = Input.GetAxis("Horizontal") * SteerAngle;
                //   WheelColliders[1].steerAngle = Input.GetAxis("Horizontal") * SteerAngle;
                ApplySteeringAngle(Input.GetAxis("Horizontal") * 30);

            }
            else
            {
                // we should smooth the transition to zero angle
                //  WheelColliders[0].steerAngle = 0;
                //  WheelColliders[1].steerAngle = 0;
                ApplySteeringAngle(0);
            }
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
            if (Input.acceleration.x != 0)
            {
         //  float SteerAngle= (Perfermance.ZeroSpeedSteerAngle - Mathf.Clamp((Speed / Perfermance.MaxSpeedToEachGear[Perfermance.MaxSpeedToEachGear.Length-1] * Perfermance.ZeroSpeedSteerAngle), 0, Perfermance.ZeroSpeedSteerAngle));
         //       SteerAngle = Mathf.Clamp(SteerAngle, Perfermance.MaxSpeedSteerAngle, Perfermance.ZeroSpeedSteerAngle);
               WheelColliders[0].steerAngle=Input.acceleration.x*30;
            WheelColliders[1].steerAngle=Input.acceleration.x*30;
          
            
            }
             
            else
            {
                WheelColliders[0].steerAngle = 0;
                WheelColliders[1].steerAngle = 0;
            }

           
#endif
        }



    }
    void ApplySteeringAngle(float steeringAngle) 
    {
        foreach (WheelCollider wheel in WheelColliders)
        {
            if (wheel.transform.localPosition.z > 0)
                wheel.steerAngle = steeringAngle;

            
        }
    }
    public void Sensor()
    {
        if (AIDriving)
        {
          
            Avoiding = false;
            AvoidMultiplier = 0;
            RaycastHit Hit;


            if (Physics.Raycast(FrontRightSensorStartTransform.position, FrontRightSensorStartTransform.forward, out Hit, SensorsLength))
            {

                if (Hit.transform.CompareTag("Obstacle") || Hit.transform.CompareTag("Player") || Hit.transform.CompareTag("OtherPlayer"))
                {


                    AvoidMultiplier -= 1;

                    Avoiding = true;

                }
                 Debug.DrawLine(FrontRightSensorStartTransform.position, Hit.point);
            }
            else
            if (Physics.Raycast(FrontRightAngleSensorStartTransform.position, FrontRightAngleSensorStartTransform.forward, out Hit, SensorsLength))
            {
                if (Hit.transform.CompareTag("Obstacle") || Hit.transform.CompareTag("Player") || Hit.transform.CompareTag("OtherPlayer"))
                {


                    AvoidMultiplier -= 0.5f;
                    Avoiding = true;

                }
                  Debug.DrawLine(FrontRightAngleSensorStartTransform.position, Hit.point);
            }




            if (Physics.Raycast(FrontLeftSensorStartTransform.position, FrontLeftSensorStartTransform.forward, out Hit, SensorsLength))
            {
                if (Hit.transform.CompareTag("Obstacle") || Hit.transform.CompareTag("Player") || Hit.transform.CompareTag("OtherPlayer"))
                {


                    AvoidMultiplier += 1;
                    Avoiding = true;

                }
                Debug.DrawLine(FrontLeftSensorStartTransform.position, Hit.point);
            }
            else
            if (Physics.Raycast(FrontLeftAngleSensorStartTransform.position, FrontLeftAngleSensorStartTransform.forward, out Hit, SensorsLength))
            {
                if (Hit.transform.CompareTag("Obstacle") || Hit.transform.CompareTag("Player") || Hit.transform.CompareTag("OtherPlayer"))
                {


                    AvoidMultiplier += 0.5f;

                    Avoiding = true;

                }
                Debug.DrawLine(FrontLeftAngleSensorStartTransform.position, Hit.point);
            }

            if (Physics.Raycast(SideRightSensorStartTransform.position, SideRightSensorStartTransform.forward, out Hit, SideSensorLength))
            {
                if ((Hit.transform.CompareTag("Obstacle") || Hit.transform.CompareTag("Player") || Hit.transform.CompareTag("OtherPlayer")))
                {
                    AvoidMultiplier -= 0.15f;
                    Avoiding = true;
                }
            }
            else if (Physics.Raycast(SideLeftSensorStartTransform.position, SideLeftSensorStartTransform.forward, out Hit, SideSensorLength))
            {
                AvoidMultiplier += 0.15f;
                Avoiding = true;
            }

            if (AvoidMultiplier == 0)
            {
                if (Physics.Raycast(FrontSensorStartTransform.position, FrontSensorStartTransform.forward, out Hit, SensorsLength))
                {

                    if (Hit.transform.CompareTag("Obstacle") || Hit.transform.CompareTag("Player") || Hit.transform.CompareTag("OtherPlayer"))
                    {
                          Debug.DrawLine(FrontSensorStartTransform.position, Hit.point);
                        Avoiding = true;

                        if (Hit.normal.x < 0)
                        {
                            AvoidMultiplier = -1;
                        }
                        else
                        {
                            AvoidMultiplier = 1;
                        }
                    }

                }
            }

            if (Avoiding)
            {
                Debug.Log("Avoiding");
            }
        }


    }
    public void ApplyChanges()
    {/*
        WheelColliders[0].mass = SuspensionAndTires.FrontWheelsMass;
        WheelColliders[1].mass = SuspensionAndTires.FrontWheelsMass;
        WheelColliders[2].mass = SuspensionAndTires.RearWheelsMass;
        WheelColliders[3].mass = SuspensionAndTires.RearWheelsMass;


        WheelColliders[0].radius = SuspensionAndTires.FrontWheelsRadius;
        WheelColliders[1].radius = SuspensionAndTires.FrontWheelsRadius;
        WheelColliders[2].radius = SuspensionAndTires.RearWheelsRadius;
        WheelColliders[3].radius = SuspensionAndTires.RearWheelsRadius;

        WheelColliders[0].wheelDampingRate = SuspensionAndTires.FrontWheelsDampingRate;
        WheelColliders[1].wheelDampingRate = SuspensionAndTires.FrontWheelsDampingRate;
        WheelColliders[2].wheelDampingRate = SuspensionAndTires.RearWheelsDampingRate;
        WheelColliders[3].wheelDampingRate = SuspensionAndTires.RearWheelsDampingRate;

        WheelColliders[0].suspensionDistance = SuspensionAndTires.FrontWheelsSuspensionDistance;
        WheelColliders[1].suspensionDistance = SuspensionAndTires.FrontWheelsSuspensionDistance;
        WheelColliders[2].suspensionDistance = SuspensionAndTires.RearWheelsSuspensionDistance;
        WheelColliders[3].suspensionDistance = SuspensionAndTires.RearWheelsSuspensionDistance;

        WheelColliders[0].forwardFriction = SuspensionAndTires.FrontWheelsForwardFriction;
        WheelColliders[1].forwardFriction = SuspensionAndTires.FrontWheelsForwardFriction;
        WheelColliders[2].forwardFriction = SuspensionAndTires.RearWheelsForwardFriction;
        WheelColliders[3].forwardFriction = SuspensionAndTires.RearWheelsForwardFriction;

        WheelColliders[0].sidewaysFriction = SuspensionAndTires.FrontWheelsSideWaysFriction;
        WheelColliders[1].sidewaysFriction = SuspensionAndTires.FrontWheelsSideWaysFriction;
        WheelColliders[2].sidewaysFriction = SuspensionAndTires.RearWheelsSideWaysdFriction;
        WheelColliders[3].sidewaysFriction = SuspensionAndTires.RearWheelsSideWaysdFriction;
        */
    }


}
