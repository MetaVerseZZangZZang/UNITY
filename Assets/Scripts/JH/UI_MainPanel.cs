using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;


public class UI_MainPanel : MonoBehaviour
{

    public static UI_MainPanel Instance;

    private void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Leave()
    {
        AgoraManager.Instance.Leave();
        PhotonNetwork.LeaveRoom();
        Hide();
    }
    
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        UI_CharPanel.Instance.StopCam();
        this.gameObject.SetActive(true);
        
    }
}
