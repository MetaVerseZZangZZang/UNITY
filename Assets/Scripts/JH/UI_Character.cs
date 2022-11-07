using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Character : MonoBehaviour
{
    public GameObject HairItem;
    public GameObject ShirtsItem;
    public GameObject PantsItem;
    public GameObject SetItem;
    
    public Toggle HairToggle;
    public Toggle ShirtsToggle;
    public Toggle PantsToggle;
    public Toggle SetToggle;


    void Start()
    {
        HairItem.SetActive(true);
        ShirtsItem.SetActive(false);
        PantsItem.SetActive(false);
        SetItem.SetActive(false);
    }
    
    public void CharRadio()
    {
        if (HairToggle.isOn)
        {
            HairItem.SetActive(true);
            ShirtsItem.SetActive(false);
            PantsItem.SetActive(false);
            SetItem.SetActive(false);
        }
        
        else if (ShirtsToggle.isOn)
        {
            HairItem.SetActive(false);
            ShirtsItem.SetActive(true);
            PantsItem.SetActive(false);
            SetItem.SetActive(false);
        }
        
        else if (PantsToggle.isOn)
        {
            HairItem.SetActive(false);
            ShirtsItem.SetActive(false);
            PantsItem.SetActive(true);
            SetItem.SetActive(false);
        }
        
        else if (SetToggle.isOn)
        {
            HairItem.SetActive(false);
            ShirtsItem.SetActive(false);
            PantsItem.SetActive(false);
            SetItem.SetActive(true);
        }
    }
    
    
}
