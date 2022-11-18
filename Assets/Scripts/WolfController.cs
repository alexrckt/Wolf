using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Mono.Cecil;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = System.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(Rigidbody2D))] 
public class WolfController : MonoBehaviour
{
    public FootStepFade footstepPrefab;
    [field: HideInInspector] public static LinkedList<FootStepFade> Footsteps { get; set; }

    public float moveSpeed;
    public float moveSpeedCurrent;
    public Rigidbody2D rb;
    Vector2 moveVelocity;
    Vector2 moveInput;
    [HideInInspector] public Vector2 lastMotionVector;
    Animator animator;
    GrabSheep grabSheep;
    bool moving;

    [HideInInspector] public bool isStealthed;
    [HideInInspector] public bool isCarryingSheep;

    public Transform dangleN;
    public Transform dangleE;
    public Transform dangleS;
    public Transform dangleW;

    private GameManager gameManager;
    private float stealthMsFactor = 1f;
    private float carryMsFactor = 1f;
    float horizontal;
    float vertical;
    private Vector3 lastStep;
    public Transform footstepParent;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        moveSpeedCurrent = moveSpeed;
        animator = GetComponent<Animator>();
        grabSheep = GetComponent<GrabSheep>();
        lastStep = new Vector3();
        Footsteps = new LinkedList<FootStepFade>();
        gameManager = FindObjectOfType<GameManager>();
        footstepParent = GameObject.FindGameObjectWithTag("FootstepParent").transform;

        //InvokeRepeating("Debugging", 1f, 1f);
    }

    void Debugging()
    {
        Debug.Log("Footstep list count: " + Footsteps.Count);
    }

    // Update is called once per frame
    void Update()
    {
        #region Animator update
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(horizontal, vertical );

        animator.SetFloat("horizontal", horizontal);
        animator.SetFloat("vertical", vertical);
        
        moving = horizontal != 0 || vertical != 0;
        animator.SetBool("moving", moving);
        if (horizontal != 0 || vertical != 0)
        {
            DangleSheep();
            lastMotionVector = new Vector2(horizontal, vertical).normalized;
            animator.SetFloat("lastHorizontal", horizontal);
            animator.SetFloat("lastVertical", vertical);
        }
        #endregion
    }

    void Move()
    {
        moveSpeedCurrent = 
            (moveInput.x != 0 && moveInput.y != 0 ? moveSpeed / 2 : moveSpeed) 
            * stealthMsFactor * carryMsFactor ;
        rb.velocity = moveInput * moveSpeedCurrent;
    }

    void SpawnFootsteps()
    {
        if (Vector3.Distance(transform.position, lastStep) > 1)
        {
            lastStep = transform.position;

            var fs = Instantiate<FootStepFade>(footstepPrefab, transform.position,
                Quaternion.LookRotation(Vector3.forward, lastMotionVector));
            
            //var bounds = fs.GetComponent<Collider2D>().bounds;
            // Expand the bounds along the Z axis
            //bounds.Expand(Vector3.forward * 1000);
            Footsteps.AddLast(fs);
            fs.transform.SetParent(footstepParent, true);
        }
    }

    private void FixedUpdate() {
        Move();
        SpawnFootsteps();
    }

    public void IsStealthed(bool yesno)
    {
        isStealthed = yesno;
        stealthMsFactor = yesno ? 0.5f : 1f;
    }
 
    public void IsCarryingSheep(bool yesno)
    {
        isCarryingSheep = yesno;
        carryMsFactor = yesno ? 0.5f : 1f;
    }

    public void WolfInjured()
    {
        gameManager.WolfInjured();
    }

    void DangleSheep()
    {
        if (isCarryingSheep)
        {
            if (horizontal == 1)
            {
                grabSheep.DangleSheep(dangleE);
            }
            else if (vertical == 1)
            {
                grabSheep.DangleSheep(dangleN);
            }
            else if (horizontal == -1)
            {
                grabSheep.DangleSheep(dangleW);
            }
            else if (vertical == -1)
            {
                grabSheep.DangleSheep(dangleS);
            }
        }
    }

}
