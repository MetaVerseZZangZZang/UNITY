using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCustomManager : MonoBehaviour
{
    public static CharCustomManager Instance;

    public string selectedGender;
    public int selectedHairIndex=-1;
    public int selectedShirtsIndex=-1;
    public int selectedPantsIndex=-1;
    public int selectedShoesIndex=-1;
    public int selectedHatIndex=-1;
    
    
    private void Awake()
    {
        Instance = this;
    }
}
