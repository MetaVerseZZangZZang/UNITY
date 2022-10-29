using System.Collections;
using System.Collections.Generic;
using Agora.Rtc;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;


public class UI_MainPanel : MonoBehaviour
{
    public GameObject m_Prefab;

    //public Text m_Text;
    public static UI_MainPanel Instance;
    
    public GameObject chatTextParent;
    public RawImage myCam;
    //public Text newText;



    // sm이 수정했다 선언
    public GameObject keyWord;
    public GameObject keywordPanel;



    private void Awake()
    {
        Instance = this;
        Hide();
    }

    public void InstantiateKeywordText(string msg)
    {
        GameObject keywordText = Instantiate<GameObject>(keyWord);
        keywordText.transform.SetParent(keywordPanel.transform);
        keywordText.transform.localScale = new Vector3(1, 1, 1);
        //keywordText.transform.position = new Vector3(0, 0, 0);
        keywordText.GetComponent<Text>().text = msg;
    }


    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        UI_CharPanel.Instance.StopCam();
        this.gameObject.SetActive(true);
        AgoraManager.Instance.Join();
        Server.Instance.ChatStart();
        Debug.LogError(111);
    }

    public void Leave()
    {
        AgoraManager.Instance.Leave();
        PhotonNetwork.LeaveRoom();
        UI_MainPanel.Instance.Hide();
        UI_CharPanel.Instance.Show();
        Server.Instance.ChatEnd();
        
    }
    
    public void ChatRPC(string msg)
    {
        GameObject newText=Instantiate<GameObject>(m_Prefab);
        newText.transform.SetParent(chatTextParent.transform);
        newText.transform.localScale=new Vector3(1,1,1);
        newText.GetComponent<Text>().text = msg;
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
