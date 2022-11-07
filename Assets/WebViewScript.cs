using UnityEngine;

using System.Collections;


public class WebViewScript : MonoBehaviour
{

    public WebViewObject webViewObject;

    // Use this for initialization

    void Start()
    {
        StartWebView();
    }

    // Update is called once per frame

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
        string strUrl = "http://www.naver.com";

        


        webViewObject =
        (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
        webViewObject.Init((msg) => {
            Debug.Log(string.Format("CallFromJS[{0}]", msg));
        });

        webViewObject.LoadURL(strUrl);
        webViewObject.SetVisibility(true);
        webViewObject.SetCameraAccess(true);
        webViewObject.SetMargins(50, 50, 50, 50);

    }
}
