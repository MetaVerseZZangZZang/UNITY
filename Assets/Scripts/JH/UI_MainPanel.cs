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
    public GameObject m_ChatTextPrefab;  //stt 결과
    public GameObject m_AIImagePrefab;
    public GameObject m_AIGraphPrefab;
    public GameObject m_AITextPrefab;   //추천, keyword

    public GameObject m_PlayerSlotPrefab;
    //public Text m_Text;
    public static UI_MainPanel Instance;
    
    public GameObject AIParent;
    public GameObject PlayerSlotParent;
    public RawImage myCam;
    //public Text newText;


    public ScrollRect m_ScrollRect;
    // sm이 수정했다 선언
    //public GameObject keyWord;
    //public GameObject keywordPanel;



    private void Awake()
    {
        Instance = this;
        Hide();
    }
    
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        NetStart();
        this.gameObject.SetActive(true);
    }

    public void NetStart()
    {
        UI_CharPanel.Instance.StopCam();
        AgoraManager.Instance.Join();
        Server.Instance.ChatStart();
        //ScaleFromMicrophone.Instance.startSaying = true;
    }

    public void Leave()
    {
        AgoraManager.Instance.Leave();
        PhotonNetwork.LeaveRoom();
        Hide();
        UI_CharPanel.Instance.Show();
        Server.Instance.ChatEnd();
        //ScaleFromMicrophone.Instance.startSaying = false;

    }

    public void AddPlayerSlot(string playerName)
    {
        Debug.Log("AddPlayerSlot");
        
        GameObject newPlayer=Instantiate<GameObject>(m_PlayerSlotPrefab);
        newPlayer.transform.SetParent(PlayerSlotParent.transform);
        newPlayer.transform.localScale=new Vector3(1,1,1);
        
        var texts = newPlayer.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var txtComponent in texts)
        {
            if (txtComponent.name == "Text_Info")
            {
                txtComponent.text = playerName;
            }
        }
    }
    
    public void DelPlayerSlot(string playerName)
    {
        for (int i = 0; i < PlayerSlotParent.transform.childCount; ++i)
        {
            var texts = PlayerSlotParent.transform.GetChild(i).GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var txtComponent in texts)
            {
                if (txtComponent.name == "Text_Info")
                {
                    if (txtComponent.text == playerName)
                    {
                        Destroy(PlayerSlotParent.transform.GetChild(i).gameObject);
                    }
                }
            }
        }

    }
    
    
    private void scrollUpdate()
    {
        AIParent.SetActive(false);
        AIParent.SetActive(true);
        AIParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, AIParent.GetComponent<RectTransform>().anchoredPosition.y + 100);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)AIParent.transform);

    }
    
    public void AddAIText(string msg)
    {
        string[] words = msg.Split(':');
        GameObject newText=Instantiate<GameObject>(m_ChatTextPrefab);
        newText.transform.SetParent(AIParent.transform);
        newText.transform.localScale=new Vector3(1,1,1);
        
        var texts = newText.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var txtComponent in texts)
        {
            if (txtComponent.name == "NameText")
            {
                txtComponent.text = words[0];
            }
            else if (txtComponent.name == "MessageText")
            {
                txtComponent.text = words[1];
            }
        }
        
        
        scrollUpdate();


    }

    public void AddAIImage(List<KeywordDict> data)
    {
        GameObject newObject =Instantiate<GameObject>(m_AIImagePrefab);
            
        newObject.transform.SetParent(AIParent.transform);
        newObject.transform.localScale=new Vector3(1,1,1);

        scrollUpdate();

        var rawImages = newObject.GetComponentsInChildren<RawImage>();

        int count = 0;
        bool isBreak = false;
        for (int i = 0; i < data.Count; i++)
        {
            foreach (var c in data[i].Elements)
            {
                int a = count;
                StartCoroutine(ImageManager.Instance.GetTexture(rawImages[a], c));
                count += 1;

                if (count >= 5)
                {
                    isBreak = true;
                    break;
                }
            }

            if (isBreak)
            {
                break;
            }
        }

    }

    public void AddAIGraph(string url)
    {
        GameObject newObject =Instantiate<GameObject>(m_AIGraphPrefab);
            
        newObject.transform.SetParent(AIParent.transform);
        newObject.transform.localScale=new Vector3(1,1,1);
        
        scrollUpdate();

        var rawImage = newObject.GetComponentInChildren<RawImage>();
        
        StartCoroutine(ImageManager.Instance.GetTexture(rawImage, url));
        // http://192.168.0.103:5100/static/network/network_XucQRD7ywOalsJKCAAAB_1.png
    }

    public void AddAISummary(string summaryText)
    {
        GameObject newObject =Instantiate<GameObject>(m_AITextPrefab);
        newObject.transform.SetParent(AIParent.transform);
        newObject.transform.localScale=new Vector3(1,1,1);
        
        var texts = newObject.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var txtComponent in texts)
        {
            if (txtComponent.name == "MessageText")
            {
                txtComponent.text = summaryText;
            }
        }

        
        //newObject.transform.Find("MessageText").GetComponent<TextMeshProUGUI>().text = summaryText;
        scrollUpdate();

    }
    public void AddAIKeyword(List<ChatKeywordData2> data)
    {
        GameObject newObject =Instantiate<GameObject>(m_AITextPrefab);
        newObject.transform.SetParent(AIParent.transform);
        newObject.transform.localScale=new Vector3(1,1,1);
        
        var texts = newObject.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var txtComponent in texts)
        {
            if (txtComponent.name == "MessageText")
            {
                string text = String.Empty;

                // todo:
                // 멋지게 텍스트 꾸미기
                int count = data.Count;
                for (int i = 0; i < count; ++i)
                {
                    text += $"<b>[<color=blue>{data[i].Keyword}</color>]</b>";
                    text += "\n(";
                    foreach (var d in data[i].Elements)
                    {
                        text += $"{d.Key} ";
                    }
                    text += ")";
                    
                    text += "\n\n";
                }

                txtComponent.text = text;
            }
        }

        scrollUpdate();

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

