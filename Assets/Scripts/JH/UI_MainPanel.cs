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
using Button = UnityEngine.UI.Button;
using Toggle = UnityEngine.UI.Toggle;


public class UI_MainPanel : MonoBehaviour
{
    //public Text m_Text;
    public static UI_MainPanel Instance;
   
    public RawImage myCam;
    // sm이 수정했다 선언
    //public GameObject keyWord;
    //public GameObject keywordPanel;
    //public GameObject BackGround;
    public Text roomName;
    public GameObject PlayerSlot;
    public GameObject PlayerSlotBtn;
    public GameObject BottomOption;
    public GameObject ChatBox;
    public int count = 0;
    public int chatCount = 0;
    public Button chatStartBtn;
    
    private void Awake()
    {
        Instance = this;
        Hide();
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        
        Server.Instance.ChatStart();
        UI_LobbyPanel.Instance.StopCam();
        ScreenShareWhileVideoCall.Instance.AgoraStart();
        gameObject.SetActive(true);
        //BackGround.transform.GetChild(mapnum).gameObject.SetActive(true);
        myCam.transform.GetChild(0).gameObject.SetActive(false);
        
        /*
        for (int i = 0; i < 4; i++)
        {
            if (i == mapnum)
                continue;
            BackGround.transform.GetChild(i).gameObject.SetActive(false);
        }
        */
        roomName.text = PhotonManager.Instance.roomname;
    }

    public void ChatStartBtn()
    {
        if (Server.Instance.AIFlag == false)
        {
            AudioLoudnessDetection.Instance.joined = true;
            Server.Instance.AIFlag = true;
            chatStartBtn.GetComponentInChildren<Text>().text = "End";
        }

        else
        {
            AudioLoudnessDetection.Instance.joined = false;
            Server.Instance.AIFlag = false;
            chatStartBtn.GetComponentInChildren<Text>().text = "Start";
        }
    }

    public void GiveMeAI()
    {
        Server.Instance.AIResultEmit();
    }

    public void Leave()
    {
        ScreenShareWhileVideoCall.Instance.Leave();
        PhotonNetwork.LeaveRoom();
        Hide();
        UI_LobbyPanel.Instance.Show();
        Server.Instance.MeetingEnd();
        AudioLoudnessDetection.Instance.joined = false;

    }


    public void CamToggle(Toggle toggle)
    {

        AgoraManager.camFlag = !toggle.isOn;
        if (!toggle.isOn)
        {
            myCam.transform.GetChild(0).gameObject.SetActive(false);
            //AgoraManager.Instance.RtcEngine.EnableLocalVideo(true);

        }
        else
        {
            //AgoraManager.Instance.RtcEngine.EnableLocalVideo(false);
            myCam.transform.GetChild(0).gameObject.SetActive(true);

        }
    }
    
    public void VoiceToggle(Toggle toggle)
    {
        /*
        AgoraManager.voiceFlag = !toggle.isOn;
        if (AgoraManager.voiceFlag)
        {
            AgoraManager.Instance.RtcEngine.EnableLocalAudio(true);
        }
        else
        {
            AgoraManager.Instance.RtcEngine.EnableLocalAudio(false);
        }
        */
    }

    public void friendCamOff(VideoSurface RemoteView)
    {
        RemoteView.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void friendCamON(VideoSurface RemoteView)
    {
        RemoteView.transform.GetChild(0).gameObject.SetActive(false);
    }
    
    
    public void PlayerSlotBtnClick()
    {
        PlayerSlot.GetComponent<Animation>().Play("SlotUpdateAnim");
        PlayerSlotBtn.SetActive(false);
    }
    
    public void PlayerSlotRemoveBtnClick()
    {
        PlayerSlot.GetComponent<Animation>().Play("SlotRemoveAnim");
        PlayerSlotBtn.SetActive(true);
    }
    
    public void ChatBtnClick()
    {
        if (chatCount % 2 == 0)
        {
            ChatBox.GetComponent<Animation>().Play("ChatUpdateAnim");
        }
        else
        {
            ChatBox.GetComponent<Animation>().Play("ChatRemoveAnim");
        }

        chatCount++;
    }
    
    public void ChatBtnRemoveClick()
    {
        ChatBox.GetComponent<Animation>().Play("ChatRemoveAnim");
        chatCount = 0;
    }

    public void BottomOptionClick()
    {
        if (count%2==0)
        {
            BottomOption.GetComponent<Animation>().Play("BottomOptionUpAnim");
        }
        else
        {
            BottomOption.GetComponent<Animation>().Play("BottomOptionDownAnim");
        }

        count++;
    }
}

