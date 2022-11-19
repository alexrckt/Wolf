using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class FarmerPatrollingState : FarmerBaseState
{
    List<GameObject> moveSpots;
    private Transform currentRandomSpot;

    public bool afterContact = false;

    public FarmerPatrollingState() : base("Patrolling")
    {
        waitTimeMin = 3f;
        waitTimeMax = 3f;
    }

    public override void EnterState(FarmerController farmer)
    {
        var patrolSpotTagName = "FarmerPatrolSpot";

        if (farmer.agitated)
        {
            patrolSpotTagName = "FarmerAgitatedPatrolSpot";
            waitTimeMin = waitTimeMax = 0f;
        }

        moveSpots = GameObject.FindGameObjectsWithTag(patrolSpotTagName).ToList();

        currentRandomSpot = GetRandomSpot();
    }

    public override void UpdateState(FarmerController farmer)
    {
        if (afterContact)
        {
            farmer.GetComponent<AIPath>().maxSpeed = 0.01f;
            waitTimeMax = waitTimeMin = farmer.waitOnLost;
            farmer.SetWaitTimer();
            afterContact = false;
        }

        if (!farmer.isWaiting)
        {
            farmer.GetComponent<AIPath>().maxSpeed = farmer.maxSpeed;
            if (!Move(farmer.transform, currentRandomSpot))
            {
                currentRandomSpot = GetRandomSpot();
                farmer.SetWaitTimer();
            }
        }
    }

    public override void OnCollisionEnter(FarmerController farmer)
    {

    }

    public Transform GetRandomSpot()
    {
        return moveSpots[Random.Range(0, moveSpots.Count)].transform;
    }
}
