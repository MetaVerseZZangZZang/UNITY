using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReplyBtn : MonoBehaviour
{
    public ChatPlayer m_ChatPlayer;
  
    
    public void ReplyBtnClick()
    {
        UI_ChatInputField.Instance.id = m_ChatPlayer.id;
    }
}
