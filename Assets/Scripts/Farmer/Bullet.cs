using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 50f;
      
    
    public Vector2 target;
    Collider2D col;
    float lifeTime = 4f;
    bool hasHitStuff = false;
    void Start()
    {
        
        
    }

   
    void Update()
    {
        if (!hasHitStuff)
        {transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);}

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }

        if (transform.position.x == target.x && transform.position.y == target.y && !hasHitStuff)
        {
            ExplodeProjectile();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
     {
        if (other.CompareTag("Player")) // OR other.CompareTag("WALL")
     {
       other.GetComponent<WolfController>().WolfInjured();
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
        hasHitStuff = true;
       GetComponent<Collider2D>().enabled = false;
       GetComponent<SpriteRenderer>().enabled = false;
       
        
        // visuals?
    }

    public void SetPlayerPos(Vector2 tar)
    {
       target = tar;
    }
}
