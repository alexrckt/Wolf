using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerStateManager : MonoBehaviour
{
    public FarmerBaseState currentState;
    private FarmerPatrollingState patrollingState = new FarmerPatrollingState();
    private FarmerConcernedState concernedState = new FarmerConcernedState();
    private FarmerShootingState shootingState = new FarmerShootingState();
    // Start is called before the first frame update
    void Start()
    {
        currentState = patrollingState;

        currentState.EnterState(this);
        currentState.EnteringLog();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(FarmerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
        state.EnteringLog();
    }
}
