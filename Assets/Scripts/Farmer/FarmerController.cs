using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FarmerController : MonoBehaviour
{

    public bool isShooting; // debug solution for now - pseudo shooting state

    public FarmerBaseState currentState;
    public FarmerPatrollingState patrollingState = new FarmerPatrollingState();
    public FarmerConcernedState concernedState = new FarmerConcernedState();
    public FarmerShootingState shootingState = new FarmerShootingState();

    public bool isWaiting = false;
    public bool canSeePlayer = false;

    void Start()
    {
        currentState = patrollingState;
        currentState.EnterState(this);
        currentState.EnterStateLog();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(FarmerBaseState state)
    {
        isWaiting = false;
        currentState = state;
        state.EnterState(this);
        state.EnterStateLog();
    }

    public void SetWaitTimer()
    {
        isWaiting = true;
        StartCoroutine(Timer());
    }
    private IEnumerator Timer()
    {
        var duration = GetWaitTime();
        var state = currentState;
        while (duration >= 0 && state.Equals(currentState))
        {
            duration -= Time.deltaTime;
            yield return null;
        }
        isWaiting = false;
    }

    private float GetWaitTime()
    {
        return Random.Range(currentState.waitTimeMin, currentState.waitTimeMax);
    }

    public void PlayerLastSeen()
    {
        
    }
}
