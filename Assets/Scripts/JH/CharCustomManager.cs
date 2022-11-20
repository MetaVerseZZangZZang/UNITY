using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCustomManager : MonoBehaviour
{
    public static CharCustomManager Instance;

    public string selectedGender;
    public int selectedHairIndex;
    public int selectedShirtsIndex;
    public int selectedPantsIndex;
    public int selectedShoesIndex;
    public int selectedBackpackIndex;
    public int selectedHatIndex;
    
    
    private void Awake()
    {
        Instance = this;
    }
}
