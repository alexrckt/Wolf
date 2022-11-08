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
        Chasing
    }

    public float speed;
    public float waitTimeMin;
    public float waitTimeMax;
    public float waitTimeSniffMin;
    public float waitTimeSniffMax;

    public State CurrentState { get; set; }
    private LinkedListNode<GameObject> wolfFootstep;
    Vector2 lastMotionVector;
    public Transform[] moveSpots;
    private int randomSpot;
    AIDestinationSetter aids;
    AIPath aiPath;
    Animator animator;
    bool moving;
    public float horizontal;
    public float vertical;
    // Start is called before the first frame update
    void Start()
    {
        CurrentState = State.Calm;
        aiPath = GetComponent<AIPath>();
        aids = GetComponent<AIDestinationSetter>();
        animator = GetComponentInChildren<Animator>();
        StartCoroutine(Move());
    }

    
    void Update()
    {
        horizontal = aiPath.desiredVelocity.x;
        vertical = aiPath.desiredVelocity.y;

        NormalizeMoveDest();

        animator.SetFloat("horizontal", horizontal);
        animator.SetFloat("vertical", vertical);
        
        moving = horizontal != 0 || vertical != 0;
        animator.SetBool("moving", moving);

        if (horizontal != 0 || vertical != 0)
        {
            lastMotionVector = new Vector2(horizontal, vertical).normalized;
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
            aiPath.maxSpeed *= 2;
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
            } else if (CurrentState == State.Sniffing)
            {
                target = wolfFootstep.Next.Value.transform;
            } else
            {
                target = moveSpots[randomSpot]; // тут будет волк
            }

            var initialState = CurrentState;
            while(Vector2.Distance(transform.position, target.position) > 0.2f
                  && initialState == CurrentState)
            {
                //transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);
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

    void NormalizeMoveDest()
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
