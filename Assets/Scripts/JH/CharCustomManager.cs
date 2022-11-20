using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCustomManager : MonoBehaviour
{
    public static CharCustomManager Instance;
    public int selectedHairIndex;
    public int selectedShirtsIndex;

    private void Awake()
    {
        Instance = this;
    }
}
