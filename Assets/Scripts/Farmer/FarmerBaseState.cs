using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FarmerBaseState
{
    public string stateName;

    protected FarmerBaseState(string name)
    {
        stateName = name;
    }

    public abstract void EnterState(FarmerStateManager farmer);

    public abstract void UpdateState(FarmerStateManager farmer);

    public abstract void OnCollisionEnter(FarmerStateManager farmer);

    public void EnteringLog()
    {
        Debug.Log($"Entering {stateName} state");
    }
}
