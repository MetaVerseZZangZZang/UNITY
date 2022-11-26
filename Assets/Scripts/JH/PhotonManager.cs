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
    public GameObject player;
    
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

    public override void OnJoinedRoom()
    {

        player=PhotonNetwork.Instantiate("Prefabs/"+CharCustomManager.Instance.selectedGender+"Character", new Vector3(1,0,35), Quaternion.Euler(new Vector3(0,0,0)));
        player.GetComponent<PlayerItem>().Nickname = PhotonNetwork.LocalPlayer.NickName;
        player.GetComponent<CharacterCustomization>().SetElementByIndex(CharacterElementType.Hair,CharCustomManager.Instance.selectedHairIndex );
        player.GetComponent<CharacterCustomization>().SetElementByIndex(CharacterElementType.Shirt,CharCustomManager.Instance.selectedShirtsIndex );
        player.GetComponent<CharacterCustomization>().SetElementByIndex(CharacterElementType.Pants,CharCustomManager.Instance.selectedPantsIndex );
        player.GetComponent<CharacterCustomization>().SetElementByIndex(CharacterElementType.Shoes,CharCustomManager.Instance.selectedShoesIndex );
        player.GetComponent<CharacterCustomization>().SetElementByIndex(CharacterElementType.Hat,CharCustomManager.Instance.selectedHatIndex );
        
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            UI_PlayerSlot.Instance.AddPlayerSlot(p.NickName);
        }
        
        Hashtable CP = PhotonNetwork.CurrentRoom.CustomProperties;
        UI_MainPanel.Instance.Show((int)CP["Map"]);
        
        playerItemList.Add(player.GetComponent<PlayerItem>());
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
        
        UI_PlayerSlot.Instance.DelAll();
    }
    
    public void roomSelect(RoomInfo room)
    {
        PhotonNetwork.JoinRoom(room.Name);
        //MainUIManager.Instance.RoomName.text = "Welcome to\n"+room.Name;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UI_PlayerSlot.Instance.AddPlayerSlot(newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //nameList.Remove(otherPlayer.NickName);
        //UpdatePlayerList();
        PlayerItem toRemove = null;
        for (int i = 0; i < playerItemList.Count; i++)
        {
            if (playerItemList[i].Nickname == otherPlayer.NickName)
            {
                toRemove = playerItemList[i];
            }
        }

        playerItemList.Remove(toRemove);
        UI_PlayerSlot.Instance.DelPlayerSlot(otherPlayer.NickName);
    }

    
}

