using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRaycast : MonoBehaviour
{
    public Camera detectCam;
    public Transform mainCanvas;

    public Transform panel;
    Ray ray;
    RaycastHit hit;

    public RectTransform elementRect;
    public Transform playerCanvas;

    public Vector2 elmentSize;
    public Vector3 elmentPosition;

    void Update()
    {
        if (UI_MainPanel.Instance.conferenceStart)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ray = detectCam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.GetChild(1).GetChild(0).gameObject.activeSelf == true && hit.transform.gameObject.name.Contains("(user)"))
                    {
                        elementRect = hit.transform.GetChild(1).GetComponent<RectTransform>();
                        playerCanvas = hit.transform.GetChild(1);
                        Debug.LogError("elmentRect" + elementRect.transform.position);

                        //Quaternion tempRot = new Quaternion(0, 0, 0, 0);

                        //hit.transform.GetChild(1).GetChild(0).GetComponent<Transform>();
                        panel = hit.transform.GetChild(1).GetChild(0);
                        panel.SetParent(mainCanvas);

                        RectTransform panelRect = panel.GetComponent<RectTransform>();
                        panelRect.transform.rotation = Quaternion.identity;
                        panelRect.transform.localScale = new Vector3(-1, -1, -1);

                        elmentSize = panelRect.sizeDelta;
                        elmentPosition = panelRect.transform.position;

                        panelRect.sizeDelta = new Vector2(1920, 980);
                        panelRect.transform.position = new Vector3(960, 490, 0);

                    }

                }
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ReduceSize();
            }
        }
    }


    public void ReduceSize()
    {
        Debug.LogError("111111");
        panel.SetParent(playerCanvas);
        panel.SetAsFirstSibling();

        RectTransform changePanel = panel.GetComponent<RectTransform>();

        changePanel.sizeDelta = elmentSize;
        changePanel.transform.position = elmentPosition;
    }
}
