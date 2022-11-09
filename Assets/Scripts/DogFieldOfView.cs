using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

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
    DogController dc;
    WolfController wc;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        wc = playerRef.GetComponent<WolfController>();
        dc = GetComponent<DogController>();
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
        if (closeRangeChecks.Length == 0 && rangeChecks.Length != 0 && !wc.isStealthed ) // if player is in the circle's range
        {
            
            WhereIsDogLooking();    // check where the dog is facing now
                                    // maybe it's better to check directly which anim is playing? idk

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
                    dc.SetChasingState();
                    Debug.DrawLine(transform.position, target.position, Color.white, 2.5f);
                    // debug white line shows fov - 5 times a second
                }
                else // if player is behind an obstacle
                {
                    canSeePlayer = false;
                    dc.PlayerLastSeen(); // if dog is chasing
                                         //sets dog to lost state and saves a ref to last point 
                                         //where it saw the player
                }
            }
            else // if player isn't in view angle
            {
                canSeePlayer = false;
                dc.PlayerLastSeen();
                
            }
        }
        else if (closeRangeChecks.Length == 0 && canSeePlayer) // if player was in view angle but got away from it
        {
            canSeePlayer = false;
            dc.PlayerLastSeen();
            
            // method for getting last pos seen, going there, then resetting to calm on reach destination

        }
        else if (closeRangeChecks.Length != 0)
        {
            canSeePlayer = true;
            Transform target = closeRangeChecks[0].transform;
                    dc.SetChasingState();
                    Debug.DrawLine(transform.position, target.position, Color.white, 2.5f);
        }


    }

    void WhereIsDogLooking()
    {
        dc.NormalizeMoveDest();

        if (dc.horizontal == 1)
             fovDir = fovDirs[1].transform.right; // right - east
        else if (dc.horizontal == -1)
             fovDir = -fovDirs[3].transform.right; // left - west
        else if (dc.vertical == -1)
             fovDir = -fovDirs[2].transform.up; // down - south
        else if (dc.vertical == 1)
             fovDir = fovDirs[0].transform.up; // up - north
        else 
             fovDir = -fovDirs[2].transform.up; // idle - face down
    }

    
}
