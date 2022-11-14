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
    public List<GameObject> moveSpots;
    Transform target;
    public bool isShooting; // debug solution for now - pseudo shooting state

    void Start()
    {
        aids = GetComponent<AIDestinationSetter>();
        moveSpots = GameObject.FindGameObjectsWithTag("FarmerPatrolSpot").ToList();

        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Move()
    {
        while (true)
        {
            if (!isShooting)
            {
                int randomSpot = Random.Range(0, moveSpots.Count);
                target = moveSpots[randomSpot].transform;

                while (target != null && Vector2.Distance(transform.position, target.position) > 0.8f)
                {
                    aids.target = target;
                    yield return null;
                }
            }
            yield return new WaitForSeconds(3f);
        }
    }

    public void PlayerLastSeen()
    {
        
    }
}
