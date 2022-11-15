using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerShootingState : FarmerBaseState
{
    public FarmerShootingState() : base("Shooting") { }

    public override void EnterState(FarmerController farmer)
    {
        farmer.GetComponent<AIPath>().maxSpeed = 0.01f;
    }

    public override void UpdateState(FarmerController farmer)
    {
        farmer.GetComponent<FarmerShooting>().Shooting();
    }

    public override void OnCollisionEnter(FarmerController farmer)
    {

    }
}
