using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using ChatProto;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance;
    //public GameObject map;
    public List<RoomInfo> m_roomList = new List<RoomInfo>();
    //public Transform ChatPeersContent;

    public List<string> nameList = new List <string>();

    void Awake()
    {
        Instance = this;
    }
    
    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.LocalPlayer.NickName = UI_StartPanel.Instance.userName;
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
        //PhotonView.Instantiate(map, new Vector3(0, 0, 0), Quaternion.identity);
        PhotonNetwork.Instantiate("Prefabs/Player", Vector3.zero, Quaternion.identity);
        //for (int i = 0; i < ChatText.Length; i++) ChatText[i].text = "";

        //룸에 있는 사람들만 통화가 가능하게 만들거니까 그거를 플레이어 닉네임이 같은 사람으로만 해서 토글을 볼수 있게 해주면 되어요

        foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
        {
            nameList.Add(p.NickName);
            
            UI_PlayerSlot.Instance.AddPlayerSlot(p.NickName);
        }

        /*
        ChatManager.Instance.Login(PhotonNetwork.LocalPlayer.NickName);
        
        if (PhotonNetwork.LocalPlayer.NickName == PhotonNetwork.MasterClient.NickName)
            ChatUIManager.Instance.VideoCall();
        */
    }

    public void LeaveRoom()
    {
        foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
        {
            nameList.Remove(p.NickName);
            UI_PlayerSlot.Instance.DelPlayerSlot(p.NickName);
        }
    }
    
    public void roomSelect(RoomInfo room)
    {
        PhotonNetwork.JoinRoom(room.Name);
        //MainUIManager.Instance.RoomName.text = "Welcome to\n"+room.Name;
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        //ChatRPC("<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
        nameList.Add(newPlayer.NickName);
        
        UI_PlayerSlot.Instance.AddPlayerSlot(newPlayer.NickName);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        nameList.Remove(otherPlayer.NickName);
        UI_PlayerSlot.Instance.DelPlayerSlot(otherPlayer.NickName);
    }
    
    
}

