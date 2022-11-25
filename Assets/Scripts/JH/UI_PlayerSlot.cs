using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerSlot : MonoBehaviour
{
    public UI_PlayerSlotItem m_PlayerSlotPrefab;
    public Transform playerSlotParent;
    public TextMeshProUGUI roomName;
    
    // Start is called before the first frame update
    public static UI_PlayerSlot Instance;
    public List<UI_PlayerSlotItem> PlayerSlotList = new List<UI_PlayerSlotItem>();
    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        roomName.text=PhotonManager.Instance.roomname;
    }
    public void AddPlayerSlot(string playerName)
    {
        UI_PlayerSlotItem newPlayer=Instantiate<UI_PlayerSlotItem>(m_PlayerSlotPrefab);
        newPlayer.gameObject.transform.SetParent(playerSlotParent);
        newPlayer.Init(playerName);
        PlayerSlotList.Add(newPlayer);
    }
    
    public void DelPlayerSlot(string playerName)
    {
        foreach (UI_PlayerSlotItem psi in PlayerSlotList)
        {
            Debug.Log(psi.name);
            if (psi.name.text == playerName)
            {
                psi.Destory();
                PlayerSlotList.Remove(psi);
                break;
            }
        }

    }

    public void DelAll()
    {
        foreach (UI_PlayerSlotItem psi in PlayerSlotList)
        {
            psi.Destory();
        }

        PlayerSlotList.Clear();
    }
}
