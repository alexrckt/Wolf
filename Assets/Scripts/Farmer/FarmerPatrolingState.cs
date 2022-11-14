using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class FarmerPatrollingState : FarmerBaseState
{
    List<GameObject> moveSpots;
    private Transform currentRandomSpot;

    public FarmerPatrollingState() : base("Patrolling")
    {
        waitTimeMin = 3f;
        waitTimeMax = 3f;
    }

    public override void EnterState(FarmerController farmer)
    {
        moveSpots ??= GameObject.FindGameObjectsWithTag("FarmerPatrolSpot").ToList();

        currentRandomSpot = GetRandomSpot();
    }

    public override void UpdateState(FarmerController farmer)
    {
        if (!farmer.isWaiting && !Move(farmer.transform, currentRandomSpot))
        {
            currentRandomSpot = GetRandomSpot();
            farmer.SetWaitTimer();
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
