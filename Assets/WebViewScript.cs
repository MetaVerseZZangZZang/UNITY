using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class WebViewScript : MonoBehaviour
{
    public WebViewObject webViewObject;
    public int currnetURL;
    // Use this for initialization

    void Start()
    {
        StartWebView();
    }

    // Update is called once per frame
    string url = "http://www.naver.com";
    void Update()
    {

        if (Application.platform == RuntimePlatform.Android)
        {

            if (Input.GetKey(KeyCode.Escape))
            {
                Destroy(webViewObject);
                return;
            }
        }
    }




    public void StartWebView()
    {
        string strUrl = "http://www.youtube.com";

        webViewObject =
        (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
        webViewObject.Init((msg) => {
            Debug.Log(string.Format("CallFromJS[{0}]", msg));
        });

        webViewObject.LoadURL(strUrl);
        webViewObject.SetVisibility(true);
        //webViewObject.SetMargins(3, 3, 3, 3);

    }
}
