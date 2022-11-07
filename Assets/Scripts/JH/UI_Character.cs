using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Character : MonoBehaviour
{
    public static UI_Character Instance;
    public GameObject HairItem;
    public GameObject ShirtsItem;
    public GameObject PantsItem;
    public GameObject SetItem;
    
    public Toggle HairToggle;
    public Toggle ShirtsToggle;
    public Toggle PantsToggle;
    public Toggle SetToggle;
    
    public Sprite SelectedShirts;

    public GameObject ShirtsParent;

    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        HairItem.SetActive(true);
        ShirtsItem.SetActive(false);
        PantsItem.SetActive(false);
        SetItem.SetActive(false);
        
        SelectedShirts=ShirtsParent.GetComponent<Image>().sprite;
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

    public void SetCloths(Image item)
    {
        Sprite itemSprite = item.sprite;

        
        if (item.tag == "Shirts")
        {
            ShirtsParent.GetComponent<Image>().sprite = itemSprite;

            SelectedShirts = itemSprite;
        }
        
    }
    
}
