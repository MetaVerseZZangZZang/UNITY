using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_LobbyPanel : MonoBehaviour
{
    
    public static UI_LobbyPanel Instance;

    public Text Name;
    
    public RawImage myCam;
    
    public Sprite cameraOff;
    
    private WebCamTexture camTexture;
    public Toggle myCamToggle;
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
        gameObject.SetActive(false);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
        //Name.text = UI_StartPanel.Instance.userName;
        ShowCam();
        myCamToggle.isOn = false;
        ScreenShareWhileVideoCall.Instance.camFlag = true;
        //ScreenShareWhileVideoCall.Instance.voiceFlag = true;
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
        myCam.texture = cameraOff.texture;

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
        if (!toggle.isOn)
        {
            ShowCam();
            ScreenShareWhileVideoCall.Instance.camFlag = true;
        }
        else
        {
            StopCam();
            ScreenShareWhileVideoCall.Instance.camFlag = false;
        }
        
    }


    public void VoiceToggle(Toggle toggle)
    {
    }
}