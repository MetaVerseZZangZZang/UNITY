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
    
    public Toggle HairToggle;
    public Toggle ShirtsToggle;
    public Toggle PantsToggle;
    
    public Sprite SelectedShirts;
    public Sprite SelectedPants;
    public Sprite SelectedHair;

    public GameObject ShirtsParent;
    public GameObject HairParent;
    public GameObject PantsParent;
    
    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        HairItem.SetActive(true);
        ShirtsItem.SetActive(false);
        PantsItem.SetActive(false);
        
        SelectedShirts=ShirtsParent.GetComponent<Image>().sprite;
    }
    
    public void CharRadio()
    {
        if (HairToggle.isOn)
        {
            HairItem.SetActive(true);
            ShirtsItem.SetActive(false);
            PantsItem.SetActive(false);
        }
        
        else if (ShirtsToggle.isOn)
        {
            HairItem.SetActive(false);
            ShirtsItem.SetActive(true);
            PantsItem.SetActive(false);
        }
        
        else if (PantsToggle.isOn)
        {
            HairItem.SetActive(false);
            ShirtsItem.SetActive(false);
            PantsItem.SetActive(true);
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
        
        if (item.tag == "Hair")
        {
            HairParent.GetComponent<Image>().sprite = itemSprite;
            SelectedHair = itemSprite;
        }
        
    }

}
