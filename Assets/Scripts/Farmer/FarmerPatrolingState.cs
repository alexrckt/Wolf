using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class FarmerPatrollingState : FarmerBaseState
{
    LinkedList<GameObject> moveSpots;
    private LinkedListNode<GameObject> currentSpot;
    private AIPath farmerAIPath;
    private FarmerController farmer;

    public bool afterContact = false;

    public FarmerPatrollingState() : base("Patrolling")
    {
        waitTimeMin = 3f;
        waitTimeMax = 3f;
    }

    public override void EnterState(FarmerController farmer)
    {
        farmerAIPath = farmer.GetComponent<AIPath>();
        this.farmer = farmer;

        if (farmer.hasSpecificRoute)
        {
            moveSpots = farmer.moveSpotsLinked;
            currentSpot = GetNextSpot(farmer.moveSpotsLinked.First);
        } else
        {
            var patrolSpotTagName = "FarmerPatrolSpot";

            if (farmer.agitated)
            {
                patrolSpotTagName = "FarmerAgitatedPatrolSpot";
                waitTimeMin = waitTimeMax = 0f;
            }

            moveSpots = new LinkedList<GameObject>(GameObject.FindGameObjectsWithTag(patrolSpotTagName));
            currentSpot = GetNextSpot();
        }

        farmerAIPath.maxSpeed = farmer.maxSpeed;
    }

    public override void UpdateState(FarmerController farmer)
    {
        if (afterContact)
        {
            farmerAIPath.destination = farmer.transform.position;
            waitTimeMax = waitTimeMin = farmer.waitOnLost;
            farmer.SetWaitTimer();
            afterContact = false;
        }

        if (!farmer.isWaiting)
        {
            if (!Move(farmer.transform, currentSpot?.Value.transform))
            {
                currentSpot = farmer.hasSpecificRoute ? GetNextSpot(currentSpot) : GetNextSpot();
                farmer.SetWaitTimer();
            }
        }
    }

    public override void OnCollisionEnter(FarmerController farmer)
    {

    }

    public LinkedListNode<GameObject> GetNextSpot()
    {
        return moveSpots.Find(moveSpots.ToList<GameObject>()[Random.Range(0, moveSpots.Count)]);
    }
    public LinkedListNode<GameObject> GetNextSpot(LinkedListNode<GameObject> spot)
    {
        return spot == null ? farmer.moveSpotsLinked.First : spot.Next;
    }
}
