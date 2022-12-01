
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DogFieldOfView : MonoBehaviour
{
    [SerializeField] private float radius;
    public float closeradius;
    [Range (0, 360)]
    private float angle;
    [SerializeField] private float currentAngle;
    public bool canSeePlayer;
    public Transform fovPoint;
    public GameObject playerRef;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public GameObject[] fovDirs;
    Vector3 fovDir = new Vector3();
    DogController dogController;
    WolfController wolfController;
    SheepsClothing sheepsClothing;
    AnimatorClipInfo[] clipAnimArray;
    Animator animator;
    private AnimatorUpdater animatorUpdater;
    private GameManager gameManager;
    [SerializeField] FOVVisual fovVisual;
    [SerializeField] FOVVisual fovVisualStealth;


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerRef = GameObject.FindGameObjectWithTag("Player");
        sheepsClothing = playerRef.GetComponent<SheepsClothing>();
        wolfController = playerRef.GetComponent<WolfController>();
        dogController = GetComponent<DogController>();
        animatorUpdater = GetComponent<AnimatorUpdater>();
        radius = gameManager.GetFOVRadius(gameObject);
        angle = gameManager.GetFOVAngle(gameObject);
        currentAngle = angle;
        
        fovVisual.SetAngleAndRadius(currentAngle, radius);
        fovVisualStealth.SetAngleAndRadius(360f, closeradius);

        EventManager.OnStealth += WolfStealthed;
        EventManager.OnStealthFinish += WolfUnstealthed;

        animator = GetComponentInChildren<Animator>();
        StartCoroutine(FOVRoutine());
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    private void OnDisable() {
        EventManager.OnStealth -= WolfStealthed;
        EventManager.OnStealthFinish -= WolfUnstealthed;
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            int rnd = Random.Range(0, 256);
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(transform.position,  radius, playerMask);
        Collider2D[] closeRangeChecks = Physics2D.OverlapCircleAll(transform.position,  closeradius, playerMask);
        WhereIsDogLooking();
        if (closeRangeChecks.Length == 0 && rangeChecks.Length != 0 && !wolfController.isStealthed ) // if player is in the circle's range
        {
            WhereIsDogLooking();    // check where the dog is facing now
            Transform target = rangeChecks[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;
            if (Vector2.Angle(fovDir, directionToTarget) < currentAngle / 2) // if player in view angle
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);
                if (!Physics2D.Raycast(transform.position, directionToTarget,
                                                distanceToTarget, obstacleMask)) // if player isn't
                                                                                   // behind an obstacle
                {
                    canSeePlayer = true;
                    sheepsClothing.isSeen = true;
                    dogController.SetChasingState();
                    Debug.DrawLine(transform.position, target.position, Color.white, 2.5f);
                    // debug white line shows fov - 5 times a second
                }
                else // if player is behind an obstacle
                {
                    canSeePlayer = false;
                    sheepsClothing.isSeen = false;
                    dogController.PlayerLastSeen(); // if dog is chasing
                                         //sets dog to lost state and saves a ref to last point 
                                         //where it saw the player
                }
            }
            else // if player isn't in view angle
            {
                canSeePlayer = false;
                sheepsClothing.isSeen = false;
                dogController.PlayerLastSeen();
                
            }
        }
        else if (closeRangeChecks.Length == 0 && canSeePlayer) // if player was in view angle but got away from it
        {
            canSeePlayer = false;
            sheepsClothing.isSeen = false;
            dogController.PlayerLastSeen();
            
            // method for getting last pos seen, going there, then resetting to calm on reach destination

        }
        else if (closeRangeChecks.Length != 0)
        {
            canSeePlayer = true;
            sheepsClothing.isSeen = true;
            Transform target = closeRangeChecks[0].transform;
                    dogController.SetChasingState();
                    Debug.DrawLine(transform.position, target.position, Color.white, 2.5f);
        }
    }

    
     public void WhereIsDogLooking()
    {
        animatorUpdater.AbsVectors();
        animatorUpdater.NormalizeMoveDest();

        clipAnimArray = animator.GetCurrentAnimatorClipInfo(0);
        string animName = (clipAnimArray[0].clip.name);

        if (animName == "Idle_Down_Dog"
            || animName == "Walk_Down_Dog")
        {
            fovDir = -fovDirs[2].transform.up;
            fovVisual.SetOrigin(fovDirs[2].transform.position);
            fovVisual.SetStartingAngle(0f);
            fovVisualStealth.SetOrigin(fovDirs[2].transform.position);
            fovVisualStealth.SetStartingAngle(0f);
            // Debug.Log("down");
        }
        else if (animName == "Walk_Up_Dog")
        {
            fovDir = fovDirs[0].transform.up;
            fovVisual.SetOrigin(fovDirs[0].transform.position);
            fovVisual.SetStartingAngle(180f);
            fovVisualStealth.SetOrigin(fovDirs[0].transform.position);
            fovVisualStealth.SetStartingAngle(180f);
            // Debug.Log("up");
        }
        else if (animName == "Walk_Right_Dog")
        {
            fovDir = fovDirs[1].transform.right;
            fovVisual.SetOrigin(fovDirs[1].transform.position);
            fovVisual.SetStartingAngle(90f);
            fovVisualStealth.SetOrigin(fovDirs[1].transform.position);
            fovVisualStealth.SetStartingAngle(90f);
            //  Debug.Log("right");
        }
        else if (animName == "Walk_Left_Dog")
        {
            fovDir = -fovDirs[3].transform.right;
            fovVisual.SetOrigin(fovDirs[3].transform.position);
            fovVisual.SetStartingAngle(270f);
            fovVisualStealth.SetOrigin(fovDirs[3].transform.position);
            fovVisualStealth.SetStartingAngle(270f);
            //  Debug.Log("left");
        }
    }

    public void WolfStealthed()
    {
        fovVisualStealth.gameObject.SetActive(true);
        fovVisual.SetMaterialActive(false);
    }

    public void WolfUnstealthed()
    {
        fovVisualStealth.gameObject.SetActive(false);
        fovVisual.SetMaterialActive(true);
    }
    
}
