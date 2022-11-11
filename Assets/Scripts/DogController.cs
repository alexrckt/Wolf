using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pathfinding;
using System.Runtime.Serialization;
using System.Text;
using UnityEditorInternal;
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

    GameObject playerRef;

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
    Animator animator;
    bool moving;
    public float horizontal;
    public float vertical;
    
    public GameObject debugTar;
    GameObject lastPlayerPosTarget;
    Transform target;
    
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        aiPath = GetComponent<AIPath>();
        aids = GetComponent<AIDestinationSetter>();
        animator = GetComponentInChildren<Animator>();
        moveSpots = GameObject.FindGameObjectsWithTag("DogPatrolSpot").ToList();

        SetCalmState();
        StartCoroutine(Move());

        //InvokeRepeating("Debugging", 1f, 1f);
    }

    void Update()
    {
        #region Animator update
        horizontal = aiPath.desiredVelocity.x;
        vertical = aiPath.desiredVelocity.y;

        NormalizeMoveDest();

        animator.SetFloat("horizontal", horizontal);
        animator.SetFloat("vertical", vertical);
        
        moving = horizontal != 0 || vertical != 0;
        animator.SetBool("moving", moving);

        if (horizontal != 0 || vertical != 0)
        {
            
            animator.SetFloat("lastHorizontal", horizontal);
            animator.SetFloat("lastVertical", vertical);
        }
        #endregion
         // if (currentState == State.Sniffing && target != null) // CRUTCH
                // {Vector2.MoveTowards(transform.position, target.position, maxSpeedSniffing * Time.deltaTime);}
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
            SetSniffingState(other);
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
        
        currentState = State.Chasing;
        aiPath.maxSpeed = maxSpeedChasing;
    }

    public void SetLostTargetState()
    {
        
        currentState = State.LostTarget;
        aiPath.maxSpeed = maxSpeedLostTarget;
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
                target = lastPlayerPosTarget.transform;
                
                if (aiPath.reachedDestination)
                    SetCalmState();
            }
            else
            {
                target = playerRef.transform; // tut budet volk
            }
            

            var initialState = currentState;
            while(target != null && Vector2.Distance(transform.position, target.position) > 0.5f
                  && initialState == currentState )
            {
                aids.target = target;
                yield return null;
            }

            //Timer
            var duration = GetWaitTime();
            while (duration >= 0
                   && currentState != State.Chasing)
            {
                duration -= Time.deltaTime;
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

    public void NormalizeMoveDest()
    {

        if (horizontal > 0)
        {
            horizontal = 1f;
        }
        else if (horizontal < 0)
        {
            horizontal = -1f;
        }

        if (vertical > 0)
        {
            vertical = 1f;
        }
        else if (vertical < 0)
        {
            vertical = -1f;
        }
    }
}
