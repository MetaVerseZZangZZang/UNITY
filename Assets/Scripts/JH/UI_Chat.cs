using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Chat : MonoBehaviour
{   
    
    public GameObject m_ChatTextPrefab;  //stt 결과
    public GameObject m_AIImagePrefab;
    public GameObject m_AIGraphPrefab;
    public GameObject m_AITextPrefab;
    public GameObject m_ReplyTextPrefab;
    
    public GameObject AIParent;
    public static UI_Chat Instance;
    public GameObject fileImage;

    private void Awake()
    {
        Instance = this;
        
        AIParent.SetActive(false);
        AIParent.SetActive(true);

    }

    

    private void scrollUpdate()
    {
        AIParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, AIParent.GetComponent<RectTransform>().anchoredPosition.y + 100);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)AIParent.transform);
        AIParent.SetActive(false);
        AIParent.SetActive(true);
    }
    
    public void AddChatText(string msg)
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
                txtComponent.text = words[1];
            }
            else if (txtComponent.name == "MessageText")
            {
                txtComponent.text = words[2];
            }
            else if (txtComponent.name == "FilteringText")
            {
                txtComponent.text = words[3];
            }
        }

        /*
        newText.GetComponent<ChatPlayer>().id= words[0];
        newText.GetComponent<ChatPlayer>().name= words[1];
        newText.GetComponent<ChatPlayer>().message= words[2];
        */

        ChatPlayer m_ChatPlayer = newText.GetComponent<ChatPlayer>();
        m_ChatPlayer.id = words[0];
        m_ChatPlayer.name = words[1];
        m_ChatPlayer.message = words[2];
        m_ChatPlayer.filtering = words[3];
        ChatPlayerManager.Instance.ChatPlayersList.Add(m_ChatPlayer);
        
        scrollUpdate();
    }

    public void AddReplyText(string msg)
    {
        string[] words = msg.Split(':');
        GameObject newReply=Instantiate<GameObject>(m_ReplyTextPrefab);
        ChatPlayer cp = ChatPlayerManager.Instance.findChatPlayerById(words[0]);
        
        newReply.transform.SetParent(AIParent.transform);
        newReply.transform.localScale=new Vector3(1,1,1);
        
        var texts = newReply.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var txtComponent in texts)
        {
            if (txtComponent.name == "AnswerTo")
            {
                txtComponent.text = cp.name + "에게 답장";
            }
            else if (txtComponent.name == "MessageText")
            {
                if (cp.message.Length >= 55)
                {
                    txtComponent.text = cp.message.Substring(0, 55) + " ...";
                }
                else
                {
                    txtComponent.text = cp.message;
                }
            }
            else if (txtComponent.name == "NameText")
            {
                txtComponent.text = words[1];
            }
            
            else if (txtComponent.name == "ReplyText")
            {
                txtComponent.text = words[2];
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

    public void AddAIVisionImage(List<string> data)
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
            foreach (string c in data)
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
    
    
    public void AddFile(string Url,string fileName ,string extension)
    {
        //Debug.Log("ADDFILE");
        GameObject newObject = Instantiate<GameObject>(fileImage);
        newObject.transform.SetParent(AIParent.transform);
        newObject.transform.localScale = new Vector3(1, 1, 1);
        Button button = newObject.GetComponent<Button>();
        button.onClick.AddListener(()=>StartCoroutine(FileUpload.Instance.URLFileSave(Url,fileName,extension)));
        
        //Text fileExtention = newObject.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        //fileExtention.text = extension.ToUpper();
        
        scrollUpdate();
        
    }

    
    public void AddAIGraph(string url)
    {
        GameObject newObject =Instantiate<GameObject>(m_AIGraphPrefab);
            
        newObject.transform.SetParent(AIParent.transform);
        newObject.transform.localScale=new Vector3(1,1,1);
        
        scrollUpdate();

        var rawImage = newObject.GetComponentInChildren<RawImage>();
        
        StartCoroutine(ImageManager.Instance.GetTexture(rawImage, url));
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
                Debug.Log("summaryText "+summaryText);
                Debug.Log("txtComponent.text "+txtComponent.text);
            }
        }
        
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


}
