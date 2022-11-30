using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIOClient;
using Newtonsoft.Json;

public class FakeChat
{
    public string Nickname;
    public string Message;

    public FakeChat(string nickname, string message)
    {
        Nickname = nickname;
        Message = message;
    }
}

public class Fake : MonoBehaviour
{
    public static Fake Instance;
    
    public string HOST = "http://zzangzzang.site:5100";

    public UI_FakeChat m_FakeChatPrefab;

    private SocketIOUnity m_Socket;
    private bool m_Connected = false;

    public List<FakeChat> FakeChats;
    public List<UI_FakeChat> UIFakeChats;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Debug.LogError("Start");
        
        MakeFakeChatList();
        ConnectSocket();
    }

    private void MakeFakeChatList()
    {
        int count = TBL_CHAT.CountEntities;
        FakeChats = new List<FakeChat>(count);
        UIFakeChats = new List<UI_FakeChat>(count);

        for (int i = 0; i < count; ++i)
        {
            var data = TBL_CHAT.GetEntity(i);
            var chat = new FakeChat(data.Nickname, data.Message);
            FakeChats.Add(chat);

            var fakeChatObject = Instantiate<UI_FakeChat>(m_FakeChatPrefab, transform, true);
            fakeChatObject.Init(chat);
            UIFakeChats.Add(fakeChatObject);
        }
    }

    private void ConnectSocket()
    {
        var uri = new Uri(HOST);
        m_Socket = new SocketIOUnity(uri, new SocketIOOptions()
        {
            Query = new Dictionary<string, string> { { "token", "UNITY" } },
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });
        
        m_Socket.Connect();
        m_Socket.On("connection", (response) =>
        {
            m_Socket.Emit("chatStart","chatStart");

        });
        
        
    }

    public void SendFakeChat(FakeChat fakeChat)
    {
        InputFieldText chat = new InputFieldText();
        chat.nickname = fakeChat.Nickname;
        chat.context = fakeChat.Message;

        string json = JsonUtility.ToJson(chat);
        m_Socket.Emit("getInputFieldFake", json);
    }
}
