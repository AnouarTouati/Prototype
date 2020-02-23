using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class CarVisualSync : MonoBehaviourPunCallbacks,IPunObservable {

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       
        if (stream.IsWriting )
        {

           
                stream.SendNext(BodyArrayColor);
                stream.SendNext(SideViewMirrosArrayColor);
                stream.SendNext(HoodArrayColor);
                stream.SendNext(RimsArrayColor);
            
               
            // WE SHOULD ONLY SYNC VISUAL APPEARNCES
            // we might not need to send these values since we are relying on the Photont Transform view to sync position and not acually calculaing the physics at each client

            /*    stream.SendNext(EngineLevel);
                stream.SendNext(SuspensionLevel);
                stream.SendNext(TiersLevel);
                stream.SendNext(BrakeLevel);*/

        }
        else
        {
           

          
            BodyArrayColor = (float[])stream.ReceiveNext();
            SideViewMirrosArrayColor = (float[])stream.ReceiveNext();
            HoodArrayColor = (float[])stream.ReceiveNext();
            RimsArrayColor = (float[])stream.ReceiveNext();
            // WE SHOULD ONLY SYNC VISUAL APPEARNCES
            // we might not need to send these values since we are relying on the Photont Transform view to sync position and not acually calculaing the physics at each client
            /*   EngineLevel = (int)stream.ReceiveNext(); ;
               SuspensionLevel = (int)stream.ReceiveNext(); 
               TiersLevel = (int)stream.ReceiveNext(); ;
               BrakeLevel = (int)stream.ReceiveNext(); ;*/
        }
    }

    public SaveGame SaveGame;
    public PhotonView PV;
    // private DictionariesClass DictionariesClass=new DictionariesClass();


    [Header("To Be Obtained From SaveGame")]

    //Keep The HideInInspector Attributes because without them errors will occur
    [HideInInspector]
    public float[] BodyArrayColor=new float[4];
    [HideInInspector]
    public float[] SideViewMirrosArrayColor = new float[4];
    [HideInInspector]
    public float[] HoodArrayColor = new float[4];
    [HideInInspector]
    public float[] RimsArrayColor = new float[4];

    
    /////////////////////////////////////////////////////////////////////////////
    Color BodyColor;
    Color SideViewMirrorsColor;
    Color HoodColor;
    Color RimsColor;
    /////////////////////////////////////////////////////////////////////////////
    public Color DefaultBodyColor;
    public Color DefaultSideViewMirrorsColor;
    public Color DefaultHoodColor;
    public Color DefaultRimsColor;
    /////////////////////////////////////////////////////////////////////////////
    public Renderer Body;
    public Renderer SideViewMirrors;
    public Renderer Hood;
    public Renderer[] Rims=new Renderer[4];
    /////////////////////////////////////////////////////////////////////////////
    public bool InstantiedFromSaveGameForInitializingOnly=false;
    bool ColorChangedSendDataAndApplyColors = false;
    /////////////////////////////////////////////////////////////////////////////
    
  
    void Start ()
    {
       
        if (InstantiedFromSaveGameForInitializingOnly == false)
        {
            if (!GameObject.Find("BuyCarGUI"))
            {

                SaveGame = GameObject.Find("SaveGame").GetComponent<SaveGame>();
                GetColorsFromSaveGame();
                ApplyColors();

            }
            else
            {
                ApplyDefaultColors();
            }
        }
       
         

    }
	
	
	void Update ()
    {
     
        if (SaveGame == null)
        {
            SaveGame = GameObject.Find("SaveGame").GetComponent<SaveGame>();
        }
        if (InstantiedFromSaveGameForInitializingOnly==false)
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                if (!GameObject.Find("BuyCarGUI") )
                {
                    GetColorsFromSaveGame();
                    CheckForColorChange();

                    if (ColorChangedSendDataAndApplyColors)
                    {
                        ApplyColors();
                      
                    }


                }
                else
                {
                    ApplyDefaultColors();//find solution for this so we dont keep calling it
                }
            }
         
           
           else
            {
                if (GetComponent<Transform>().tag.Equals("AIDriving")) { //&& transform.tag=="AIDriving" is to detect in offline mode and not apply the same colors as Player pref
                    ///this block my break online syncing i didnt test it yet
                    ApplyDefaultColors();
                }
                  else  if (PV.IsMine)
                {
                    GetColorsFromSaveGame();
                    CheckForColorChange();
                    if (ColorChangedSendDataAndApplyColors)
                    {
                        ApplyColors();
                    }
                   
                }
                else
                {

                    CheckForColorChange();
                    if (ColorChangedSendDataAndApplyColors)
                    {
                        Debug.Log("we applied colors");
                        ApplyColors();
                    }
                }
            }
        }


       
    }
    void CheckForColorChange()
    {
        ColorChangedSendDataAndApplyColors = false;
        for (int i = 0; i < 4; i++)
        {
            if (Body != null)
            {
                if (BodyArrayColor[i] != Body.material.color[i])
                {
                    ColorChangedSendDataAndApplyColors = true;


                }
            }
          
            
        }
        for (int i = 0; i < 4; i++)
        {
            if (SideViewMirrors != null)
            {
                if (SideViewMirrosArrayColor[i] != SideViewMirrors.material.color[i])
                {
                    ColorChangedSendDataAndApplyColors = true;


                }
            }
           
        }
        for (int i = 0; i < 4; i++)
        {
            if (Hood != null)
            {
                if (HoodArrayColor[i] != Hood.material.color[i])
                {
                    ColorChangedSendDataAndApplyColors = true;

                }
            }
            
        }
        for (int i = 0; i < 4; i++)
        {
            if (Rims != null)
            {
                if (Rims[i] != null)
                {
                    if (RimsArrayColor[i] != Rims[0].material.color[i])
                    {
                        ColorChangedSendDataAndApplyColors = true;


                    }
                }
            }
            
        }
        
    }
    void GetColorsFromSaveGame()
    {
        BodyArrayColor = 
            SaveGame.SelectedCarForPlay.BodyColor;
        
        SideViewMirrosArrayColor = SaveGame.SelectedCarForPlay.SideViewMirrorsColor;
        
        HoodArrayColor = SaveGame.SelectedCarForPlay.HoodColor;
        
        RimsArrayColor = SaveGame.SelectedCarForPlay.RimsColor;
        
    }
    void ApplyColors()
    {
       
        BodyColor = new Color(BodyArrayColor[0], BodyArrayColor[1], BodyArrayColor[2], BodyArrayColor[3]);
        SideViewMirrorsColor = new Color(SideViewMirrosArrayColor[0], SideViewMirrosArrayColor[1], SideViewMirrosArrayColor[2], SideViewMirrosArrayColor[3]);
        HoodColor = new Color(HoodArrayColor[0], HoodArrayColor[1], HoodArrayColor[2], HoodArrayColor[3]);
        RimsColor = new Color(RimsArrayColor[0], RimsArrayColor[1], RimsArrayColor[2], RimsArrayColor[3]);
        if(Body!=null)
        Body.material.color = BodyColor;
        if (SideViewMirrors != null)
        SideViewMirrors.material.color = SideViewMirrorsColor;
        if (Hood != null)
        Hood.material.color = HoodColor;
        if (Rims != null)
        for (int i = 0; i < Rims.Length; i++)
        {
                if (Rims[i] != null)
                {
                    Rims[i].material.color = RimsColor;
                }
           
        }
    }

   void ApplyDefaultColors()
    {
       
        if (Body != null)
            Body.material.color = DefaultBodyColor;
        if (SideViewMirrors != null)
            SideViewMirrors.material.color = DefaultSideViewMirrorsColor;
        if (Hood != null)
            Hood.material.color = DefaultHoodColor;
        if (Rims != null)
        {
            for (int i = 0; i < Rims.Length; i++)
            {
                if (Rims[i] != null)
                {
                    Rims[i].material.color = DefaultRimsColor;
                }
                    
            }
        }
           
            
    }
}
