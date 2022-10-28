using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;


public class UI_MainPanel : MonoBehaviour
{
    public GameObject m_Prefab;

    //public Text m_Text;
    public static UI_MainPanel Instance;

    public GameObject chatTextParent;
    //public Text newText;
    private void Awake()
    {
        Instance = this;
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
        AgoraManager.Instance.Join();
        Server.Instance.ChatStart();
        Debug.LogError(111);
    }

    public void Leave()
    {
        AgoraManager.Instance.Leave();
        PhotonNetwork.LeaveRoom();
        UI_MainPanel.Instance.Hide();
        UI_CharPanel.Instance.Show();
        Server.Instance.ChatEnd();
        
    }
    
    public void ChatRPC(string msg)
    {
        GameObject newText=Instantiate<GameObject>(m_Prefab);
        newText.transform.SetParent(chatTextParent.transform);
        newText.transform.localScale=new Vector3(1,1,1);
        newText.GetComponent<Text>().text = msg;
    }
}
