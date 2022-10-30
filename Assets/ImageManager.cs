using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    public static ImageManager Instance;

    private void Awake()
    {
        Instance = this;
    }


    public RawImage downloadImage;
    public List<RawImage> downloadImageList = new List<RawImage>();



    public void DownloadImage()
    {
        //StartCoroutine(GetTexture(downloadImage));
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
}
