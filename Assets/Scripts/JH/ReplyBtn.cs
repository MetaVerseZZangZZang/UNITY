using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReplyBtn : MonoBehaviour
{
    public ChatPlayer m_ChatPlayer;

    public void ReplyBtnClick()
    {
        UI_ReplyInputField.Instance.id = m_ChatPlayer.id;
        UI_ReplyInputField.Instance.Show();
        UI_ChatInputField.Instance.Hide();
    }
}
