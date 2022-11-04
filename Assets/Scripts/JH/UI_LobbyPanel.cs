using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class UI_LobbyPanel : MonoBehaviour
{
    
    public static UI_LobbyPanel Instance;

    public Text Name;
    
    public RawImage myCam;
    
    public Sprite cameraOff;
    
    private WebCamTexture camTexture;


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
        Name.text = UI_StartPanel.Instance.userName;
        ShowCam();
    }

    private void ShowCam()
    {
        WebCamDevice device = WebCamTexture.devices[0];
        camTexture = new WebCamTexture(device.name);
        myCam.texture = camTexture;
        camTexture.Play();
    }
    
    public void StopCam()
    {
        if (camTexture != null)
        {
            myCam.texture = null;
            camTexture.Stop();
        }
    }
    
    public void CreateBtn()
    {
        UI_CreateMapPanel.Instance.Show();
    }
    
    public void JoinBtn()
    {
        UI_JoinRoom.Instance.Show();
    }

    public void CamToggle(Toggle toggle)
    {
        if (toggle.isOn)
        {
            ShowCam();
        }
        else
        {
            StopCam();
            myCam.texture = cameraOff.texture;
        }
        
        AgoraManager.camFlag = toggle.isOn;
    }

    public void VoiceToggle(Toggle toggle)
    {
        AgoraManager.voiceFlag = toggle.isOn;
    }
}