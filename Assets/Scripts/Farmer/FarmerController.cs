using Pathfinding;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class FarmerController : MonoBehaviour
{
    public float maxSpeed = 2;
    

    public FarmerBaseState currentState;
    public FarmerPatrollingState patrollingState = new FarmerPatrollingState();
    public FarmerConcernedState concernedState = new FarmerConcernedState();
    public FarmerShootingState shootingState = new FarmerShootingState();
    
    public bool isWaiting = false;
    public bool canSeePlayer = false;
    public Vector3 lastPositionOfInterest;

    private Animator animator;
    private GameManager gameManager;

    void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();

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

    #region Timer
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
    #endregion

    public void PlayerHid()
    {
        if (!currentState.Equals(concernedState))
            SwitchState(concernedState);

        canSeePlayer = false;
        animator.SetBool("playerIsSeen", false);
        GetComponent<AIDestinationSetter>().target = null;
    }
    public void PlayerSeen(Vector3 lastSeenPosition)
    {
        if(!currentState.Equals(shootingState))
            SwitchState(shootingState);

        if (!gameManager.huntersCounterOn && !gameManager.huntersArrived)
        {
            gameManager.huntersCounterOn = true;
            StartCoroutine(gameManager.HuntersCounter());
        }

        lastPositionOfInterest = lastSeenPosition;
        canSeePlayer = true;
        animator.SetBool("playerIsSeen", true);
    }
    public void PlayerInRangeNoSee()
    {
        canSeePlayer = false;
        animator.SetBool("playerIsSeen", false);
        GetComponent<AIDestinationSetter>().target = null;
    }
}
