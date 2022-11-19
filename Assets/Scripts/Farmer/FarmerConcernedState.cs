using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerConcernedState : FarmerBaseState
{
    public FarmerConcernedState() : base("Concerned")
    {
        waitTimeMin = 0f;
        waitTimeMax = 0f;
    }

    public override void EnterState(FarmerController farmer)
    {
        farmer.GetComponent<AIPath>().maxSpeed = farmer.maxSpeed;
    }

    public override void UpdateState(FarmerController farmer)
    {

    }

    public override void OnCollisionEnter(FarmerController farmer)
    {

    }
}
