using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FarmerController : MonoBehaviour
{
    public float maxSpeed = 2f;
    public float waitOnLost = 0f;

    public FarmerBaseState currentState;
    public FarmerPatrollingState patrollingState = new FarmerPatrollingState();
    public FarmerShootingState shootingState = new FarmerShootingState();
    
    public bool isWaiting = false;
    public bool canSeePlayer = false;
    public bool agitated = false;
    public Vector3 lastPositionOfInterest;

    public bool hasSpecificRoute = false;
    public List<GameObject> moveSpots;
    public LinkedList<GameObject> moveSpotsLinked;

    private Animator animator;
    private GameManager gameManager;
    SheepsClothing sheepsClothing;
    EventManager em;

    public delegate void OnHearBarking();
    public static OnHearBarking onHearBarking;

    void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        sheepsClothing = FindObjectOfType<SheepsClothing>();
        em = FindObjectOfType<EventManager>();

        if(hasSpecificRoute)
            moveSpotsLinked = new LinkedList<GameObject>(moveSpots);

        currentState = patrollingState;
        currentState.EnterState(this);
        currentState.EnterStateLog();

        onHearBarking += HeardAlarm;
    }

    private void OnDestroy()
    {
        onHearBarking -= HeardAlarm;
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
        GetComponent<AIDestinationSetter>().target = null;
        if (!currentState.Equals(patrollingState))
        {
            patrollingState.afterContact = true;
            SwitchState(patrollingState);
        }

        canSeePlayer = false;
        sheepsClothing.isSeen = false;
        animator.SetBool("playerIsSeen", false);
    }
    public void PlayerSeen(Vector3 lastSeenPosition)
    {
        agitated = true;
        if(!currentState.Equals(shootingState))
            SwitchState(shootingState);

        if (!gameManager.huntersCounterOn && !gameManager.huntersArrived)
        {
            gameManager.huntersCounterOn = true;
            em.SeenPlayer(); // event for emote
            StartCoroutine(gameManager.HuntersCounter());
        }

        lastPositionOfInterest = lastSeenPosition;
        canSeePlayer = true;
        sheepsClothing.isSeen = true;
        animator.SetBool("playerIsSeen", true);
    }

    public void HeardAlarm()
    {
        if (!currentState.Equals(shootingState) && !agitated)
        {
            agitated = true;
            em.Agitated(); // event for emote
            
            SwitchState(patrollingState);
        }
            
    }
    public void PlayerInRangeNoSee()
    {
        canSeePlayer = false;
        sheepsClothing.isSeen = false;
        animator.SetBool("playerIsSeen", false);
        GetComponent<AIDestinationSetter>().target = null;
    }
}
