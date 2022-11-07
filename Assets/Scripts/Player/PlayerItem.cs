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
    }

    public void SetPlayer(Image item)
    {
        if (item.tag == "Shirts")
        {
            shirts.sprite = item.sprite;
            playerProperties["shirts"] = Array.IndexOf(shirtsSprites, item.sprite);
            PhotonNetwork.SetCustomProperties(playerProperties);
        }
        
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
    }
}
