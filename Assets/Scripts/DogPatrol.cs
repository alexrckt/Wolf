using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class DogPatrol : MonoBehaviour
{
    public float speed;
    public float waitTimeMin;
    public float waitTimeMax;
    
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

     IEnumerator Move(){
        while (true)
        {
            
            randomSpot = Random.Range(0, moveSpots.Length);
            while(Vector2.Distance(transform.position, moveSpots[randomSpot].position) > 0.2f)
            {
                //transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);
                aids.target = moveSpots[randomSpot];
                yield return null;
            }
            yield return new WaitForSeconds(Random.Range(waitTimeMin, waitTimeMax));
            
        }
        
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
