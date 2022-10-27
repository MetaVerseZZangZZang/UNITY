using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class UI_JoinRoomItem : MonoBehaviour
{
    private RoomInfo m_Data;

    //public List<RawImage> m_imageList = new List<RawImage>();
    public Text m_roomNameText;
    public Text m_playerCountText;
    public Text m_maxCountText;
    
    public void Init(RoomInfo data)
    {
        m_Data = data;

        Set();
    }

    private void Set()
    {
        if (m_Data == null)
        {
            return;
        }
        m_roomNameText.text = m_Data.Name.ToString();
        m_playerCountText.text = m_Data.PlayerCount.ToString();
        m_maxCountText.text = "/"+m_Data.MaxPlayers.ToString();
        
    }

    public void onClick()
    {
        NetManager.Instance.roomSelect(m_Data);
        UI_JoinRoom.Instance.Hide();
        UI_CharPanel.Instance.Hide();
        UI_MainPanel.Instance.Show();
    }
}
