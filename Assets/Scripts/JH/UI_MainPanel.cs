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
    public GameObject BackGround;
    public Text roomName;
    public GameObject PlayerSlot;
    public GameObject PlayerSlotBtn;
    public GameObject BottomOption;
    public GameObject ChatBox;
    public int count = 0;
    public int chatCount = 0;
    public Button chatStartBtn;
    public bool conferenceStart = false;
    private QuarterViewCam quarter;
    private ThirdPersonCam third;
    public Camera cam;
    public Button camButton;
    public Toggle MyCamToggle;
    public Toggle MyVoiceToggle;
    
    private void Awake()
    {
        Instance = this;
        Hide();
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
        conferenceStart = false;
    }

    public void Show(int mapnum)
    {
        
        Server.Instance.ChatStart();
        UI_LobbyPanel.Instance.StopCam();
        ScreenShareWhileVideoCall.Instance.AgoraStart();
        gameObject.SetActive(true);
        conferenceStart = true;
        
        for (int i = 0; i < 2; i++)
        {
            if (i == mapnum)
            {
                BackGround.transform.GetChild(mapnum).gameObject.SetActive(true);
                continue;
            }
            BackGround.transform.GetChild(i).gameObject.SetActive(false);
        }
        
        
        roomName.text = PhotonManager.Instance.roomname;
        
        quarter = cam.GetComponent<QuarterViewCam>();
        third = cam.GetComponent<ThirdPersonCam>();
        
        quarter.enabled = true;
        third.enabled = false;

        MyCamToggle.isOn = !ScreenShareWhileVideoCall.Instance.camFlag;
        MyVoiceToggle.isOn = !ScreenShareWhileVideoCall.Instance.voiceFlag;
    }
    
    public void CamButtonClick()
    {
        string camText = camButton.GetComponentInChildren<Text>().text;
        if (camText.Contains("쿼터뷰"))
        {
            quarter.enabled = false;
            third.enabled = true;
            camButton.GetComponentInChildren<Text>().text = "3인칭";
        }
        else
        {
            quarter.enabled = true;
            third.enabled = false;
            camButton.GetComponentInChildren<Text>().text = "쿼터뷰";
        }
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
        MyCamControl(!toggle.isOn);
        ScreenShareWhileVideoCall.Instance.camFlag = !toggle.isOn;
    }
    
    public void VoiceToggle(Toggle toggle)
    {
        MyVoiceControl(!toggle.isOn);
        ScreenShareWhileVideoCall.Instance.voiceFlag = !toggle.isOn;
    }


    public void MyCamControl(bool flag)
    {
        if (flag)  //켜기
        {
            myCam.transform.GetChild(0).gameObject.SetActive(false);
        }
        else      //끄기
        {
            myCam.transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    
    public void MyVoiceControl(bool flag)
    {
        if (flag)  //켜기
        {
            ScreenShareWhileVideoCall.Instance.playbackSpeaker(true);
            AudioLoudnessDetection.Instance.joined = true;
        }
        else      //끄기
        {
            ScreenShareWhileVideoCall.Instance.playbackSpeaker(false);
            AudioLoudnessDetection.Instance.joined = false;
        }
    }
    
    public void friendCamON(PlayerItem playerItem)
    {
        VideoSurface RemoteView = getVSByPlayerItem(playerItem);
        if(getVSByPlayerItem(playerItem)!=null)
            RemoteView.transform.GetChild(0).gameObject.SetActive(false);
        foreach (UI_PlayerSlotItem item in UI_PlayerSlot.Instance.PlayerSlotList)
        {
            if (item.name.text == playerItem.Nickname)
            {
                item.camControl(true);
            }
        }
    }

    public void friendCamOff(PlayerItem playerItem)
    {
        VideoSurface RemoteView = getVSByPlayerItem(playerItem);
        if(getVSByPlayerItem(playerItem)!=null)
            RemoteView.transform.GetChild(0).gameObject.SetActive(true);
        
        foreach (UI_PlayerSlotItem item in UI_PlayerSlot.Instance.PlayerSlotList)
        {
            if (item.name.text == playerItem.Nickname)
            {
                item.camControl(false);
            }
        }
    }
    
    /*
    public void friendVoiceOn(PlayerItem playerItem)
    {
        VideoSurface RemoteView = getVSByPlayerItem(playerItem);
        foreach (UI_PlayerSlotItem item in UI_PlayerSlot.Instance.PlayerSlotList)
        {
            if (item.name.text == playerItem.Nickname)
            {
                item.voiceControl(true);
            }
        }
    }

    public void friendVoiceOff(PlayerItem playerItem)
    {
        VideoSurface RemoteView = getVSByPlayerItem(playerItem);
        foreach (UI_PlayerSlotItem item in UI_PlayerSlot.Instance.PlayerSlotList)
        {
            if (item.name.text == playerItem.Nickname)
            {
                item.voiceControl(false);
            }
        }
    }
*/
    public VideoSurface getVSByPlayerItem(PlayerItem playerItem)
    {
        VideoSurface vs = null;

        if (ScreenShareWhileVideoCall.FriendCamList.Count >= 1)
        {

            foreach (VideoSurface s in ScreenShareWhileVideoCall.FriendCamList)
            {
                if (s.Uid ==playerItem.playerObjectID)
                {
                    vs = s;
                    break;
                }
            }
        }

        return vs;
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

