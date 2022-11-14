using System;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimatorUpdater : MonoBehaviour
{
    AIPath aiPath;
    Animator animator;

    public Vector2 lastMotionVector;
    public bool moving;
    public float horizontal;
    public float vertical;
    public Vector2 debugVelocity;

    // Start is called before the first frame update
    void Start()
    {
        aiPath = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        #region Animator update
        debugVelocity = aiPath.velocity;
        horizontal = aiPath.desiredVelocity.x;
        vertical = aiPath.desiredVelocity.y;
        AbsVectors();   // compares if the agent is going more vertically or horizontally
                        // to decide which anim is more sutiable
        NormalizeMoveDest();

        animator.SetFloat("horizontal", horizontal);
        animator.SetFloat("vertical", vertical);

        moving = horizontal != 0 || vertical != 0;
        animator.SetBool("moving", moving);

        if (horizontal != 0 || vertical != 0)
        {
            lastMotionVector = new Vector2(horizontal, vertical);
            animator.SetFloat("lastHorizontal", horizontal);
            animator.SetFloat("lastVertical", vertical);
        }
        #endregion
    }

    public void NormalizeMoveDest()
    {
        if (horizontal > 0)
        {
            horizontal = 1f;
        }
        else if (horizontal < 0)
        {
            horizontal = -1f;
        }

        if (vertical > 0)
        {
            vertical = 1f;
        }
        else if (vertical < 0)
        {
            vertical = -1f;
        }
    }

    public void AbsVectors()
    {
        if (Math.Abs(vertical) > Math.Abs(horizontal))
        {
            horizontal = 0f;
        }
        else
        {
            vertical = 0f;
        }

    }
}
