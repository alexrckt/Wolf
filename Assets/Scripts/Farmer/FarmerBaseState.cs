using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FarmerBaseState
{
    public string stateName;
    public float waitTimeMin = 0f;
    public float waitTimeMax = 0f;

    protected FarmerBaseState(string name)
    {
        stateName = name;
    }

    public abstract void EnterState(FarmerController farmer);

    public abstract void UpdateState(FarmerController farmer);

    public abstract void OnCollisionEnter(FarmerController farmer);

    public bool Move(Transform actor, Transform target)
    {
        if (target != null && Vector2.Distance(actor.position, target.position) > 0.8f)
        {
            actor.GetComponent<AIDestinationSetter>().target = target;
            return true;
        }
        return false;
    }

    public void EnterStateLog()
    {
        Debug.Log($"Entering {stateName} state");
    }
}
