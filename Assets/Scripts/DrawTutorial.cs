using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class DrawTutorial : MonoBehaviour
{
    public Camera m_camera;
    public GameObject brush;

    private LineRenderer currentLineRenderer;

    private Vector2 lastPos;

    private void Update()
    {
        Draw();
    }

    void Draw()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CreateBrush();
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos != lastPos)
            {
                AddPoint(mousePos);
                lastPos = mousePos;
            }
            
        }
        else
        {
            currentLineRenderer = null;
        }
    }


    void CreateBrush()
    {
        GameObject brushInstance = Instantiate(brush);
        currentLineRenderer = brushInstance.GetComponent<LineRenderer>();

        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        
        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);
    }

    void AddPoint(Vector2 pointPos)
    {
        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPos);
    }
}
