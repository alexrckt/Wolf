using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerFOV : MonoBehaviour
{
    public float radius;
    public float closeradius;
    [Range (0, 360)]
    public float angle;
    public bool canSeePlayer;
    public Transform fovPoint;
    public GameObject playerRef;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public GameObject[] fovDirs;
    WolfController wc;
    FarmerController fc;
    Animator animator;
    AnimatorClipInfo[] clipAnimArray;
    Vector3 fovDir = new Vector3();
    public Transform viewPoint; // which way we are looking - one of four "vision point" objects
    public string viewPointString;
    
    
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        wc = playerRef.GetComponent<WolfController>();
        fc = GetComponent<FarmerController>();
        animator = GetComponent<Animator>();
        StartCoroutine(FOVRoutine());
    }

    
    void Update()
    {
        
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
            
        }
    }

    private void FieldOfViewCheck() // may have problems with logic!
    {
         Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(transform.position,  radius, playerMask);
        Collider2D[] closeRangeChecks = Physics2D.OverlapCircleAll(transform.position,  closeradius, playerMask);

        if (closeRangeChecks.Length != 0)
    {
        WhereIsFarmerLooking();
        Transform target = closeRangeChecks[0].transform;
        Vector2 directionToTarget = (target.position - transform.position).normalized;

       if (Vector2.Angle(fovDir, directionToTarget) < angle / 2) // if player in view angle
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);
                if (!Physics2D.Raycast(transform.position, directionToTarget,
                                                distanceToTarget, obstacleMask)) // if player isn't
                                                                                   // behind an obstacle
                {
                    canSeePlayer = true;
                    //dc.SetChasingState();
                    Debug.DrawLine(transform.position, target.position, Color.white, 2.5f);
                    // debug white line shows fov - 5 times a second
                }
                else if (closeRangeChecks.Length != 0) // if player is behind an obstacle
                {
                    canSeePlayer = false;
                    fc.PlayerLastSeen(); // if dog is chasing
                                         //sets dog to lost state and saves a ref to last point 
                                         //where it saw the player
                }
            }
            else // if player isn't in view angle
            {
                canSeePlayer = false;
                fc.PlayerLastSeen();
                
            }
        }

        
        else if (rangeChecks.Length != 0 && !wc.isStealthed)
        {
            WhereIsFarmerLooking();
        Transform target = rangeChecks[0].transform;
        Vector2 directionToTarget = (target.position - transform.position).normalized;

       if (Vector2.Angle(fovDir, directionToTarget) < angle / 2) // if player in view angle
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);
                if (!Physics2D.Raycast(transform.position, directionToTarget,
                                                distanceToTarget, obstacleMask)) // if player isn't
                                                                                   // behind an obstacle
                {
                    canSeePlayer = true;
                    //dc.SetChasingState();
                    Debug.DrawLine(transform.position, target.position, Color.white, 2.5f);
                    // debug white line shows fov - 5 times a second
                }
                else if (rangeChecks.Length != 0) // if player is behind an obstacle
                {
                    canSeePlayer = false;
                    fc.PlayerLastSeen(); // if dog is chasing
                                         //sets dog to lost state and saves a ref to last point 
                                         //where it saw the player
                }
            }
            else // if player isn't in view angle
            {
                canSeePlayer = false;
                fc.PlayerLastSeen();
                
            }
                    
        }

        else if (rangeChecks.Length == 0 && canSeePlayer) // if player was in view angle but got away from it
        {
            canSeePlayer = false;
            fc.PlayerLastSeen();
            
            

        }




    
    }





    public void WhereIsFarmerLooking() // sets dir for the purpose of FOV angle + sets viewpoint obj
    {
        fc.AbsVectors();
        fc.NormalizeMoveDest();
        
        clipAnimArray = animator.GetCurrentAnimatorClipInfo(0);
        string animName = (clipAnimArray[0].clip.name);

        if (animName == "Idle_Down_Farmer" || animName == "Shoot_Down_Farmer"
         || animName == "Walk_Down_Farmer")
        {
           fovDir = -fovDirs[2].transform.up;
           viewPoint = fovDirs[2].transform; // which way we are looking
           viewPointString = "down";
        }
        else if (animName == "Idle_Up_Farmer" || animName == "Shoot_Up_Farmer"
         || animName == "Walk_Up_Farmer")
         {
            fovDir = fovDirs[0].transform.up;
            viewPoint = fovDirs[0].transform;
            viewPointString = "up";
         }
         else if (animName == "Idle_Right_Farmer" || animName == "Shoot_Right_Farmer"
         || animName == "Walk_Right_Farmer")
         {
            fovDir = fovDirs[1].transform.right;
            viewPoint = fovDirs[1].transform;
            viewPointString = "right";
         }
         else if (animName == "Idle_Left_Farmer" || animName == "Shoot_Left_Farmer"
         || animName == "Walk_Left_Farmer")
         {
            fovDir = -fovDirs[3].transform.right;
            viewPoint = fovDirs[3].transform;
            viewPointString = "left";
         }
    }

    
}
