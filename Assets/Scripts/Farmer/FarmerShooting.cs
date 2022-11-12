using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FarmerShooting : MonoBehaviour
{
    public float stoppingDistance;
    public float timeBtwShots = 3f;
    public float startTimeBtwShots = 3f;

    Transform player;
    public GameObject projectile;
    public Transform riflePoint;
    AIPath aiPath;
    public GameObject crosshair;
    FarmerFOV farmerFOV;
    bool isAiming;
    Animator animator;
    public string currentState;
    AIDestinationSetter aids;
    public float aimingTime = 1f;
    
    // string SHOOT_UP_FARMER = "Shoot_Up_Farmer";
    // string SHOOT_RIGHT_FARMER = "Shoot_Right_Farmer";
    // string SHOOT_DOWN_FARMER = "Shoot_Down_Farmer";
    // string SHOOT_LEFT_FARMER = "Shoot_Left_Farmer";
    // string IDLE_LEFT_FARMER = "Idle_Left_Farmer";
    // string IDLE_RIGHT_FARMER = "Idle_Right_Farmer";
    // string IDLE_UP_FARMER = "Idle_Up_Farmer";
    // string IDLE_DOWN_FARMER = "Idle_Down_Farmer";

    
    void Start()
    {
        aiPath = GetComponent<AIPath>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        farmerFOV = GetComponent<FarmerFOV>();
        aids = GetComponent<AIDestinationSetter>();
        animator = GetComponent<Animator>();
    }

    
    void FixedUpdate()
    {
        // ref for current movespeed
         if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
          {aiPath.maxSpeed = 2f;}
          
        

        if (Vector2.Distance(transform.position, player.position) < stoppingDistance && farmerFOV.canSeePlayer)
                   
          {
            aids.target = player;
            aiPath.maxSpeed = 0.01f;
            StartCoroutine(Aim());
            
            
            
          }
      

        
      

       if(timeBtwShots > 0){
            
            timeBtwShots -= Time.deltaTime;
        }


    }

    public void Shoot()
    {
       
        
        
       riflePoint = farmerFOV.viewPoint;
       
       var b = Instantiate (projectile, riflePoint.position, Quaternion.identity);
       b.GetComponent<Bullet>().target = player.position;
       timeBtwShots = startTimeBtwShots;
       
       WhichWayShoot();
    //    ChangeAnimState("Idle");
       //WhichWayIdle();
       // idle anim
       
       
        
        
    }

     IEnumerator Aim()
     {
        yield return new WaitForSeconds(aimingTime);
        if (timeBtwShots <= 0)
        {
        WhichWayShoot();
        if (Vector2.Distance(transform.position, player.position)
                                     < stoppingDistance && farmerFOV.canSeePlayer)
        animator.SetTrigger("aimed");
        //Shoot(); // actually trigger animator and change state
     }}
    void WhichWayShoot()
    {
      farmerFOV.WhereIsFarmerLooking();
      switch (farmerFOV.viewPointString)
      {
        case "up": animator.SetFloat("lastVertical", 1f);
        
        break;
        case "right": animator.SetFloat("lastHorizontal", 1f);
        
        break;
        case "down": animator.SetFloat("lastVertical", -1f);
        
        break;
        case "left": animator.SetFloat("lastHorizontal", -1f);
        
        break;
      }
    }

    // void WhichWayIdle()
    // {
    //     farmerFOV.WhereIsFarmerLooking();
    //     switch (farmerFOV.viewPointString)
    //   {
    //     case "up": ChangeAnimState(IDLE_UP_FARMER);
        
    //     break;
    //     case "right": ChangeAnimState(IDLE_RIGHT_FARMER);
        
    //     break;
    //     case "down": ChangeAnimState(IDLE_DOWN_FARMER);
        
    //     break;
    //     case "left": ChangeAnimState(IDLE_LEFT_FARMER);
        
    //     break;
    //   }
    // }

    // void ChangeAnimState(string newState)
    // {
    //     //stop the same anim from interrupting itself
    //   if (currentState == newState)
    //   return;
    //   // if shooting, create crosshair
    //   if (newState == SHOOT_UP_FARMER || newState == SHOOT_DOWN_FARMER || newState == SHOOT_RIGHT_FARMER
    //   || newState == SHOOT_LEFT_FARMER)
    //   {
    //   Instantiate (crosshair, player.position, Quaternion.identity);
    //   }

    //   animator.Play(newState);
      
    //   //reassign the current state
    //   currentState = newState;
    // }
}
