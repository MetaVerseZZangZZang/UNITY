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

public class NetManager : MonoBehaviourPunCallbacks
{
    public static NetManager Instance;
    //public GameObject map;
    public List<RoomInfo> m_roomList = new List<RoomInfo>();
    //public Transform ChatPeersContent;

    public List<string> nameList = new List <string>();

    void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        Login();
    }
    
    public void Login()
    {
        if (string.IsNullOrEmpty(UI_StartPanel.Instance.userName))
        {
            MessageManager.Instance.ShowMessage("please input username!");
            return;
        } 
        ChatManager.Instance.Login(UI_StartPanel.Instance.userName);
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

        //if (PhotonNetwork.LocalPlayer.NickName!=PhotonNetwork.MasterClient.NickName)
        //    roomNameList.Add(PhotonNetwork.MasterClient.NickName);
        
        foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
        {
            nameList.Add(p.NickName);
        }

        ChatManager.Instance.Login(PhotonNetwork.LocalPlayer.NickName);
    }

    public void roomSelect(RoomInfo room)
    {
        PhotonNetwork.JoinRoom(room.Name);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        //ChatRPC("<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
        nameList.Add(newPlayer.NickName);

    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        nameList.Remove(otherPlayer.NickName);
        //ChatRPC("<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
    }
    
    
}

