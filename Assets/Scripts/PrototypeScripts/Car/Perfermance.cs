using UnityEngine;
using UnityEngine.SceneManagement;

public class Perfermance : MonoBehaviour {


    public Motor Motor;
    [Range(0, 3000)]
    public int BrakingTorque;

    public float OriginalMaxTorque;

    public float ModifiableMaxTorqueByItems;
    public bool AllWheelDrive = false;
    public bool RearWheelDrive = true;
    public bool FrontWheelDrive = false;
    // public int MaxGears;
    // public float EngineTorqueAtMinEngineRPM;
    //  public float MinEngineTorque;
    // public float RPMatMinEngineTorque;
    [Range(1200,9200)]
    public float MaxEngineRPM;
    [Range(1200, 9200)]
    public float MinEngineRPM;
    [Range(1200, 9200)]
    public float[] GearUpRPMNextGear;
    [Range(1200, 9200)]
    public float[] GearUpRPMThisGear;
    [Range(1200, 9200)]
    public float[] GearDownRPMpreviousGear;

    [Header("Max Upgrade Section")]
    [Range(0,3)]
    public int MaxEngineUpgrade;
    [Range(0, 3)]
    public int MaxSuspensionUpgrade;
    [Range(0, 3)]
    public int MaxTiresUpgrade;
    [Range(0, 3)]
    public int MaxBrakesUpgrade;


    [Range(0, 20)]
    public float[] WRPMtoERPMmultiplierByGears;
    [Range(0, 80)]
    public float ReverseWRPMtoERPMmultiplier;

    public float[] MaxSpeedToEachGear;
    public float ZeroSpeedSteerAngle;
    public float MaxSpeedSteerAngle;


    public SaveGame SaveGame;

    public bool AutomaticGearBox = true;

    void OnDrawGizmos()
    {

      //  CalculateGearRatios();
      
    }

   
    void CalculateGearRatios()
    {

        for (int i = 1; i < WRPMtoERPMmultiplierByGears.Length - 1; i++)
        {
            WRPMtoERPMmultiplierByGears[i + 1] = WRPMtoERPMmultiplierByGears[i] / (GearUpRPMThisGear[i] / GearUpRPMNextGear[i]);
        }

       
        for (int i = 1; i < MaxSpeedToEachGear.Length; i++)
        {
            float WheelRPM = 0;
            if (i < WRPMtoERPMmultiplierByGears.Length - 1)
            {

                WheelRPM = GearUpRPMThisGear[i] / WRPMtoERPMmultiplierByGears[i];
                //   MaxSpeedToEachGear[i] = Mathf.RoundToInt(WheelRPM * (WheelColliders[0].radius * 2 * 100) * 0.001885f);  
                MaxSpeedToEachGear[i] = Motor.CalculateSpeed(WheelRPM);
            }

            else if (i == WRPMtoERPMmultiplierByGears.Length)
            {
                WheelRPM = MaxEngineRPM / WRPMtoERPMmultiplierByGears[i];
                //   MaxSpeedToEachGear[i] = Mathf.RoundToInt(WheelRPM * (WheelColliders[0].radius * 2 * 100) * 0.001885f);  
                MaxSpeedToEachGear[i] = Motor.CalculateSpeed(WheelRPM);
            }

        }
 
    }
                                //  private SaveGame SaveGame;
    void Start()
    {
        //SaveGame = GameObject.Find("SaveGame").GetComponent<SaveGame>();
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            this.enabled = false;
        }
        else //if (PhotonNetwork.IsMasterClient)
        {
            SaveGame = GameObject.Find("SaveGame").GetComponent<SaveGame>();
            ModifiableMaxTorqueByItems = OriginalMaxTorque + SaveGame.SelectedCarForPlay.EngineUpgrade * 1 / 3 * OriginalMaxTorque;
            BrakingTorque = BrakingTorque + SaveGame.SelectedCarForPlay.BrakeUpgrade * 1 / 3 * BrakingTorque;
          //  Rpc_SyncData(BrakingTorque, OriginalMaxTorque, ModifiableMaxTorqueByItems, WRPMtoERPMmultiplierByGears, ReverseWRPMtoERPMmultiplier, AutomaticGearBox);
        }
        


    }
    private void Update()
    {
    
        CalculateGearRatios();
/*
        if (Input.GetKeyDown(KeyCode.K))
        {
           ModifiableMaxTorqueByItems = 10000000;
        }
        if (PhotonNetwork.IsMasterClient)
        {
             Rpc_SyncData(BrakingTorque, OriginalMaxTorque, ModifiableMaxTorqueByItems,  WRPMtoERPMmultiplierByGears, ReverseWRPMtoERPMmultiplier, AutomaticGearBox);
        }
        */
    }
    
        
       
   
/// <summary>
/// we might not need to send these values since we are relying on the Photont Transform view to sync position and not acually calculaing the physics at each client
/// </summary>
/// <param name="braking"></param>
/// <param name="originalmaxtorque"></param>
/// <param name="modifiableMaxtorquebyitems"></param>
/// <param name="wrpmtoerpmmultiplierbygears"></param>
/// <param name="reversewrpMtoerpmmultiplier"></param>
/// <param name="automaticgearbox"></param>
/// 
  // [PunRPC]
    void Rpc_SyncData(int braking, float originalmaxtorque, float modifiableMaxtorquebyitems,  float[] wrpmtoerpmmultiplierbygears, float reversewrpMtoerpmmultiplier, bool automaticgearbox)
    {
        BrakingTorque = braking;
        OriginalMaxTorque = originalmaxtorque;
        ModifiableMaxTorqueByItems = modifiableMaxtorquebyitems;
        WRPMtoERPMmultiplierByGears = wrpmtoerpmmultiplierbygears;// at this stage this array is not synced players can manipulate it   
        ReverseWRPMtoERPMmultiplier = reversewrpMtoerpmmultiplier;
        AutomaticGearBox = automaticgearbox;
    }

    
}

