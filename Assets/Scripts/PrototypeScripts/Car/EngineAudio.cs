using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class EngineAudio : MonoBehaviourPunCallbacks,IPunObservable
{
    
  //  FMOD.Studio.EventInstance EngineSource;
  //  FMOD.Studio.ParameterInstance RPM;
  //  FMOD.Studio.ParameterInstance DistanceFromListener;
    public Motor Motor;
    public float speed;
    public Transform CameraTransform;
    public float RPMcopy;//i used a copy to syn it to other instances
    public PhotonView PV;
    public string name;
    void Start()
    {
        
    //    EngineSource=  FMODUnity.RuntimeManager.CreateInstance("event:/Engine/"+name);
    //    FMODUnity.RuntimeManager.AttachInstanceToGameObject(EngineSource, this.GetComponent<Transform>(), this.GetComponent<Rigidbody>());
        
    //    EngineSource.getParameter("RPM", out RPM);
    //    EngineSource.getParameter("DistanceFromListener", out DistanceFromListener);
    //    EngineSource.start();
        if (CameraTransform == null && GameObject.Find("CamParent"))
        {
            CameraTransform = GameObject.Find("CamParent").GetComponent<Transform>();
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            RPMcopy = Motor.RPM;
        }
       
        if (CameraTransform == null && GameObject.Find("CamParent"))
        {
            CameraTransform = GameObject.Find("CamParent").GetComponent<Transform>();
        }
        else if(CameraTransform!=null)
        {
            
           
          //    DistanceFromListener.setValue(Mathf.Clamp(Vector3.Distance(this.GetComponent<Transform>().position, CameraTransform.position),0,90));
           
           
        }

      //  RPM.setValue(RPMcopy);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(RPMcopy);

        }
        else
        {
           RPMcopy=(float) stream.ReceiveNext();
        }
    }
    
    public void ResetTheScript()
    {
     //  EngineSource.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        
    }
    
}
