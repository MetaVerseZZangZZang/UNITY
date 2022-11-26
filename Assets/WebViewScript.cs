using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class WebViewScript : MonoBehaviour
{

    public static WebViewScript Instance;
    public WebViewObject webViewObject;
    public GameObject safariPanel;

    public int currnetURL;
    // Use this for initialization
    public List<string> surfSaveList = new List<string>();
    public int saveURLCount;


    public bool extend = false;


    public GameObject safariIcon;

    public bool shutDown = false;


    private void Awake()
    {
        Instance = this;
    }

    //void Start()
    //{

    //    StartWebView();
    //}

    public void ChatWebview(string url)
    {
        webViewObject =
        (new GameObject("WebViewObject")).AddComponent<WebViewObject>();

        webViewObject.Init(1148, 720);
        //webViewObject.Init((msg) =>
        //{
        //    Debug.Log(string.Format("CallFromJS[{0}]", msg));
        //});

        webViewObject.LoadURL(url);
        webViewObject.SetVisibility(true);
        //webViewObject.SetMargins(3, 3, 3, 3);
    }

    public void OnSafariGetURL(string url)
    {
        webViewObject.LoadURL(url);
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
        shutDown = true;
        surfSaveList = WebViewObject.Instance.surfingList;
        saveURLCount = WebViewObject.Instance.currentURL;
        webViewObject.gameObject.SetActive(false);
        safariIcon.SetActive(true);
        safariPanel.SetActive(false);


        
        GameObject player = GameObject.Find(ScreenShareWhileVideoCall.Instance.playerdict[(int)ScreenShareWhileVideoCall.Instance.Uid2] + "(user)");
        PlayerItem playerScript = player.GetComponent<PlayerItem>();
        playerScript.webviewStart = false;
        player.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        Debug.LogError(player);
        
    }

    public void _BTN()
    {

        shutDown = true;
        surfSaveList = WebViewObject.Instance.surfingList;
        saveURLCount = WebViewObject.Instance.currentURL;
        webViewObject.gameObject.SetActive(false);
        safariIcon.SetActive(true);
        safariPanel.SetActive(false);


        
        GameObject player = GameObject.Find(ScreenShareWhileVideoCall.Instance.playerdict[(int)ScreenShareWhileVideoCall.Instance.Uid2] + "(user)");
        PlayerItem playerScript = player.GetComponent<PlayerItem>();
        playerScript.webviewStart = false;
        player.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        Debug.LogError(player);

        
    }

    public void safariBTN()
    {
        if (shutDown == true)
        {
            shutDown = false;
            safariIcon.SetActive(false);
            webViewObject.gameObject.SetActive(true);
            safariPanel.SetActive(true);

            
            GameObject player = GameObject.Find(ScreenShareWhileVideoCall.Instance.playerdict[(int)ScreenShareWhileVideoCall.Instance.Uid2] + "(user)");
            PlayerItem playerScript = player.GetComponent<PlayerItem>();
            playerScript.webviewStart = true;
            player.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            Debug.LogError(player);

            
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
            safariPanelRect.sizeDelta = new Vector2(1920, 1080);


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
            ScreenShareWhileVideoCall.Instance._rect = new Rect(0, 0, 1148, 700);
        }

    }


    public void StartWebView()
    {
        string strUrl = "http://www.youtube.com";

        webViewObject =
        (new GameObject("WebViewObject")).AddComponent<WebViewObject>();

        webViewObject.Init(1148, 720);
        //webViewObject.Init((msg) =>
        //{
        //    Debug.Log(string.Format("CallFromJS[{0}]", msg));
        //});

        webViewObject.LoadURL(strUrl);
        webViewObject.SetVisibility(true);
        //webViewObject.SetMargins(3, 3, 3, 3);

    }
}
