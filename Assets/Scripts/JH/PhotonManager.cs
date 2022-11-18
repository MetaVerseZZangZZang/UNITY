using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Policy;
using AdvancedPeopleSystem;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.StructWrapping;
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
    public string roomname;
    
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
        roomname = UI_CreateMapPanel.Instance.RoomNameInputField.text== "" ? "Room" + Random.Range(0, 100) : UI_CreateMapPanel.Instance.RoomNameInputField.text;
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

    private PlayerItem m_PlayerItem;
    public override void OnJoinedRoom()
    {
        GameObject player=PhotonNetwork.Instantiate("Prefabs/Character", Vector3.zero, Quaternion.Euler(new Vector3(0,180,0)));

        player.GetComponent<CharacterCustomization>().SetElementByIndex(CharacterElementType.Hair,CharacterManager.Instance.selectedHairIndex );

        //m_PlayerItem.Sethair()
        /*
        int selectedHairIndex = CharacterManager.Instance.selectedHairIndex;
        
        player.GetComponent<CharacterCustomization>().SetElementByIndex(CharacterElementType.Hair,selectedHairIndex);
        
        Hashtable playerCP=new Hashtable { { "hair", selectedHairIndex } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerCP);
        */
        //player.GetComponent<PlayerItem>().SetPlayerInfo(PhotonNetwork.LocalPlayer);
        
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            UI_PlayerSlot.Instance.AddPlayerSlot(p.NickName);
        }
        
        Hashtable CP = PhotonNetwork.CurrentRoom.CustomProperties;
        UI_MainPanel.Instance.Show((int)CP["Map"]);
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
        Debug.Log("OnPlayerEnteredRoom "+newPlayer.NickName);
        Hashtable cp = newPlayer.CustomProperties;
        Debug.Log("newPlayer.CustomProperties[hair] "+(int)cp["hair"]);
        
        
        
        UI_PlayerSlot.Instance.AddPlayerSlot(newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //nameList.Remove(otherPlayer.NickName);
        //UpdatePlayerList();
        UI_PlayerSlot.Instance.DelPlayerSlot(otherPlayer.NickName);
    }

    
}

