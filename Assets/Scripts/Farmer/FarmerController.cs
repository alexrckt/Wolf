using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using Random = UnityEngine.Random;
using System.Linq;

public class FarmerController : MonoBehaviour
{

    
    AIDestinationSetter aids;
    AIPath aiPath;
    Animator animator;
    public enum State
    {
        Calm,
        Sniffing,
        Chasing,
        LostTarget
    }
    public State currentState;
    GameObject playerRef;
    Vector2 lastMotionVector;
    public bool moving;
    public float horizontal;
    public float vertical;
    public Vector2 debugVelocity;
    
    public List<GameObject> moveSpots;
    Transform target;

    
    // Start is called before the first frame update
    void Start()
    {
        
        aiPath = GetComponent<AIPath>();
        aids = GetComponent<AIDestinationSetter>();
        animator = GetComponent<Animator>();
        playerRef = GameObject.FindGameObjectWithTag("Player");
        moveSpots = GameObject.FindGameObjectsWithTag("FarmerPatrolSpot").ToList();

        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
        debugVelocity = aiPath.velocity;
        #region Animator update
        horizontal = aiPath.desiredVelocity.x;
        vertical = aiPath.desiredVelocity.y;
        AbsVectors(); // compares if the agent is going more vertically or horizontally
                      // to decide which anim is more sutiable
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
       
    }


IEnumerator Move(){
        while (true)
        {
            int randomSpot = Random.Range(0, moveSpots.Count);
            target = moveSpots[randomSpot].transform;

            while(target != null && Vector2.Distance(transform.position, target.position) > 0.8f)
            {
                aids.target = target;
                yield return null;
            }

            yield return new WaitForSeconds(3f);
        }
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

    public void AbsVectors()
    {
        if (Math.Abs(vertical) > Math.Abs(horizontal))
        {
            horizontal = 0f;
        }
        else 
        {
            vertical = 0f;
        }
       
    }

    public void PlayerLastSeen()
    {
        
    }
}
