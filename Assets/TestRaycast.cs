using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRaycast : MonoBehaviour
{
    public Camera detectCam;
    public Transform mainCanvas;
    Ray ray;
    RaycastHit hit;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = detectCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.GetChild(1).GetChild(0).gameObject.activeSelf == true)
                {
                    Quaternion tempRot = new Quaternion(180,0,0,0);
                    //hit.transform.GetChild(1).GetChild(0).GetComponent<Transform>();
                    Transform panel = hit.transform.GetChild(1).GetChild(0);
                    panel.SetParent(mainCanvas);
                    panel.transform.rotation = tempRot;
                    RectTransform panelRect = panel.GetComponent<RectTransform>();
                    panelRect.sizeDelta = new Vector2(1920,980);
                    panelRect.transform.position = new Vector3(960, 490, 0);

                    //panelRect.

                    //Transform change = hit.transform.GetChild(1).GetChild(0).GetComponent<Transform>();
                    //RectTransform rect = hit.transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();

                    //change.transform.SetParent(mainCanvas);
                    //change.transform.position = new Vector3(0,0,0);
                    //change.transform.rotation = tempRot;


                    //rect.transform.position = new Vector3(960, 500, 0);
                    //rect.sizeDelta = new Vector2(1400, 870);

                }

            }
        }
    }
}
