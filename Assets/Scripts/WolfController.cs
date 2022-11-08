using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] 
public class WolfController : MonoBehaviour
{
    public float moveSpeed;
    public float moveSpeedCurrent;
    public Rigidbody2D rb;
    Vector2 moveVelocity;
    Vector2 moveInput;
    [HideInInspector] public Vector2 lastMotionVector;
    Animator animator;
    GrabSheep gs;
    bool moving;
    [HideInInspector] public bool isStealthed;
    [HideInInspector]  public bool isCarryingSheep;

    public Transform dangleN;
    public Transform dangleE;
    public Transform dangleS;
    public Transform dangleW;
    
    private float stealthMsFactor = 1f;
    private float carryMsFactor = 1f;
    float horizontal;
    float vertical;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        moveSpeedCurrent = moveSpeed;
        animator = GetComponent<Animator>();
        gs = GetComponent<GrabSheep>();

    }

    // Update is called once per frame
    void Update()
    {
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
    }

    void Move()
    {
        moveSpeedCurrent = 
            (moveInput.x != 0 && moveInput.y != 0 ? moveSpeed / 2 : moveSpeed) 
            * stealthMsFactor * carryMsFactor ;
        rb.velocity = moveInput * moveSpeedCurrent;
    }

    private void FixedUpdate() {
        Move();
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

    void DangleSheep()
    {
     if (isCarryingSheep)
     { 
      if (horizontal == 1 )
      {
        gs.DangleSheep(dangleE);
      }
      else if (vertical == 1  )
      {
        gs.DangleSheep(dangleN);
      }
      else if (horizontal == -1  )
      {
        gs.DangleSheep(dangleW);
      }
      else if (vertical == -1  )
      {
        gs.DangleSheep(dangleS);
      }
      


     }
    }

}
