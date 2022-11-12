using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 50f;

    
    public Vector2 target;
    
    void Start()
    {
        
        
    }

   
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (transform.position.x == target.x && transform.position.y == target.y)
        {
            ExplodeProjectile();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
     {
        if (other.CompareTag("Player")) // OR other.CompareTag("WALL")
     {
       // gameover
       ExplodeProjectile();
     }
     if (other.CompareTag("Obstacle"))
     {
        //Getcomponent "ovechka" - kill ovechka if so
        ExplodeProjectile();
     }
    }
    private void ExplodeProjectile()
    {
        Destroy(gameObject);
        // visuals?
    }
}
