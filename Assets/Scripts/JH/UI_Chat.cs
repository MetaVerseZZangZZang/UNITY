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
    
    public GameObject AIParent;
    public GameObject HeartParent;
    
    public static UI_Chat Instance;
    public Sprite Heart_filled;
    public Sprite Heart_unfilled;
    
    private void Awake()
    {
        Instance = this;
    }
    
    private void scrollUpdate()
    {
        AIParent.SetActive(false);
        AIParent.SetActive(true);
        AIParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, AIParent.GetComponent<RectTransform>().anchoredPosition.y + 100);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)AIParent.transform);

    }
    
    public void AddChatText(string msg)
    {
        string id;
        string[] words = msg.Split(':');
        GameObject newText=Instantiate<GameObject>(m_ChatTextPrefab);
        newText.transform.SetParent(AIParent.transform);
        newText.transform.localScale=new Vector3(1,1,1);
        
        id = words[0];
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
        }
        
        Debug.Log(id);
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
    
    public void HeartClick(int num)
    {
        for (int i = 0; i < num; ++i)
        {
            HeartParent.transform.GetChild(i).GetComponent<Image>().sprite = Heart_filled;
        }

        for (int i = num; i <= HeartParent.transform.childCount; ++i)
        {
            HeartParent.transform.GetChild(i).GetComponent<Image>().sprite = Heart_unfilled;
        }

        //Server.Instance.HeartEmit(num,);

    }
}
