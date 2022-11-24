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
    public Toggle camToggle;
    public Toggle voiceToggle;
    void Start()
    {
    }

    public void Init(string playerName)
    {
        transform.localScale=new Vector3(1,1,1);
        name.text = playerName;

        if (playerName == PhotonNetwork.LocalPlayer.NickName)
        {
            camToggle.gameObject.SetActive(false);
            voiceToggle.gameObject.SetActive(false);
        }
    }

    public void Destory()
    {
        Destroy(this);        
    }
}
