using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerSlot : MonoBehaviour
{
    public GameObject m_PlayerSlotPrefab;

    public TextMeshProUGUI roomName;
    public GameObject PlayerSlotParent;
    // Start is called before the first frame update
    public static UI_PlayerSlot Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SetRoomName()
    {
        roomName.text=PhotonManager.Instance.roomname;
    }
    public void AddPlayerSlot(string playerName)
    {
        GameObject newPlayer=Instantiate<GameObject>(m_PlayerSlotPrefab);
        newPlayer.transform.SetParent(PlayerSlotParent.transform);
        newPlayer.transform.localScale=new Vector3(1,1,1);
        
        var texts = newPlayer.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var txtComponent in texts)
        {
            if (txtComponent.name == "Text_Info")
            {
                txtComponent.text = playerName;
            }
        }
        
        var toggles = newPlayer.GetComponentsInChildren<Toggle>();
        if (playerName == PhotonNetwork.LocalPlayer.NickName)
        {
            foreach (var toggle in toggles)
            {
                toggle.gameObject.SetActive(false);
            }
        }
            
        
    }
    
    public void DelPlayerSlot(string playerName)
    {
        for (int i = 0; i < PlayerSlotParent.transform.childCount; ++i)
        {
            var texts = PlayerSlotParent.transform.GetChild(i).GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var txtComponent in texts)
            {
                if (txtComponent.name == "Text_Info")
                {
                    if (txtComponent.text == playerName)
                    {
                        Destroy(PlayerSlotParent.transform.GetChild(i).gameObject);
                    }
                }
            }
        }
    }
}
