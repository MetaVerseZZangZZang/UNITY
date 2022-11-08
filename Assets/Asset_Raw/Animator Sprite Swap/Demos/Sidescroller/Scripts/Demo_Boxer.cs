using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo_Boxer : MonoBehaviour
{
    private Animator animator;

    private float next_animation_timer = 2.0f;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if(next_animation_timer < Time.time)
        {
            animator.SetTrigger("Next");
            next_animation_timer = Time.time + 2.0f;
        }
    }
}
