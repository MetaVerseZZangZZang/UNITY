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

    public IEnumerator GetTexture(RawImage img, string url)
    {
        Debug.LogError($"요청: {url}");
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("실패");
            Debug.LogError(url);
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("imageDownload");
            img.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }
}
