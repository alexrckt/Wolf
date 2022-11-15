using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerConcernedState : FarmerBaseState
{
    public FarmerConcernedState() : base("Concerned")
    {
        waitTimeMin = 3f;
        waitTimeMax = 3f;
    }

    public override void EnterState(FarmerController farmer)
    {
        farmer.GetComponent<AIPath>().maxSpeed = farmer.maxSpeed;
    }

    public override void UpdateState(FarmerController farmer)
    {
        if (!Move(farmer.transform, farmer.lastPositionOfInterest))
        {
            // What to do next?
            farmer.SetWaitTimer();
        }
    }

    public override void OnCollisionEnter(FarmerController farmer)
    {

    }
}
