using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FakeChat : MonoBehaviour
{
    private FakeChat m_FakeChat;
    public Text m_MessageText;

    public void Init(FakeChat fakeChat)
    {
        m_FakeChat = fakeChat;

        m_MessageText.text = $"{m_FakeChat.Nickname} : {m_FakeChat.Message}";
    }

    public void OnClickButton()
    {
        Fake.Instance.SendFakeChat(m_FakeChat);
    }
}

