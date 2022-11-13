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
    FarmerController fc;
    //bool isAiming = false;
    Animator animator;
    public string currentState;
    AIDestinationSetter aids;
    public float aimingTime = 1f;
    Vector2 crossHairPlayerPos;
    public Vector2 whichWayPlayer;
    
    
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
        fc = GetComponent<FarmerController>();
    }

    
    void Update()
    {
        whichWayPlayer =  player.position - transform.position;

        
         if (!farmerFOV.canSeePlayer) // implement fov state - if just seen and hidden from sight but 
         
                                     // still in  range - stand until the player gets out of range
          {aiPath.maxSpeed = 2f;
          //aiPath.canMove = true;
            fc.isShooting = false; 
            farmerFOV.angle = 180f;}
          
        

        if ( farmerFOV.canSeePlayer  )
                   
          {

            fc.isShooting = true;
            aids.target = player;
            aiPath.maxSpeed = 0.01f;
            //aiPath.canMove = false;
            farmerFOV.angle = 360f;
            if ( timeBtwShots <= 0)
          {
            
            Instantiate (crosshair, player.position, Quaternion.identity);
            crossHairPlayerPos = player.position;
            
            

           WhichWayShoot(); // tell anim which way to look
        
            animator.SetTrigger("aimed"); // start animating the shot
            timeBtwShots = startTimeBtwShots;
          }
            
            
            
          }
      

        
      

       if(timeBtwShots > 0){
            
            timeBtwShots -= Time.deltaTime;
        }


    }

    public void Shoot()
    {
       
        
        
       riflePoint = farmerFOV.viewPoint;
       
       var b = Instantiate (projectile, riflePoint.position,Quaternion.LookRotation(Vector3.forward, fc.lastMotionVector ));
       // ne rabotaet!
       b.GetComponent<Bullet>().SetPlayerPos(crossHairPlayerPos);
       
       
       WhichWayShoot();
        
    }

     
    void WhichWayShoot()
    {
      
      switch (fc.lastMotionVector.x)
      {
       
        case 1: animator.SetFloat("lastHorizontal", 1f);
        
        break;
       
        case -1: animator.SetFloat("lastHorizontal", -1f);
        
        break;
        default: animator.SetFloat("lastHorizontal", 0);
        
        break;
         
      }

      switch (fc.lastMotionVector.y)
      {
        case 1: animator.SetFloat("lastVertical", -1f);
        
        break;
         case -1: animator.SetFloat("lastVertical", 1f);
        
        break;
        default: animator.SetFloat("lastVertical", 0);
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
    //   
    //   }

    //   animator.Play(newState);
      
    //   //reassign the current state
    //   currentState = newState;
    // }
}
