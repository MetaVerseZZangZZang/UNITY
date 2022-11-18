using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
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

    public void setTrue(Toggle toggle)
    {
        toggle.transform.GetChild(0).gameObject.SetActive(true);
        toggle.transform.GetChild(1).gameObject.SetActive(false);
    }
    
    public void setFalse(Toggle toggle)
    {
        toggle.transform.GetChild(0).gameObject.SetActive(false);
        toggle.transform.GetChild(1).gameObject.SetActive(true);
    }
    
    void Start()
    {
        HairItem.SetActive(true);
        setTrue(HairToggle);
        ShirtsItem.SetActive(false);
        setFalse(ShirtsToggle);
        PantsItem.SetActive(false);
        setFalse(PantsToggle);
        
        SelectedShirts=ShirtsParent.GetComponent<Image>().sprite;
        SelectedPants=HairParent.GetComponent<Image>().sprite;
        SelectedHair=PantsParent.GetComponent<Image>().sprite;
    }
    
    public void CharRadio()
    {
        if (HairToggle.isOn)
        {
            setTrue(HairToggle);
            setFalse(ShirtsToggle);
            setFalse(PantsToggle);
            
            HairItem.SetActive(true);
            ShirtsItem.SetActive(false);
            PantsItem.SetActive(false);
        }
        
        else if (ShirtsToggle.isOn)
        {
            HairItem.SetActive(false);
            ShirtsItem.SetActive(true);
            PantsItem.SetActive(false);
            
            setTrue(ShirtsToggle);
            setFalse(HairToggle);
            setFalse(PantsToggle);
        }
        
        else if (PantsToggle.isOn)
        {
            HairItem.SetActive(false);
            ShirtsItem.SetActive(false);
            PantsItem.SetActive(true);
            
            setTrue(PantsToggle);
            setFalse(ShirtsToggle);
            setFalse(HairToggle);
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
        
        if (item.tag == "Pants")
        {
            PantsParent.GetComponent<Image>().sprite = itemSprite;
            SelectedPants = itemSprite;
        }
    }
    

}
*/