/*
using UnityEngine;

public class Items : MonoBehaviour {

    public float LifeTime = 5f;
    public float ItemDamage;
    public float ItemTurboBoostPercentage;


    public bool isWeapon;
    public bool isTurbo;
    public bool isShield;


   // [SyncVar]
    public string NameOfOwningPlayer;
    private void Start()
    {
        StickMySelfToThePlayer();
        if (isTurbo)
        {
            GetComponentInParent<Perfermance>().ModifiableMaxTorqueByItems += GetComponentInParent<Perfermance>().ModifiableMaxTorqueByItems * ItemTurboBoostPercentage/100;
            //we revert this effect in server controlled beaviour when the item is destroyed   StartAutoDestructionForItem function
        }else if (isWeapon)
        {
            //we alredy did it OnTriggerEnter
        }
        else if(isShield)
        {
            //for now we just make it as absolute shield no damage is taken by covering the player with a the item(Shield prefab) collider
        }

    }
   
    
    //we use OnTriggerEnter instead of OnCllisionEnter because we dont wanna attach rigidbody to item due to unrealstic bahviour
    //we also use two colliders one as Trigger and the other fo detecting collisions
    //to properly calculate the dammage we use the velocity/force of the car insstead of this object cause we must not include any rigidbody
    void OnTriggerEnter(Collider info)
    {
      
        if (isServer && isWeapon)
        {
            
            if (info.transform.tag=="Player"|| info.transform.tag == "OtherPlayer")
            {
                info.gameObject.GetComponent<GamePlayScore>().PlayerHealth -= ItemDamage *Mathf.Abs( GetComponentInParent<Rigidbody>().velocity.magnitude - info.GetComponent<Rigidbody>().velocity.magnitude);
                // Debug.Log("i REDUUUUUCED THE HEALTH OF PLAYER by "+ItemDamage * GetComponentInParent<Rigidbody>().velocity.magnitude);
               
            }
        }
    }
    void StickMySelfToThePlayer()
    {
        Transform[] temp;
        temp = GameObject.Find(NameOfOwningPlayer).GetComponentsInChildren<Transform>();
        for (int i = 0; i < temp.Length; i++)
        {
            if(temp[i].name== "TurboItemSpawnPoint")
            {
               
                transform.SetParent(temp[i],false);// use this method with false argument to prevent automatic resizing
                transform.position = temp[i].position;
                transform.rotation = temp[i].rotation;
                break;
            }
        }
    }
    
}
*/