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
    
    public RawImage myCam;
    
    public Sprite cameraOff;
    
    private WebCamTexture camTexture;
    private int currentIndex = 0;

    public bool camFlag=true;

    public bool voiceFlag=true;
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
        showCam();

    }

    private void showCam()
    {
        WebCamDevice device = WebCamTexture.devices[currentIndex];
        camTexture = new WebCamTexture(device.name);
        //camTexture = new Vector2(-1, 1);
        myCam.texture = camTexture;
        camTexture.Play();
    }

    public void StopCam()
    {
        myCam.texture = null;
        camTexture.Stop();
        camTexture = null;
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
            camFlag = true;
            showCam();
        }
        else
        {
            camFlag = false;
            StopCam();
            myCam.texture = cameraOff.texture;
        }
    }

    public void VoiceToggle(Toggle toggle)
    {
        voiceFlag = toggle.isOn;
    }
}
