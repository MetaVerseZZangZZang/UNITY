using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedPeopleSystem;
using UnityEngine.UI;

public class UI_Character : MonoBehaviour
{
    public static UI_Character Instance;
    
    public CharacterCustomization CharacterCustomization;
    [Space(15)]

    public Text lod_text;

    public Text panelNameText;

    public RectTransform HairPanel;
    public RectTransform ShirtPanel;
    public RectTransform PantsPanel;
    public RectTransform ShoesPanel;
    public RectTransform HatPanel;
    public RectTransform BackpackPanel;

    public RectTransform FaceEditPanel;
    public RectTransform BaseEditPanel;
    
    public RectTransform HairColorPanel;
    
    public RectTransform SavesPanel;
    public RectTransform SavesPanelList;
    public RectTransform SavesPrefab;
    public List<RectTransform> SavesList = new List<RectTransform>();
    
    public Image HairColorButtonColor;

    public Vector3[] CameraPositionForPanels;
    public Vector3[] CameraEulerForPanels;
    int currentPanelIndex = 0;

    public Camera Camera;

    public RectTransform maleUI;
    public RectTransform femaleUI;

    public void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        HairPanel.gameObject.SetActive(false);
        ShirtPanel.gameObject.SetActive(false);
        PantsPanel.gameObject.SetActive(false);
        ShoesPanel.gameObject.SetActive(false);
        HatPanel.gameObject.SetActive(false);
        BackpackPanel.gameObject.SetActive(false);
    
    }
    #region ButtonEvents
    public void SwitchCharacterSettings(string name)
    {
        CharacterCustomization.SwitchCharacterSettings(name);
        CharCustomManager.Instance.selectedGender = name;
        
        if(name == "Male")
        {
            maleUI.gameObject.SetActive(true);
            femaleUI.gameObject.SetActive(false);
        }
        if (name == "Female")
        {
            femaleUI.gameObject.SetActive(true);
            maleUI.gameObject.SetActive(false);
        }
    }
    
    public void HairPanel_Select()
    {
        HideAllPanels();
        if (HairPanel.gameObject.activeSelf)
            HairPanel.gameObject.SetActive(false);
        else
            HairPanel.gameObject.SetActive(true);
    }
    
    public void ShowFaceEdit()
    {
        FaceEditPanel.gameObject.SetActive(true);
        BaseEditPanel.gameObject.SetActive(false);
        currentPanelIndex = 1;
        panelNameText.text = "FACE CUSTOMIZER";
    }

    public void ShowBaseEdit()
    {
        FaceEditPanel.gameObject.SetActive(false);
        BaseEditPanel.gameObject.SetActive(true);
        currentPanelIndex = 0;
        panelNameText.text = "BASE CUSTOMIZER";
    }
   
    int lodIndex;
    public void Lod_Event(int next)
    {
        lodIndex += next;
        if (lodIndex < 0)
            lodIndex = 3;
        if (lodIndex > 3)
            lodIndex = 0;

        lod_text.text = lodIndex.ToString();

        CharacterCustomization.ForceLOD(lodIndex);
    }
    
    public void SetNewHairColor(Color color)
    {
        HairColorButtonColor.color = color;
        CharacterCustomization.SetBodyColor(BodyColorPart.Hair, color);
    }
   
    public void VisibleHairColorPanel(bool v)
    {
        HideAllPanels();
        HairColorPanel.gameObject.SetActive(v);
    }
    
    public void ShirtPanel_Select()
    {
        HideAllPanels();
        if (ShirtPanel.gameObject.activeSelf)
            ShirtPanel.gameObject.SetActive(false);
        else
            ShirtPanel.gameObject.SetActive(true);
    }
    public void PantsPanel_Select()
    {
        HideAllPanels();
        if (PantsPanel.gameObject.activeSelf)
            PantsPanel.gameObject.SetActive(false);
        else
            PantsPanel.gameObject.SetActive(true);
    }
    public void ShoesPanel_Select()
    {
        HideAllPanels();
        if (ShoesPanel.gameObject.activeSelf)
            ShoesPanel.gameObject.SetActive(false);
        else
            ShoesPanel.gameObject.SetActive(true);
    }
    public void BackpackPanel_Select()
    {
        HideAllPanels();
        if (BackpackPanel.gameObject.activeSelf)
            BackpackPanel.gameObject.SetActive(false);
        else
            BackpackPanel.gameObject.SetActive(true);
    }

    public void HatPanel_Select()
    {
        HideAllPanels();
        if (HatPanel.gameObject.activeSelf)
            HatPanel.gameObject.SetActive(false);
        else
            HatPanel.gameObject.SetActive(true);
    }


    public void SavesPanel_Select(bool v)
    {
        HideAllPanels();
        if (!v)
        {
            SavesPanel.gameObject.SetActive(false);
            foreach(var save in SavesList)
            {
                Destroy(save.gameObject);
            }
            SavesList.Clear();
        }
        else
        {
            var saves = CharacterCustomization.GetSavedCharacterDatas();
            for (int i = 0; i < saves.Count;i++)
            {
                var savePrefab = Instantiate(SavesPrefab, SavesPanelList);
                int index = i;
                savePrefab.GetComponent<Button>().onClick.AddListener(() => SaveSelect(index));
                savePrefab.GetComponentInChildren<Text>().text = string.Format("({0}) {1}",index,saves[i].name);
                SavesList.Add(savePrefab);
            }
            SavesPanel.gameObject.SetActive(true);
        }
    }
    public void SaveSelect(int index)
    {
        var saves = CharacterCustomization.GetSavedCharacterDatas();
        CharacterCustomization.ApplySavedCharacterData(saves[index]);
    }
    public void EmotionsChange_Event(int index)
    {
        var anim = CharacterCustomization.Settings.characterAnimationPresets[index];
        if (anim != null)
            CharacterCustomization.PlayBlendshapeAnimation(anim.name, 2f);
    }
    public void HairChange_Event(int index)
    {
        CharCustomManager.Instance.selectedHairIndex = index;
        CharacterCustomization.SetElementByIndex(CharacterElementType.Hair, index);
    }
    public void ShirtChange_Event(int index)
    {
        CharCustomManager.Instance.selectedShirtsIndex = index;
        CharacterCustomization.SetElementByIndex(CharacterElementType.Shirt, index);
    }
    public void PantsChange_Event(int index)
    {
        CharCustomManager.Instance.selectedPantsIndex = index;
        CharacterCustomization.SetElementByIndex(CharacterElementType.Pants, index);
    }
    public void ShoesChange_Event(int index)
    {
        CharCustomManager.Instance.selectedShoesIndex = index;
        CharacterCustomization.SetElementByIndex(CharacterElementType.Shoes, index);
    }
    
    public void HatChange_Event(int index)
    {
        CharCustomManager.Instance.selectedHatIndex = index;
        CharacterCustomization.SetElementByIndex(CharacterElementType.Hat, index);
    }
    
    public void BackpackChange_Event(int index)
    {
        CharacterCustomization.SetElementByIndex(CharacterElementType.Item1, index);
    }

    public void AccessoryChange_Event(int index)
    {
        CharacterCustomization.SetElementByIndex(CharacterElementType.Accessory, index);
    }
    public void HideAllPanels()
    {
        //HairColorPanel.gameObject.SetActive(false);
        HairPanel.gameObject.SetActive(false);
        HatPanel.gameObject.SetActive(false);
        ShirtPanel.gameObject.SetActive(false);
        PantsPanel.gameObject.SetActive(false);
        ShoesPanel.gameObject.SetActive(false);
        //BackpackPanel.gameObject.SetActive(false);
    }
    public void SaveToFile()
    {
        CharacterCustomization.SaveCharacterToFile(CharacterCustomizationSetup.CharacterFileSaveFormat.Json);
    }
    public void ClearFromFile()
    {
        SavesPanel.gameObject.SetActive(false);
        CharacterCustomization.ClearSavedData();
    }
    public void Randimize()
    {
        CharacterCustomization.Randomize();
    }
    
    #endregion
    


    // Update is called once per frame
    void Update()
    {
        
    }
}
