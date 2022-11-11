using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Policy;
using ExitGames.Client.Photon;
//using ChatProto;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Hashtable=ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance;
    //public GameObject map;
    public List<RoomInfo> m_roomList = new List<RoomInfo>();
    //public Transform ChatPeersContent;

    public List<string> nameList = new List <string>();
    public List<PlayerItem> playerItemList = new List<PlayerItem>();
    
    void Awake()
    {
        Instance = this;
    }
    
    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.LocalPlayer.NickName = UI_StartPanel.Instance.userName;
    }

    public void CreateRoom()
    {
        string roomname = UI_CreateMapPanel.Instance.RoomNameInputField.text== "" ? "Room" + Random.Range(0, 100) : UI_CreateMapPanel.Instance.RoomNameInputField.text;
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 6;
        roomOptions.CustomRoomProperties = new Hashtable() { { "Map",UI_CreateMapPanel.Instance.mapNum } };
        roomOptions.BroadcastPropsChangeToAll = true;
        PhotonNetwork.CreateRoom(roomname == "" ? "Room" + Random.Range(0, 100) : roomname, roomOptions);

    }
    
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();

        PhotonNetwork.LocalPlayer.NickName = UI_StartPanel.Instance.nameInput.text;
        
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!m_roomList.Contains(roomList[i])) m_roomList.Add(roomList[i]);
                else m_roomList[m_roomList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (m_roomList.IndexOf(roomList[i]) != -1) m_roomList.RemoveAt(m_roomList.IndexOf(roomList[i]));

        }

        UI_JoinRoom.Instance.Refresh();
    }


    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Prefabs/Player", Vector3.zero, Quaternion.identity);
;        //UpdatePlayerList();
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            //nameList.Add(p.NickName);
            
            UI_PlayerSlot.Instance.AddPlayerSlot(p.NickName);
        }

        Hashtable CP = PhotonNetwork.CurrentRoom.CustomProperties;
        Debug.Log(CP["Map"]);

        /*
        ChatManager.Instance.Login(PhotonNetwork.LocalPlayer.NickName);
        
        if (PhotonNetwork.LocalPlayer.NickName == PhotonNetwork.MasterClient.NickName)
            ChatUIManager.Instance.VideoCall();
        */
    }

    public void LeaveRoom()
    {
        foreach (PlayerItem item in playerItemList)
        {
            Destroy(item.gameObject);
        }
        playerItemList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }
        
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            //nameList.Remove(p.NickName);
            UI_PlayerSlot.Instance.DelPlayerSlot(p.NickName);
        }
    }
    
    public void roomSelect(RoomInfo room)
    {
        PhotonNetwork.JoinRoom(room.Name);
        //MainUIManager.Instance.RoomName.text = "Welcome to\n"+room.Name;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //ChatRPC("<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
        //nameList.Add(newPlayer.NickName);
        //UpdatePlayerList();
        UI_PlayerSlot.Instance.AddPlayerSlot(newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //nameList.Remove(otherPlayer.NickName);
        //UpdatePlayerList();
        UI_PlayerSlot.Instance.DelPlayerSlot(otherPlayer.NickName);
    }

    
    void UpdatePlayerList()
    {
        foreach (PlayerItem item in playerItemList)
        {
            Destroy(item.gameObject);
        }
        playerItemList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            GameObject newPlayerItem = PhotonNetwork.Instantiate("Prefabs/Player", Vector3.zero, Quaternion.identity);
            
            
            newPlayerItem.transform.localScale = new Vector3(1, 1, 1);
            PlayerItem newPlayer = newPlayerItem.GetComponent<PlayerItem>();
            //newPlayer.SetPlayerInfo(player.Value);
            playerItemList.Add(newPlayer);
        }
    }
    
}

