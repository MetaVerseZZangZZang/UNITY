using UnityEngine;

using System.Collections;
using UnityEngine.UI;


public class WebViewScript : MonoBehaviour
{

    public WebViewObject webViewObject;


    public InputField userInputURL;
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

        if (Input.GetKeyDown(KeyCode.N))
        {
            webViewObject.LoadURL(url);
        }



    }


    public void SurfURL()
    {

        webViewObject.LoadURL(userInputURL.text);
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
// webViewObject.SetMargins(50, 50, 50, 50);

    }
}
