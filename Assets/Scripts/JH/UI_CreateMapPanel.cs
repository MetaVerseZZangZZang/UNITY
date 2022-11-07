using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class UI_CreateMapPanel : MonoBehaviour
{
    public static UI_CreateMapPanel Instance;

    public InputField RoomNameInputField;

    private void Awake()
    {
        Instance = this;
        Hide();
    }
    
    // Start is called before the first frame update
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(RoomNameInputField.text == "" ? "Room" + Random.Range(0, 100) : RoomNameInputField.text, new RoomOptions { MaxPlayers = 6,BroadcastPropsChangeToAll = true});
        Hide();
        UI_LobbyPanel.Instance.Hide();
        UI_MainPanel.Instance.Show();
    }
}
