using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShareScreenExtend : MonoBehaviour
{
    public GameObject mainCanvas;
    public Button extendBTN;

    private void Awake()
    {
        //extendBTN = transform.GetComponent<Button>();
        extendBTN.onClick.AddListener(ExtendScreen);
    }


    public void ExtendScreen()
    {
        transform.SetParent(mainCanvas.transform);
        Debug.LogError("cl");
    }
}
