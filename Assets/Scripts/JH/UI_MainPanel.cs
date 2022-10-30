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
    public GameObject m_chatPrefab;
    public GameObject m_AIChatWithImagePrefab;
    public GameObject m_AIChatWithGraphPrefab;
    public GameObject m_AIChatWithKeywordPrefab;

    //public Text m_Text;
    public static UI_MainPanel Instance;
    
    public GameObject chatParent;
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

    public void InstantiateKeywordText(string msg)
    {
        /*
        GameObject keywordText = Instantiate<GameObject>(keyWord);
        keywordText.transform.SetParent(keywordPanel.transform);
        keywordText.transform.localScale = new Vector3(1, 1, 1);
        //keywordText.transform.position = new Vector3(0, 0, 0);
        keywordText.GetComponent<Text>().text = msg;
        */
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
        ScaleFromMicrophone.Instance.startSaying = true;
    }

    public void Leave()
    {
        AgoraManager.Instance.Leave();
        PhotonNetwork.LeaveRoom();
        UI_MainPanel.Instance.Hide();
        UI_CharPanel.Instance.Show();
        Server.Instance.ChatEnd();
        ScaleFromMicrophone.Instance.startSaying = false;

    }
    
    public void ChatRPC(string msg)
    {
        string[] words = msg.Split(':');
        GameObject newText=Instantiate<GameObject>(m_chatPrefab);
        newText.transform.SetParent(chatParent.transform);
        newText.transform.localScale=new Vector3(1,1,1);
        newText.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = words[0];
        newText.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = words[1];
        
        chatParent.gameObject.SetActive(false);
        chatParent.gameObject.SetActive(true);
        
        chatParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, chatParent.GetComponent<RectTransform>().anchoredPosition.y + 100);

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)chatParent.transform);
        

    }

    public void AddAIChatWithImage(List<KeywordDict> data)
    {
        GameObject newObject =Instantiate<GameObject>(m_AIChatWithImagePrefab);
            
        newObject.transform.SetParent(chatParent.transform);
        newObject.transform.localScale=new Vector3(1,1,1);
        
        chatParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, chatParent.GetComponent<RectTransform>().anchoredPosition.y + 100);

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)chatParent.transform);
        chatParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, chatParent.GetComponent<RectTransform>().anchoredPosition.y + 100);

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

    public void AddAIChatWithGraph(string url)
    {
        GameObject newObject =Instantiate<GameObject>(m_AIChatWithGraphPrefab);
            
        newObject.transform.SetParent(chatParent.transform);
        newObject.transform.localScale=new Vector3(1,1,1);
        
        chatParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, chatParent.GetComponent<RectTransform>().anchoredPosition.y + 100);

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)chatParent.transform);

        var rawImage = newObject.GetComponentInChildren<RawImage>();
        
        StartCoroutine(ImageManager.Instance.GetTexture(rawImage, url));
        // http://192.168.0.103:5100/static/network/network_XucQRD7ywOalsJKCAAAB_1.png
    }

    public void AddAIChatWithSummary(string summaryText)
    {
        GameObject newObject =Instantiate<GameObject>(m_AIChatWithKeywordPrefab);
        newObject.transform.SetParent(chatParent.transform);
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


        chatParent.gameObject.SetActive(false);
        chatParent.gameObject.SetActive(true);

        chatParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, chatParent.GetComponent<RectTransform>().anchoredPosition.y + 100);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)chatParent.transform);
        
        chatParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, chatParent.GetComponent<RectTransform>().anchoredPosition.y + 100);

    }
    public void AddAIChatWithKeyword(List<ChatKeywordData2> data)
    {
        GameObject newObject =Instantiate<GameObject>(m_AIChatWithKeywordPrefab);
        newObject.transform.SetParent(chatParent.transform);
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

        

        chatParent.gameObject.SetActive(false);
        chatParent.gameObject.SetActive(true);
        
        chatParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, chatParent.GetComponent<RectTransform>().anchoredPosition.y + 100);

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)chatParent.transform);
        
        chatParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, chatParent.GetComponent<RectTransform>().anchoredPosition.y + 100);

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

public void AddFriendList()
{

}

}

