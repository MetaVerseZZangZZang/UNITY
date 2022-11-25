using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCustomManager : MonoBehaviour
{
    public static CharCustomManager Instance;

    public string selectedGender;
    public int selectedHairIndex=0;
    public int selectedShirtsIndex=0;
    public int selectedPantsIndex=0;
    public int selectedShoesIndex=0;
    public int selectedHatIndex=-1;
    
    
    private void Awake()
    {
        Instance = this;
    }
}
