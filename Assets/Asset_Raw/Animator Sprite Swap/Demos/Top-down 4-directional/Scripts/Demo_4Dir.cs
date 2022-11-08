using AnimatorSpriteSwapSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demo_4Dir : MonoBehaviour {
    public bool rotate;
    public Text demoText;

    private Animator animator;
    private ReskinDynamic reskin;

    private int directionIndex; //look direction index (0 - left, 1 - bottom, 2 - right, 3 - top)
    private string[] directionNames = { "West", "South", "East", "North",};

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
            if (directionIndex >= 4)
                directionIndex = 0;
            reskin.SetIndex(directionIndex);
            next_direction_timer = Time.time + 1.5f;

            if (demoText)
                demoText.text = "Direction index: " + directionIndex + " (" + directionNames[directionIndex] + ")";
        }

    }
}
