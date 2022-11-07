using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    public Text playerName;
    public Image shirts;
    public Sprite[] shirtsSprites;
    // Start is called before the first frame update
    
    private ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    
    private Player player;
    
    public void SetPlayerInfo(Player _player)
    {
        playerName.text = _player.NickName;
        player = _player;
        playerProperties["shirts"] = Array.IndexOf(shirtsSprites, UI_Character.Instance.SelectedShirts);
        Debug.Log("UI_Character.Instance.SelectedShirts "+UI_Character.Instance.SelectedShirts);

        foreach (Sprite i in shirtsSprites)
        {
            
            Debug.Log("shirtsSprites "+i);
        }
        Debug.Log("Array.IndexOf(shirtsSprites, UI_Character.Instance.SelectedShirts)"+Array.IndexOf(shirtsSprites, UI_Character.Instance.SelectedShirts));
        PhotonNetwork.SetCustomProperties(playerProperties);
        UpdatePlayerItem(player);
    }


    public override void OnPlayerPropertiesUpdate(Player targetPlayer,ExitGames.Client.Photon.Hashtable playerProperties)
    {
        if (player == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
        }
    }

    void UpdatePlayerItem(Player player)
    {
        if (player.CustomProperties.ContainsKey("shirts"))
        {
            shirts.sprite = shirtsSprites[(int)player.CustomProperties["shirts"]];
            playerProperties["shirts"] = (int)player.CustomProperties["shirts"];
        }
        else
        {
            playerProperties["shirts"] = 0;
        }
    }
}
