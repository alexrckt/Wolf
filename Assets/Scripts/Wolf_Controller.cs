using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] 
public class Wolf_Controller : MonoBehaviour
{
    public float moveSpeed;
    public float moveSpeedCurrent;
    public Rigidbody2D rb;
    Vector2 moveVelocity;
    Vector2 moveInput;
    public Vector2 lastMotionVector;
    Animator animator;
    public bool moving;
    public bool isStealthed;
    public bool isCarryingSheep;
    
    private float stealthMsFactor = 1f;
    private float carryMsFactor = 1f;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        moveSpeedCurrent = moveSpeed;
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(horizontal, vertical );
        animator.SetFloat("horizontal", horizontal);
        animator.SetFloat("vertical", vertical);
        
        moving = horizontal != 0 || vertical != 0;
        animator.SetBool("moving", moving);
        if (horizontal != 0 || vertical != 0)
        {
         lastMotionVector = new Vector2(horizontal, vertical).normalized;
         animator.SetFloat("lastHorizontal", horizontal);
         animator.SetFloat("lastVertical", vertical);
        }
    }

    void Move()
    {
        moveSpeedCurrent = moveSpeed * stealthMsFactor * carryMsFactor ;
        rb.velocity = moveInput * moveSpeedCurrent;
    }

    private void FixedUpdate() {
        Move();
    }

    public void IsStealthed(bool yesno)
    {
        isStealthed = yesno;
      if (yesno)
      stealthMsFactor = 0.5f;
      else
      stealthMsFactor = 1f;
      
    }
 
 public void IsCarryingSheep(bool yesno)
    {
        isCarryingSheep = yesno;
      if (yesno)
      carryMsFactor = 0.5f;
      else
      carryMsFactor = 1f;
      
    }

}
