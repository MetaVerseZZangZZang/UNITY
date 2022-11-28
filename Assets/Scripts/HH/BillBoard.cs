using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Transform camTransform;
    private GameObject mainPanel;
    private QuarterViewCam quarter;
    public RectTransform ImageTr;
    public RectTransform NameTr;

    // Start is called before the first frame update
    void Start()
    {
        mainPanel = FindObjectOfType<UI_MainPanel>().gameObject;
        camTransform = Camera.main.transform;
        quarter = mainPanel.GetComponent<UI_MainPanel>().cam.GetComponent<QuarterViewCam>();
    }
    
    private void LateUpdate()   
    {
        if (quarter.enabled)
        {
            //ImageTr.position = new Vector3(0f, 2.460022f, 0.24f);
            ImageTr.rotation = Quaternion.Euler(200,316,1.5f);
            //ImageTr.anchoredPosition = new Vector2(-1f, 2.460022f);
            //NameTr.position = new Vector3(0f, 2.460022f, 0.15f);
            NameTr.rotation = Quaternion.Euler(45f,-45f,0f);
            //ImageTr.anchoredPosition = new Vector2(0f, 2.460022f);
            //NameTr.localPosition = new Vector3(NameTr.localPosition.x, NameTr.localPosition.y, 0.15f);
        }
        else
        {
            ImageTr.LookAt(ImageTr.position + camTransform.forward);
            NameTr.LookAt(NameTr.position + camTransform.forward);
            ImageTr.rotation = Quaternion.Euler(ImageTr.rotation.x,ImageTr.rotation.y,180f);
        }
    }
}
