using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DogFieldOfView : MonoBehaviour
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
    Vector3 fovDir = new Vector3();
    DogController dogController;
    WolfController wolfController;
    AnimatorClipInfo[] clipAnimArray;
    Animator animator;
    private AnimatorUpdater animatorUpdater;
    [SerializeField] FOVVisual fovVisual;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        wolfController = playerRef.GetComponent<WolfController>();
        dogController = GetComponent<DogController>();
        animatorUpdater = GetComponent<AnimatorUpdater>();
        

        animator = GetComponentInChildren<Animator>();
        StartCoroutine(FOVRoutine());
    }

    // Update is called once per frame
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

    private void FieldOfViewCheck()
    {
        Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(transform.position,  radius, playerMask);
        Collider2D[] closeRangeChecks = Physics2D.OverlapCircleAll(transform.position,  closeradius, playerMask);
        WhereIsDogLooking(); 
        if (closeRangeChecks.Length == 0 && rangeChecks.Length != 0 && !wolfController.isStealthed ) // if player is in the circle's range
        {
            
            WhereIsDogLooking();    // check where the dog is facing now
                                    

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
                    dogController.SetChasingState();
                    Debug.DrawLine(transform.position, target.position, Color.white, 2.5f);
                    // debug white line shows fov - 5 times a second
                }
                else // if player is behind an obstacle
                {
                    canSeePlayer = false;
                    dogController.PlayerLastSeen(); // if dog is chasing
                                         //sets dog to lost state and saves a ref to last point 
                                         //where it saw the player
                }
            }
            else // if player isn't in view angle
            {
                canSeePlayer = false;
                dogController.PlayerLastSeen();
                
            }
        }
        else if (closeRangeChecks.Length == 0 && canSeePlayer) // if player was in view angle but got away from it
        {
            canSeePlayer = false;
            dogController.PlayerLastSeen();
            
            // method for getting last pos seen, going there, then resetting to calm on reach destination

        }
        else if (closeRangeChecks.Length != 0)
        {
            canSeePlayer = true;
            Transform target = closeRangeChecks[0].transform;
                    dogController.SetChasingState();
                    Debug.DrawLine(transform.position, target.position, Color.white, 2.5f);
        }
    }

    
     public void WhereIsDogLooking()
    {
        animatorUpdater.AbsVectors();
        animatorUpdater.NormalizeMoveDest();

        clipAnimArray = animator.GetCurrentAnimatorClipInfo(0);
        string animName = (clipAnimArray[0].clip.name);

        if (animName == "Idle_Down_Dog"
            || animName == "Walk_Down_Dog")
        {
            fovDir = -fovDirs[2].transform.up;
            fovVisual.SetOrigin(fovDirs[2].transform.position);
            fovVisual.SetStartingAngle(0f);
            // Debug.Log("down");
        }
        else if (animName == "Walk_Up_Dog")
        {
            fovDir = fovDirs[0].transform.up;
            fovVisual.SetOrigin(fovDirs[0].transform.position);
            fovVisual.SetStartingAngle(180f);
            // Debug.Log("up");
        }
        else if (animName == "Walk_Right_Dog")
        {
            fovDir = fovDirs[1].transform.right;
            fovVisual.SetOrigin(fovDirs[1].transform.position);
            fovVisual.SetStartingAngle(90f);
            //  Debug.Log("right");
        }
        else if (animName == "Walk_Left_Dog")
        {
            fovDir = -fovDirs[3].transform.right;
            fovVisual.SetOrigin(fovDirs[3].transform.position);
            fovVisual.SetStartingAngle(270f);
            //  Debug.Log("left");
        }
    }

    
}
