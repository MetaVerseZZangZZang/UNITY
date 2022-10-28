using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.Json.Nodes;

public class ReceiveKeyword : MonoBehaviour
{
    public static ReceiveKeyword Instance;


    private void Awake()
    {
        Instance = this;
    }


    public void ReceiveJson(System.Text.Json.JsonElement jd)
    {
        Debug.Log(jd);
    }


}
