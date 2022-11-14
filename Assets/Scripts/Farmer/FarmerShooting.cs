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
    FarmerController farmerController;
    private AnimatorUpdater animatorUpdater;
    //bool isAiming = false;
    Animator animator;
    public string currentState;
    AIDestinationSetter aids;
    public float aimingTime = 1f;
    Vector2 crossHairPlayerPos;
    public Vector2 whichWayPlayer;
    public Transform[] gunPoints; // NESW
    
    
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
        animatorUpdater = GetComponent<AnimatorUpdater>();
        farmerController = GetComponent<FarmerController>();
    }

    
    void Update()
    { 
        whichWayPlayer =  player.position - transform.position;

        if (!farmerFOV.canSeePlayer)   // implement fov state - if just seen and hidden from sight but 
                                        // still in  range - stand until the player gets out of range
         {
             aiPath.maxSpeed = 2f;
             //aiPath.canMove = true;
             farmerController.isShooting = false;
             farmerFOV.angle = 180f;
         }



        if ( farmerFOV.canSeePlayer  )
        {

            farmerController.isShooting = true;
            aids.target = player;
            aiPath.maxSpeed = 0.01f;
            //aiPath.canMove = false;
            farmerFOV.angle = 360f;
            if (timeBtwShots <= 0)
            {

                Instantiate(crosshair, player.position, Quaternion.identity);
                crossHairPlayerPos = player.position;

                WhichWayShoot(); // tell anim which way to look

                animator.SetTrigger("aimed"); // start animating the shot
                timeBtwShots = startTimeBtwShots;
            }
        }

        if(timeBtwShots > 0)
        {
            timeBtwShots -= Time.deltaTime;
        }
    }

    public void Shoot()
    {
        farmerFOV.WhereIsFarmerLooking();
        switch (farmerFOV.viewPointString)
        {
            case "up":
                riflePoint = gunPoints[0];
                break;
            case "right":
                riflePoint = gunPoints[1];
                break;
            case "down":
                riflePoint = gunPoints[2];
                break;
            case "left":
                riflePoint = gunPoints[3];
                break;
        }



        var b = Instantiate (projectile, riflePoint.position,Quaternion.LookRotation(Vector3.forward, new Vector2(player.position.x, player.position.y)));
        // ne rabotaet!
        b.GetComponent<Bullet>().SetPlayerPos(crossHairPlayerPos);

        WhichWayShoot();
    }

     
    void WhichWayShoot()
    {
        switch (animatorUpdater.lastMotionVector.x)
        {
            case 1:
                animator.SetFloat("lastHorizontal", 1f);
                break;
            case -1:
                animator.SetFloat("lastHorizontal", -1f);
                break;
            default:
                animator.SetFloat("lastHorizontal", 0);
                break;
        }

        switch (animatorUpdater.lastMotionVector.y)
        {
            case 1:
                animator.SetFloat("lastVertical", -1f);
                break;
            case -1:
                animator.SetFloat("lastVertical", 1f);
                break;
            default:
                animator.SetFloat("lastVertical", 0);
                break;
        }
    }

    
}
