using AnimatorSpriteSwapSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demo_8Dir : MonoBehaviour {
    public bool rotate;
    public Text demoText;

    private Animator animator;
    private ReskinDynamic reskin;

    private int directionIndex; //look direction index (0 - left, 1 - bottom-left, 2 - bottom, 3 - bottom-right, 4 - right, 5 - top-right, 6 - top, 7 - top-left)
    private string[] directionNames = {"West" , "Southwest", "South", "Southeast", "East", "Northeast", "North", "Northwest"}; 

    private float next_animation_timer = 2.0f;
    private float next_direction_timer = 1.5f;

    void Start()
    {
        animator = GetComponent<Animator>();
        reskin = GetComponent<ReskinDynamic>();
    }

    void Update()
    {
        if (next_animation_timer < Time.time)
        {
            animator.SetTrigger("Next");
            next_animation_timer = Time.time + 2.0f;
        }


        if (rotate && next_direction_timer < Time.time)
        {
            directionIndex++;
            if (directionIndex >= 8)
                directionIndex = 0;
            reskin.SetIndex(directionIndex);
            next_direction_timer = Time.time + 1.5f;

            if (demoText)
                demoText.text = "Direction index: " + directionIndex + " (" + directionNames[directionIndex] + ")";
        }

    }
}
