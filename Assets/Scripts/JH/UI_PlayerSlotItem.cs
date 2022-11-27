using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerSlotItem : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI name;
    public Image cam;
    public Image voice;
    void Start()
    {
    }

    public void Init(string playerName)
    {
        transform.localScale=new Vector3(1,1,1);
        name.text = playerName;

        if (playerName == PhotonNetwork.LocalPlayer.NickName)
        {
            cam.gameObject.SetActive(false);
            voice.gameObject.SetActive(false);
        }
    }

    public void Destory()
    {
        Destroy(this);
        UI_PlayerSlot.Instance.PlayerSlotList.Remove(this);
    }

    public void camControl(bool camFlag)
    {
        if (!camFlag)  //꺼라
        {
            cam.transform.GetChild(3).gameObject.SetActive(false);  //꺼라
        }
        else  //켜라
        {
            cam.transform.GetChild(3).gameObject.SetActive(true);  //켜라
        }
    }
    
    public void voiceControl(bool voiceFlag)
    {
        if (!voiceFlag)  //꺼라
        {
            voice.transform.GetChild(3).gameObject.SetActive(false);
        }
        else  //켜라
        {
            voice.transform.GetChild(3).gameObject.SetActive(true);
        }
    }
    
    
}
