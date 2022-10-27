using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharPanel : MonoBehaviour
{
    
    public static UI_CharPanel Instance;

    public InputField inputName;

    public Text Name;

    public Text info;
    // Start is called before the first frame update
    //public void Connect() => PhotonNetwork.ConnectUsingSettings();
    //public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();
    
    /*
    public void Connect() => PhotonNetwork.ConnectUsingSettings();
    public override void OnConnectedToMaster()
    {
        Debug.LogError("Connected");
        
        PhotonNetwork.JoinLobby();
    }
    */
    private void Awake()
    {
        Instance = this;
        Hide();
    }

    /*
    private void Start()
    {
        Connect();

    }
    */
    
    // Update is called once per frame

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
        
        Name.text = inputName.text;
        
        
    }
    
    
    public void CreateBtn()
    {
        UI_CreateMapPanel.Instance.Show();
    }
    
    public void JoinBtn()
    {
        UI_JoinRoom.Instance.Show();
    }
}
