using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pathfinding;
using Random = UnityEngine.Random;

public class DogController : MonoBehaviour
{
    public enum State
    {
        Calm,
        Sniffing,
        Chasing,
        LostTarget
    }

    WolfController playerRef;

    public float maxSpeedCalm;
    public float maxSpeedSniffing;
    public float maxSpeedChasing;
    public float maxSpeedLostTarget;
    public float waitTimeMin;
    public float waitTimeMax;
    public float waitTimeSniffMin;
    public float waitTimeSniffMax;
    public float waitTimeLostTargetMin;
    public float waitTimeLostTargetMax;

    public State currentState;
    private LinkedListNode<FootStepFade> wolfFootstep;
    //Vector2 lastMotionVector;
    //public Transform[] moveSpots;
    public List<GameObject> moveSpots;
    private int randomSpot;
    AIDestinationSetter aids;
    AIPath aiPath;
    private Barker barker;
    private float TimeInterval;
    private float timerDuration;

    public GameObject debugTar;
    GameObject lastPlayerPosTarget;
    Transform target;
    public Transform footstepParent;

    public delegate void OnAttention(Transform wolfPosition);
    public static OnAttention onAttention;
    private SoundManager soundManager;

    void Start()
    {
        playerRef = FindObjectOfType<WolfController>();
        aiPath = GetComponent<AIPath>();
        aids = GetComponent<AIDestinationSetter>();
        barker = GetComponentInChildren<Barker>();
        soundManager = FindObjectOfType<SoundManager>();

        if (moveSpots == null || moveSpots.Count <= 0)
        {
            moveSpots = GameObject.FindGameObjectsWithTag("DogPatrolSpot").ToList();
        }

        barker.AnimationSwitch(false);
        SetCalmState();
        StartCoroutine(Move());

        onAttention += RunToPosition;

        //InvokeRepeating("Debugging", 1f, 1f);
    }

    private void OnDestroy()
    {
        onAttention -= RunToPosition;
    }

    void Update()
    {

    }

    void Debugging()
    {
        var aiPath = GetComponent<AIPath>();
        Debug.Log($"{aiPath.remainingDistance} {aiPath.hasPath} {aiPath.velocity}");
    }

    #region State switch
    // Switching to CALM state on Awake AND after timeout in LOSTTARGET state
    // Switching to SNIFFING state IF find wolf steps AND NOT in CHASING state
    // Switching to CHASING state IF wolf appear in raycast field
    // Switching to LOSTTARGET state IF wolf disappear in raycast field
    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.tag.Equals("WolfFootstep") 
            && currentState != State.Chasing
            && currentState != State.LostTarget)
        {
            soundManager.PlaySniffing();
            SetSniffingState(other);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            // BITE THE WOLF
            soundManager.PlayDogBite();
            playerRef.WolfInjured();
        }
    }
    public void SetSniffingState(Collider2D step)
    {
        currentState = State.Sniffing;
        aiPath.maxSpeed = maxSpeedSniffing;
        

        var newStep = WolfController.Footsteps.Find(step.GetComponent<FootStepFade>());

        if (wolfFootstep != null)
        {
            wolfFootstep = (newStep.Value.lifeTime > wolfFootstep.Value.lifeTime) ? newStep : wolfFootstep;
        }
        else
            wolfFootstep = newStep;
    }

    public void SetCalmState()
    {
        
        currentState = State.Calm;
        aiPath.maxSpeed = maxSpeedCalm;
    }

    public void SetChasingState()
    {
        soundManager.PlayBark();
        currentState = State.Chasing;
        aiPath.maxSpeed = maxSpeedChasing;
        barker.AnimationSwitch(true);
        if(FarmerController.onHearBarking != null)
        {
            FarmerController.onHearBarking();
        }

        onAttention(playerRef.transform); // for other dog instances
    }

    public void SetLostTargetState()
    {
        currentState = State.LostTarget;
        aiPath.maxSpeed = maxSpeedLostTarget;
        barker.AnimationSwitch(false);
    }
    #endregion

    IEnumerator Move(){
        while (true)
        {
            // if calm do random check
            // if sniffing follow the path
            // if chasing follow the wolf
            // if losttarget go to last visible point, wait, then calm

            if (currentState == State.Calm)
            {
                randomSpot = Random.Range(0, moveSpots.Count);
                target = moveSpots[randomSpot].transform;
            }
            else if (currentState == State.Sniffing)
            {
                try
                {
                     target = wolfFootstep.Next.Value.transform; // follow the footstep
                }
                catch
                {
                    target = null;  // if there is none left - set calm state
                    SetCalmState();
                }
            }
            else if (currentState == State.LostTarget)
            {
                target = lastPlayerPosTarget?.transform;
                
                if (aiPath.reachedDestination)
                    SetCalmState();
            }
            else
            {
                target = playerRef.transform; // tut budet volk
            }

            var initialState = currentState;
            var initialTarget = target;
            while (target != null
                && initialTarget == target
                && Vector2.Distance(transform.position, target.position) > 0.2f
                && initialState == currentState )
            {
                aids.target = target;
                yield return null;
            }

            //Timer
            timerDuration = GetWaitTime();
            while (timerDuration >= 0
                   && currentState != State.Chasing)
            {
                timerDuration -= Time.deltaTime;
                yield return null;
            }

            //Debug.Log("Iteration of main While in state " + currentState);
            yield return null;
        }
    }

    public void PlayerLastSeen()
    {
        if (currentState == State.Chasing)
        {
            lastPlayerPosTarget = Instantiate(debugTar, playerRef.transform.position, Quaternion.identity);
            lastPlayerPosTarget.transform.SetParent(footstepParent, true);
            SetLostTargetState();
        }
    }

    private float GetWaitTime()
    {
        float min = 0;
        float max = 0;

        switch (currentState)
        {
            case State.Calm:
                min = waitTimeMin;
                max = waitTimeMax;
                break;
            case State.Sniffing:
                min = waitTimeSniffMin;
                max = waitTimeSniffMax;
                break;
            case State.LostTarget:
                min = waitTimeLostTargetMin;
                max = waitTimeLostTargetMax;
                break;
        }

        return Random.Range(min, max);
    }

    void LateUpdate()
    {
        TimeInterval += Time.deltaTime;
    }

    private void RunToPosition(Transform wolfPosition)
    {
        if(currentState != State.Chasing)
        {
            if (TimeInterval >= 1)
            {
                TimeInterval = 0;
                timerDuration = 0;
                aiPath.maxSpeed = maxSpeedChasing;
                lastPlayerPosTarget = Instantiate(debugTar, wolfPosition.position, Quaternion.identity);
                SetLostTargetState();
            }
        }
    }
}
