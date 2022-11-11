using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barker : MonoBehaviour
{
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("Barking", true);
    }

    public void AnimationSwitch(bool active)
    {
        GetComponent<SpriteRenderer>().enabled = active;
    }
}
