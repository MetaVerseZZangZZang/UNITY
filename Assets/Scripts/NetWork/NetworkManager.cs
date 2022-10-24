using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    
    public InputField nickNamgeInput;
    public GameObject nickNamePanel;
    public GameObject respawnPanel;


    public GameObject webViewPanel;


    //포톤 접속 상태 보여주는 텍스트
    public Text serverStatusText;
    //로비 접속 현황 텍스트
    public Text lobbyInfoText;
    //룸을 만들때 표시하는 텍스트(방이름)
    //텍스트 inputfield의 text로 바꾸어 줘야함
    public Text inputFieldRoomNameText;
    //로비에서 방 입장시 표시할 닉네임
    public Text playerListText;


    public GameObject loginPanel;
    //로비패널
    public GameObject lobbyPanel;


    //리스트
    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;


    //방선택할수 있는 버selectRoomBtn
    public Button[] selectRoomBtn;
    public Button roomListPreviousBtn;
    public Button roomListNextBtn;



    public Text test2;

    //join버튼을 눌렀을때 보여줄 UI
    public GameObject roomList_UI;

    //채팅 기능 panel Text
    public Text[] chatting_labelList;

    //맵 생성할 프리팹
    public GameObject map;


    #region 처음 접속 후 방리스트 접속
    

    void RoomRenewal()
    {
        playerListText.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            playerListText.text += ("player : ", PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", "));
        playerListText.text = PhotonNetwork.CurrentRoom.Name + " / " + PhotonNetwork.CurrentRoom.PlayerCount + "명 / " + PhotonNetwork.CurrentRoom.MaxPlayers + "최대";
    }
    #endregion


    public void Spawn()
    {
        PhotonNetwork.Instantiate("Prefabs/Player", Vector3.zero, Quaternion.identity);
        respawnPanel.SetActive(false);
    }
    //public override void OnConnectedToMaster()
    //{
    //    //포톤 네트워크 플레이어 닉네임 등록하는 곳에 사용자가 기입한 텍스트로 포톤 네트워크상에 플레이어 닉네임을 등록한다
    //    PhotonNetwork.LocalPlayer.NickName = nickNamgeInput.text;
    //}


    #region 로비 입장 코드



    #endregion


    // 로비 입장
    void Awake() => Screen.SetResolution(960, 540, false);

    void Update()
    {
        serverStatusText.text = PhotonNetwork.NetworkClientState.ToString();
        lobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "로비 / " + PhotonNetwork.CountOfPlayers + "접속";
        if (Input.GetKeyDown(KeyCode.Escape) && roomList_UI.activeSelf == true)
        {
            roomList_UI.SetActive(false);
        }
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();
    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby()
    {
        loginPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause)
    {
        loginPanel.SetActive(true);
        lobbyPanel.SetActive(false);
    }



    //chatting 관련 알려주는 코
    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < chatting_labelList.Length; i++)
            if (chatting_labelList[i].text == "")
            {
                isInput = true;
                chatting_labelList[i].text = msg;
                break;
            }
        if (!isInput) // 꽉차면 한칸씩 위로 올림
        {
            for (int i = 1; i < chatting_labelList.Length; i++) chatting_labelList[i - 1].text = chatting_labelList[i].text;
            chatting_labelList[chatting_labelList.Length - 1].text = msg;
        }
    }





    public void WebViewActiveBtn()
    {
        webViewPanel.SetActive(true);
    }


    //// 방 입장
    public void CreateRoom() => PhotonNetwork.CreateRoom(test2.text == "" ? "Room" + Random.Range(0, 100) : test2.text, new RoomOptions { MaxPlayers = 4 });

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnJoinedRoom()
    {
        //RoomPanel.SetActive(true);
        RoomRenewal();

        PhotonView.Instantiate(map, new Vector3(0, 0, 0), Quaternion.identity);
        lobbyPanel.SetActive(false);
        if (roomList_UI.activeSelf == true) { roomList_UI.SetActive(false); }

        Spawn();


        //for (int i = 0; i < ChatText.Length; i++) ChatText[i].text = "";
    }

    //누가 들어왔는지 누가 나갔는지 텍스트 보여주는 함	
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        RoomRenewal();
        ChatRPC("<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        RoomRenewal();
        ChatRPC("<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
    }

    //join버튼 클릭 이벤트 함수
    public void JoinBtn() => roomList_UI.SetActive(true);

    //방 리스트 생성
    // ◀버튼 -2 , ▶버튼 -1 , 셀 숫자
    public void RoomListClick(int num)
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else PhotonNetwork.JoinRoom(myList[multiple + num].Name);
        MyListRenewal();
    }

    void MyListRenewal()
    {
        // 최대페이지
        maxPage = (myList.Count % selectRoomBtn.Length == 0) ? myList.Count / selectRoomBtn.Length : myList.Count / selectRoomBtn.Length + 1;

        // 이전, 다음버튼
        roomListPreviousBtn.interactable = (currentPage <= 1) ? false : true;
        roomListNextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // 페이지에 맞는 리스트 대입
        multiple = (currentPage - 1) * selectRoomBtn.Length;
        for (int i = 0; i < selectRoomBtn.Length; i++)
        {
            selectRoomBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
            selectRoomBtn[i].transform.GetChild(0).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
            selectRoomBtn[i].transform.GetChild(1).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
        }
        MyListRenewal();
    }

}