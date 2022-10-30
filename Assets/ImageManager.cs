using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    public static ImageManager Instance;


    public GameObject summaryGameObject;
    public GameObject networkGraphObject;

    private void Awake()
    {
        Instance = this;
    }


    public RawImage downloadImage;
    public List<RawImage> downloadImageList = new List<RawImage>();


    public void SummaryResult(string summary)
    {
        summaryGameObject.SetActive(true);
        Text summaryText = summaryGameObject.GetComponent<Text>();
        summaryText.text = summary;
    }


    public IEnumerator GetTexture(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("imageDownload");
            downloadImage.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            downloadImageList.Add(downloadImage);
        }
    }


    public IEnumerator GetNetworkGraph(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            networkGraphObject.SetActive(true);
            RawImage networkGraph = networkGraphObject.GetComponent<RawImage>();
            Debug.Log("1111111123421431+imageDownload");

            networkGraph.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

        }
    }


}
