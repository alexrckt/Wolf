using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerFieldOfView : MonoBehaviour
{
    [SerializeField] private float radius;
    public float closeradius;
    [Range (0, 360)]
    private float angle;
    [SerializeField] private float currentAngle;
    public Transform fovPoint;
    public GameObject playerRef;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public GameObject[] fovDirs;
    WolfController wc;
    FarmerController farmerController;
    AnimatorClipInfo[] clipAnimArray;
    Vector3 fovDir = new Vector3();
    public Transform viewPoint; // which way we are looking - one of four "vision point" objects
    public string viewPointString;
    private AnimatorUpdater animatorUpdater;
    private Vector3 lastSeenPlayerPosition;

    public List<Collider2D> resultsFarRange;
    public List<Collider2D> resultsCloseRange;
    private ContactFilter2D contactFilter;
    private GameManager gameManager;
    [SerializeField] FOVVisual fovVisual;
    [SerializeField] FOVVisual fovVisualStealth;
    public bool iAmAHunter = false;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerRef = GameObject.FindGameObjectWithTag("Player");
        wc = playerRef.GetComponent<WolfController>();
        farmerController = GetComponent<FarmerController>();
        animatorUpdater = GetComponent<AnimatorUpdater>();
        contactFilter = new ContactFilter2D
        {
            layerMask = playerMask,
            useLayerMask = true
        };

        EventManager.OnStealth += WolfStealthed;
        EventManager.OnStealthFinish += WolfUnstealthed;
        radius = gameManager.GetFOVRadius(gameObject);
        angle = gameManager.GetFOVAngle(gameObject);
        currentAngle = angle;
        if (fovVisual != null)
        {
        fovVisual.SetAngleAndRadius(currentAngle, radius);
        fovVisualStealth.SetAngleAndRadius(currentAngle, closeradius);
        }

        StartCoroutine(SensesRoutine());
    }

    
    void Update()
    {
        
    }

     private void OnDisable() {
        EventManager.OnStealth -= WolfStealthed;
        EventManager.OnStealthFinish -= WolfUnstealthed;
    }
    private IEnumerator SensesRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            SensesCheck();
        }
    }

    private void SensesCheck()
    {
        Physics2D.OverlapCircle(transform.position, radius, contactFilter, resultsFarRange);
        Physics2D.OverlapCircle(transform.position, closeradius, contactFilter, resultsCloseRange);
        WhereIsFarmerLooking();

        if (resultsCloseRange.Count != 0)
        {
            AngleViewCheck(resultsCloseRange);
        }
        else if (resultsFarRange.Count != 0 && !wc.isStealthed)
        {
            AngleViewCheck(resultsFarRange);
        }
        else if (resultsCloseRange.Count == 0 
                 && resultsFarRange.Count == 0
                 && farmerController.canSeePlayer) // if player was in view angle but got away from it
        {
            farmerController.PlayerHid();
        }
    }

    private void AngleViewCheck(List<Collider2D> results)
    {
        WhereIsFarmerLooking();
        Transform target = results[0].transform;
        Vector2 directionToTarget = (target.position - transform.position).normalized;

        if (Vector2.Angle(fovDir, directionToTarget) < currentAngle / 2) // if player in view angle
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.position);
            if (!Physics2D.Raycast(transform.position, directionToTarget,
                    distanceToTarget, obstacleMask))      // if player isn't
                                                            // behind an obstacle
            {
                currentAngle = 360f;
                farmerController.PlayerSeen(target.position);

                Debug.DrawLine(transform.position, target.position, Color.white, 2.5f);
                // debug white line shows fov - 5 times a second
            }
            else if (results.Count != 0 
                     && farmerController.canSeePlayer)     // player WAS in view but HID behind an obstacle
            {
                currentAngle = angle;
                farmerController.PlayerHid();
            }
            else if (results.Count != 0) // if player is behind an obstacle
            {
                farmerController.PlayerInRangeNoSee();
            }
        }
        else // if player isn't in view angle
        {
            farmerController.PlayerInRangeNoSee();
        }
    }

    public void WhereIsFarmerLooking() // sets dir for the purpose of FOV angle + sets viewpoint obj
    {
        animatorUpdater.AbsVectors();
        animatorUpdater.NormalizeMoveDest();

        clipAnimArray = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
        string animName = (clipAnimArray[0].clip.name);

        if (animName == "Idle_Down_Farmer" || animName == "Shoot_Down_Farmer"
                                           || animName == "Walk_Down_Farmer")
        {
            fovDir = -fovDirs[2].transform.up;
            viewPoint = fovDirs[2].transform; // which way we are looking
            viewPointString = "down";
            fovVisual?.SetOrigin(fovDirs[2].transform.position);
            fovVisual?.SetStartingAngle(0f);
            fovVisualStealth?.SetOrigin(fovDirs[2].transform.position);
            fovVisualStealth?.SetStartingAngle(0f);
        }
        else if (animName == "Idle_Up_Farmer" || animName == "Shoot_Up_Farmer"
                                              || animName == "Walk_Up_Farmer")
        {
            fovDir = fovDirs[0].transform.up;
            viewPoint = fovDirs[0].transform;
            viewPointString = "up";
            fovVisual?.SetOrigin(fovDirs[0].transform.position);
            fovVisual?.SetStartingAngle(180f);
            fovVisualStealth?.SetOrigin(fovDirs[0].transform.position);
            fovVisualStealth?.SetStartingAngle(180f);
        }
        else if (animName == "Idle_Right_Farmer" || animName == "Shoot_Right_Farmer"
                                                 || animName == "Walk_Right_Farmer")
        {
            fovDir = fovDirs[1].transform.right;
            viewPoint = fovDirs[1].transform;
            viewPointString = "right";
            fovVisual?.SetOrigin(fovDirs[1].transform.position);
            fovVisual?.SetStartingAngle(90f);
            fovVisualStealth?.SetOrigin(fovDirs[1].transform.position);
            fovVisualStealth?.SetStartingAngle(90f);
        }
        else if (animName == "Idle_Left_Farmer" || animName == "Shoot_Left_Farmer"
                                                || animName == "Walk_Left_Farmer")
        {
            fovDir = -fovDirs[3].transform.right;
            viewPoint = fovDirs[3].transform;
            viewPointString = "left";
            fovVisual?.SetOrigin(fovDirs[3].transform.position);
            fovVisual?.SetStartingAngle(270f);
            fovVisualStealth?.SetOrigin(fovDirs[3].transform.position);
            fovVisualStealth?.SetStartingAngle(270f);
        }
    }

    public void WolfStealthed()
    {
        fovVisualStealth?.gameObject.SetActive(true);
        fovVisual?.SetMaterialActive(false);
    }

    public void WolfUnstealthed()
    {
        fovVisualStealth?.gameObject.SetActive(false);
        fovVisual?.SetMaterialActive(true);
    }
}
