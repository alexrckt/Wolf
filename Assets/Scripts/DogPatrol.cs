using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class DogPatrol : MonoBehaviour
{
    public enum State
    {
        Calm,
        Sniffing,
        Chasing,
        LostTarget
    }

    GameObject playerRef;
    public float waitTimeMin;
    public float waitTimeMax;
    public float waitTimeSniffMin;
    public float waitTimeSniffMax;
    public string stateDebug;

    public State CurrentState { get; set; }
    private LinkedListNode<GameObject> wolfFootstep;
    //Vector2 lastMotionVector;
    public Transform[] moveSpots;
    private int randomSpot;
    AIDestinationSetter aids;
    AIPath aiPath;
    Animator animator;
    bool moving;
    public float horizontal;
    public float vertical;
    Vector2 lastPlayerPos;
    
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        CurrentState = State.Calm;
        aiPath = GetComponent<AIPath>();
        aids = GetComponent<AIDestinationSetter>();
        animator = GetComponentInChildren<Animator>();
        StartCoroutine(Move());
    }

    
    void Update()
    {
        switch (CurrentState)
        {
            case State.Calm:
            stateDebug = "Calm";
            return;
            case State.Chasing:
            stateDebug = "Chasing";
            return;
            case State.Sniffing:
            stateDebug = "Sniffin";
            return;
            case State.LostTarget:
            stateDebug = "Lost";
            return;
        }
        horizontal = aiPath.desiredVelocity.x;
        vertical = aiPath.desiredVelocity.y;

        NormalizeMoveDest();

        animator.SetFloat("horizontal", horizontal);
        animator.SetFloat("vertical", vertical);
        
        moving = horizontal != 0 || vertical != 0;
        animator.SetBool("moving", moving);

        if (horizontal != 0 || vertical != 0)
        {
            //lastMotionVector = new Vector2(horizontal, vertical).normalized;
            animator.SetFloat("lastHorizontal", horizontal);
            animator.SetFloat("lastVertical", vertical);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("WolfFootstep") 
            && CurrentState != State.Chasing)
        {
            SetSniffingState(other);
        }
    }

    void SetSniffingState(Collider2D step)
    {
        if (CurrentState != State.Sniffing)
        {
            CurrentState = State.Sniffing;
            aiPath.maxSpeed *= 2; // need var to store normal / sniffing / chasing speeds
        }
        wolfFootstep = WolfController.Footsteps.Find(step.gameObject);
    }

     IEnumerator Move(){
        while (true)
        {
            // if calm do random check
            // if sniffing follow the path
            // if chasing follow the wolf
            Transform target;

            if (CurrentState == State.Calm)
            {
                randomSpot = Random.Range(0, moveSpots.Length);
                target = moveSpots[randomSpot];
            }
             else if (CurrentState == State.Sniffing)
            {
                target = wolfFootstep.Next.Value.transform;
                
            }
            else if (CurrentState == State.LostTarget)
            {
                target = null;
                aiPath.destination = lastPlayerPos;
                if (aiPath.reachedDestination)
                {CurrentState = State.Calm;}
            }
             else
            {
                target = playerRef.transform; // tut budet volk
            }

            var initialState = CurrentState;
            while(Vector2.Distance(transform.position, target.position) > 0.2f
                  && initialState == CurrentState)
            {
                
                aids.target = target;
                yield return null;
            }
            yield return new WaitForSeconds(GetWaitTime());
        }
     }

     private float GetWaitTime()
     {
         float min = 0;
         float max = 0;

         switch (CurrentState)
         {
            case State.Calm:
                min = waitTimeMin;
                max = waitTimeMax;
                break;
            case State.Sniffing:
                min = waitTimeSniffMin;
                max = waitTimeSniffMax;
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


    public void PlayerLastSeen()
    {
         
          lastPlayerPos = playerRef.transform.position;
         
         
       if (CurrentState == State.Chasing)
         {CurrentState = State.LostTarget;}
    }
}
