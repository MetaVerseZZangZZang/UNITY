using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class WebViewScript : MonoBehaviour
{
    public WebViewObject webViewObject;
    public GameObject safariPanel;

    public int currnetURL;
    // Use this for initialization
    public List<string> surfSaveList = new List<string>();
    public int saveURLCount;


    public bool extend = false;


    public GameObject safariIcon;

    public bool shutDown = false;

/*
    void Start()
    {
        
        StartWebView();
    }

    // Update is called once per frame
    string url = "http://www.naver.com";
    void Update()
    {

        if (Input.GetKey(KeyCode.Escape))
        {
            Destroy(webViewObject);
            return;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Destroy(webViewObject);
                return;
            }

        }
    }


    public void XBtn()
    {
        Destroy(webViewObject);
        Destroy(webViewObject.gameObject);
        safariPanel.SetActive(false);
    }

    public void _BTN()
    {
        shutDown = true;
        surfSaveList = WebViewObject.Instance.surfingList;
        saveURLCount = WebViewObject.Instance.currentURL;
        webViewObject.gameObject.SetActive(false);
        safariIcon.SetActive(true);
        safariPanel.SetActive(false);
    }

    public void safariBTN()
    {
        if (shutDown == true)
        {
            shutDown = false;
            safariIcon.SetActive(false);
            webViewObject.gameObject.SetActive(true);
            safariPanel.SetActive(true);
        }
    }

    public void ExtendSize()
    {
        if (extend == false)
        {
            extend = true;
            surfSaveList = WebViewObject.Instance.surfingList;
            saveURLCount = WebViewObject.Instance.currentURL;
            Destroy(webViewObject);
            Destroy(webViewObject.gameObject);

            
            webViewObject =
            (new GameObject("WebViewObject")).AddComponent<WebViewObject>();


            webViewObject.Init(1920, 1000);
            

            if (saveURLCount - 1 >= 0)
            {
                webViewObject.LoadURL(surfSaveList[saveURLCount - 1]);
            }
            else
            {
                webViewObject.LoadURL(surfSaveList[saveURLCount - 1]);
            }
            RectTransform safariPanelRect = safariPanel.GetComponent<RectTransform>();
            webViewObject.SetVisibility(true);
            webViewObject.rect = new Rect(0, 0, 1920, 1000);
            safariPanelRect.sizeDelta = new Vector2(1920,1080);


            //sharescreen area
            ScreenShareWhileVideoCall.Instance._rect = new Rect(0, 0, 1920, 1000);

        }
        else
        {
            extend = false;
            surfSaveList = WebViewObject.Instance.surfingList;
            saveURLCount = WebViewObject.Instance.currentURL;
            Destroy(webViewObject);
            Destroy(webViewObject.gameObject);


            webViewObject =
            (new GameObject("WebViewObject")).AddComponent<WebViewObject>();

            webViewObject.Init(1148, 720);
            webViewObject.LoadURL(surfSaveList[saveURLCount - 1]);
            webViewObject.SetVisibility(true);
            webViewObject.rect = new Rect(0, 0, 1148, 700);
            RectTransform safariPanelRect = safariPanel.GetComponent<RectTransform>();
            safariPanelRect.sizeDelta = new Vector2(1148, 780);
            ScreenShareWhileVideoCall.Instance._rect = new Rect(0, 0, 1148, 780);
        }
        
    }


    public void StartWebView()
    {
        string strUrl = "http://www.youtube.com";

        webViewObject =
        (new GameObject("WebViewObject")).AddComponent<WebViewObject>();

        webViewObject.Init(1148,720);
        //webViewObject.Init((msg) =>
        //{
        //    Debug.Log(string.Format("CallFromJS[{0}]", msg));
        //});

        webViewObject.LoadURL(strUrl);
        webViewObject.SetVisibility(true);
        //webViewObject.SetMargins(3, 3, 3, 3);

    }

*/
}
