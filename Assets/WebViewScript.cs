using UnityEngine;

using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class WebViewScript : MonoBehaviour
{

    public WebViewObject webViewObject;


    public InputField userInputURL;


    public List<string> surfingList = new List<string>();

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


    public void SurfURL()
    {
        if (userInputURL.text.Contains("http://") == true && userInputURL.text.Contains("www.") == true)
        {
            if (userInputURL.text.Contains("www.") == true)
            {
                webViewObject.LoadURL(userInputURL.text);
                surfingList.Add(userInputURL.text);

                currnetURL = surfingList.Count;

                userInputURL.text = userInputURL.text;
            }
        }

        if (userInputURL.text.Contains("www.") == true)
        {
            webViewObject.LoadURL("http://" + userInputURL.text);
            surfingList.Add("http://" + userInputURL.text);

            currnetURL = surfingList.Count;

            userInputURL.text = "http://" + userInputURL.text;
        }

        else if (userInputURL.text.Contains("www.") == false)
        {
            webViewObject.LoadURL("http://www." + userInputURL.text);
            surfingList.Add("http://www." + userInputURL.text);

            currnetURL = surfingList.Count;

            userInputURL.text = "http://www." + userInputURL.text;
        }
    }

    public void BackURL()
    {
        currnetURL = currnetURL - 1;
        string backURL = surfingList[currnetURL-1];
        webViewObject.LoadURL(backURL);
        userInputURL.text = backURL;
    }

    public void NextURL()
    {
        currnetURL = currnetURL + 1;
        string frontURL = surfingList[currnetURL-1];
        webViewObject.LoadURL(frontURL);
        userInputURL.text = frontURL;

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
