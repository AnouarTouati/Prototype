using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ZombieScript : MonoBehaviour
{
    public Animator Animator;
    public NavMeshAgent Agent;
    private bool HasFallen = false;
    private bool IsUpRightAnimation = true;
    private bool IsGoingToStandUp = false;
    private bool IsAttacking = false;
    private bool IsAboutToAttack = false;
    
    public List<Collider> RagdollParts;
    public List<Rigidbody> RagdollRigid;
    private int NumberOFlives = 2;
    public Transform Hips;
    private Vector3 OriginalHipsPosition;
    private Transform CarTransform;
    private Vector3 PositionBeforeImpact;
    private Quaternion RotationBeforeImpact;
    void Awake()
    {
        RagdollParts=GetComponentsInChildren<Collider>().ToList<Collider>();
        RagdollRigid= GetComponentsInChildren<Rigidbody>().ToList<Rigidbody>();
        OriginalHipsPosition = Hips.position;
        GetComponent<Rigidbody>().useGravity = false;
        DisableRagdoll();
        
    }

    private void Update()
    {
      
            if (Animator.GetCurrentAnimatorStateInfo(0).IsName("Running"))
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }

            if (IsAboutToAttack && Animator.GetCurrentAnimatorStateInfo(0).IsName("HandAttack") && !IsAttacking)
            {
                IsAttacking = true;
            }
            else if (IsAboutToAttack && Animator.GetCurrentAnimatorStateInfo(0).IsName("Running") && IsAttacking)
            {
                IsAttacking = false;
                IsAboutToAttack = false;
            }

            if (HasFallen && Animator.GetCurrentAnimatorStateInfo(0).IsName("Running") && !IsGoingToStandUp)
            {
                IsUpRightAnimation = false;

            }
            else if (!HasFallen && Animator.GetCurrentAnimatorStateInfo(0).IsName("StandUp"))
            {
                IsGoingToStandUp = true;
                IsUpRightAnimation = false;

            }
            else if (!HasFallen && Animator.GetCurrentAnimatorStateInfo(0).IsName("Running") && IsGoingToStandUp)
            {
                IsGoingToStandUp = false;
                IsUpRightAnimation = true;
            }

            if (IsUpRightAnimation && !IsAttacking)
            {
                Agent.acceleration = 1000;
                if (CarTransform == null)
                {
                    if (GameObject.FindGameObjectWithTag("Player"))
                    {
                        CarTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
                    }
                }
                else
                {
                    Agent.destination = CarTransform.position;
                }
            }
            else
            {
                Agent.acceleration = 0;
                Agent.velocity = Vector3.zero;
                if (IsAttacking)
                {
                    transform.position = PositionBeforeImpact;
                    transform.rotation = RotationBeforeImpact;
                }
            }

        
       
    }
    private void DisableRagdoll()
    {
        Animator.SetTrigger("StandUpTrigger");
        foreach (Collider collider in RagdollParts)
        {
            if (collider != this.gameObject.GetComponent<Collider>())
            {
                collider.enabled = false;
            }
        }

        foreach (Rigidbody rigidbody in RagdollRigid)
        {
            if (rigidbody != this.gameObject.GetComponent<Rigidbody>())
            {
                rigidbody.isKinematic = true;
            }
        }
        transform.position = new Vector3(Hips.transform.position.x,0, Hips.position.z);
        Hips.position = OriginalHipsPosition;
        transform.rotation = Quaternion.identity;// when we hit the zombie the rotation changes causing the animation to WalkUp To the sky that is why we reset rotation
        Animator.enabled = true;
        GetComponent<BoxCollider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        
    }
    private void EnableRagdoll()
    {
        Animator.enabled = false;
        GetComponent<BoxCollider>().enabled = false;

        foreach (Collider collider in RagdollParts)
        {
            if (collider != this.gameObject.GetComponent<Collider>())
            {
                collider.enabled = true;
            }
        }
        foreach (Rigidbody rigidbody in RagdollRigid)
        {
            if (rigidbody != this.gameObject.GetComponent<Rigidbody>())
            {
                rigidbody.isKinematic = false;
            }
        }
        GetComponent<Rigidbody>().isKinematic = true; // we enable it because a bug where zombie will dispear under terrain will occur
    }
    private void OnCollisionEnter(Collision collision)
    {
       if (collision.relativeVelocity.magnitude>=10)
        {
            if (!HasFallen)
            {
                if (collision.gameObject.GetComponentInParent<Transform>().tag.Equals("Player"))
                {

                    EnableRagdoll();
                    HasFallen = true;
                    if (NumberOFlives > 0)
                    {
                        StartCoroutine(Revive());
                    }
                    NumberOFlives--;
                }
            }

        }
       

    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.relativeVelocity.magnitude < 10)
        {
            if (collision.gameObject.GetComponentInParent<Transform>().tag.Equals("Player") && !IsAttacking && !IsAboutToAttack)
            {
              
                    Animator.SetTrigger("HandAttackTrigger");
                    PositionBeforeImpact = transform.position;
                    RotationBeforeImpact = transform.rotation;
                    IsAboutToAttack = true;
                  
            }
        }
    }
   
    IEnumerator Revive()
    {
        yield return new WaitForSeconds(10f);
        HasFallen = false;
        DisableRagdoll();
    }
    
}
