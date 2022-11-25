using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerCam : MonoBehaviour
{
    public static UIPlayerCam instance;
    public Button camButton;
    private QuarterViewCam quarter;
    private ThirdPersonCam third;

    private void Awake()
    {
        instance = this;
        quarter = Camera.main.GetComponent<QuarterViewCam>();
        third = Camera.main.GetComponent<ThirdPersonCam>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        quarter.enabled = true;
        third.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 쿼터뷰 -> 3인칭, 3인칭 -> 쿼터뷰
    public void CamButtonClick()
    {
        string camText = camButton.GetComponentInChildren<Text>().text;
        if (camText.Contains("쿼터뷰"))
        {
            quarter.enabled = false;
            third.enabled = true;
            camButton.GetComponentInChildren<Text>().text = "3인칭";
        }
        else
        {
            quarter.enabled = true;
            third.enabled = false;
            camButton.GetComponentInChildren<Text>().text = "쿼터뷰";
        }
    }
}
