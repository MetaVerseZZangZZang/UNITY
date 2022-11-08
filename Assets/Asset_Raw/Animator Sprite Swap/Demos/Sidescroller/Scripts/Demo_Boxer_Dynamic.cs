using AnimatorSpriteSwapSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo_Boxer_Dynamic : MonoBehaviour
{
    private Animator animator;
    private ReskinDynamic  reskin;

    private float   next_animation_timer = 2.0f;
    private float   next_skin_timer = 0.3f;
    private int     next_skin_index = 0;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        reskin = GetComponent<ReskinDynamic>();
    }
    
    void Update()
    {
        if(next_animation_timer < Time.time)
        {
            animator.SetTrigger("Next");
            next_animation_timer = Time.time + 2.0f;
        }

        if (reskin && next_skin_timer < Time.time)
        {
            next_skin_index++;
            if (reskin.reskinAsset && next_skin_index >= reskin.reskinAsset.alternateCount)
                next_skin_index = 0;

            reskin.SetIndex(next_skin_index);
            next_skin_timer = Time.time + 0.3f;
        }
    }
}
