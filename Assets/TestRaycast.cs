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
            Debug.LogError("1111111111");

            if (Physics.Raycast(ray, out hit))
            {
                Debug.LogError("222222222");
                if (hit.transform.GetChild(1).GetChild(0).gameObject.activeSelf == true)
                {
                    Debug.LogError("3333333");
                    Quaternion tempRot = new Quaternion(180,0,0,0);

                    Transform change = hit.transform.GetChild(1).GetChild(0).GetComponent<Transform>();
                    //RectTransform rect = hit.transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();

                    change.transform.SetParent(mainCanvas);
                    //change.transform.position = new Vector3(0,0,0);
                    //change.transform.rotation = tempRot;

                  
                    //rect.transform.position = new Vector3(960, 500, 0);
                    //rect.sizeDelta = new Vector2(1400, 870);

                }

            }
        }
    }
}
