using System;
using System.Collections;
using System.Collections.Generic;
using Agora.Rtc;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Toggle = UnityEngine.UI.Toggle;


public class UI_MainPanel : MonoBehaviour
{
    //public Text m_Text;
    public static UI_MainPanel Instance;
   
    public RawImage myCam;
    // sm이 수정했다 선언
    //public GameObject keyWord;
    //public GameObject keywordPanel;
    public GameObject BackGround;


    private void Awake()
    {
        Instance = this;
        Hide();
    }
    
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show(int mapnum)
    {
        NetStart();
        this.gameObject.SetActive(true);
        BackGround.transform.GetChild(mapnum).gameObject.SetActive(true);
        
        for (int i = 0; i < 4; i++)
        {
            if (i == mapnum)
                continue;
            BackGround.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void NetStart()
    {
        UI_LobbyPanel.Instance.StopCam();
        AgoraManager.Instance.Join();
        Server.Instance.ChatStart();
        AudioLoudnessDetection.Instance.joined = true;
    }

    public void Leave()
    {
        AgoraManager.Instance.Leave();
        PhotonNetwork.LeaveRoom();
        Hide();
        UI_LobbyPanel.Instance.Show();
        Server.Instance.ChatEnd();
        AudioLoudnessDetection.Instance.joined = false;

    }


    public void CamToggle(Toggle toggle)
    {

        AgoraManager.camFlag = toggle.isOn;
        if (toggle.isOn)
        {
            myCam.transform.GetChild(0).gameObject.SetActive(false);
            AgoraManager.Instance.RtcEngine.EnableLocalVideo(true);

        }
        else
        {
            AgoraManager.Instance.RtcEngine.EnableLocalVideo(false);
            myCam.transform.GetChild(0).gameObject.SetActive(true);

        }
    }
    
    public void VoiceToggle(Toggle toggle)
    {
        AgoraManager.voiceFlag = toggle.isOn;
        if (toggle.isOn)
        {
            AgoraManager.Instance.RtcEngine.EnableLocalAudio(true);
        }
        else
        {
            AgoraManager.Instance.RtcEngine.EnableLocalAudio(false);
        }
    }

    public void friendCamOff(VideoSurface RemoteView)
    {
        RemoteView.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void friendCamON(VideoSurface RemoteView)
    {
        RemoteView.transform.GetChild(0).gameObject.SetActive(false);
    }


}

