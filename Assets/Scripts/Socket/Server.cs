using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Net.WebSockets;
using ChatNetWork;
using System.Runtime.InteropServices;
using SocketIOClient;
using System.IO;
using System.Text.Json.Serialization;
//using System.Text.Json;
using Newtonsoft.Json;

public class Server : MonoBehaviour
{
    public static Server Instance;
    
    private SocketIOUnity m_Socket;
    private bool m_Connected = false;
    public AudioSource mic;
    public AudioClip micClip;

    public string receive_Name;
    public string receieveSTT_Word;

    public string proto1;
    public string proto2;
    //json
    private List<Action> m_Actions = new List<Action>();

    private List<Action> m_keyActions = new List<Action>();
    
    private void Awake()
    {
        Instance = this;
        
        DontDestroyOnLoad (this);

    }

    public void ChatStart()
    {
        Debug.LogError(222);

        mic = transform.GetComponent<AudioSource>();
        //mic.clip = Microphone.Start(Microphone.devices[0].ToString(), true, 5, AudioSettings.outputSampleRate);
        var uri = new Uri("http://192.168.0.37:5100");
        m_Socket = new SocketIOUnity(uri, new SocketIOOptions()
        {
            Query = new Dictionary<string, string> { { "token", "UNITY" } },
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });
        m_Socket.Connect();

        m_Socket.On("connection", (response) =>
        {
            m_Socket.Emit("userInfo", UI_StartPanel.Instance.userName);

            m_Connected = true;

            m_Socket.On("message", (response) =>
            {
                Debug.Log(response.GetValue<string>());

            });


            m_Socket.On("getKeylist", (response) =>
            {
                Debug.Log("receiveKeyword");
                string jsonText = response.GetValue<string>();
                Debug.Log(jsonText);
                ChatKeywordData chatKeyword = JsonConvert.DeserializeObject<ChatKeywordData>(jsonText);
                
                foreach (string i in chatKeyword.subkey1)
                {
                    proto1 += i+", ";
                }
                Debug.Log(proto1);
                foreach (string i in chatKeyword.subkey2)
                {
                    proto2 += i+", ";
                }
                Debug.Log(proto2);

                m_keyActions.Add(() =>
                {
                    UI_MainPanel.Instance.ReceiveJson(chatKeyword.mainkey[0], chatKeyword.mainkey[1], proto1, proto2);
                });

            });


            m_Socket.On("receiveVoice", (response) =>
            {
                test1 = response.GetValue<byte[]>();


            });
            m_Socket.On("receiveSTT", (response) =>
            {
                m_Actions.Add(() =>
                {
                    string text = response.GetValue<string>();
                    var spilttedText = text.Split("/|*^");
                    receive_Name = spilttedText[0];
                    receieveSTT_Word = spilttedText.Length > 1 ? spilttedText[1] : string.Empty;
                    UI_MainPanel.Instance.ChatRPC($"{receive_Name}: {receieveSTT_Word}");

                    print(receive_Name);
                    print(receieveSTT_Word);
                });
                

            });
            
        });
        
        StartCoroutine(Record());
    }

    public void ChatEnd()
    {
        m_Socket.Disconnect();
        m_Connected = false;
    }
    
    public IEnumerator Record()
    {
        yield return null;
        StartCoroutine(ConvertAudio());
        mic.clip = Microphone.Start(Microphone.devices[0].ToString(), true, 5, AudioSettings.outputSampleRate);
        print("Record");

    }


    IEnumerator ConvertAudio()
    {
        yield return new WaitForSeconds(5f);
        //mic.clip = Microphone.Start(Microphone.devices[0].ToString(), true, 100, AudioSettings.outputSampleRate);
        StartCoroutine(Record());
        test = GetClipData(mic.clip);
        print("Send");
        m_Socket.Emit("message", test);
        //m_Socket.Emit("userInfo",UI_StartPanel.Instance.userName);
        
    }


    public bool calling = false;
    public byte[] test;
    //public bool recording = false;
    public byte[] test1;

    public float[] floatTest;
    public byte[] receiveVoice;


    float time;
    private void Update()
    {
        foreach (var a in m_Actions)
        {
            a.Invoke();
        }
        
        m_Actions.Clear();


        foreach (var a in m_keyActions)
        {
            a.Invoke();
        }

        m_keyActions.Clear();

        //time += Time.deltaTime;

        //if (time >= 5)
        //{
        //    time = 0;
        //    //print(Microphone.IsRecording(null));
        //    print(mic.clip.length);
        //    test = GetClipData(mic.clip);
        //    m_Socket.Emit("message", test);
        //}

    }



    // audioclip to byte
    public static byte[] GetClipData(AudioClip _clip)
    {
        //Get data
        float[] floatData = new float[_clip.samples * _clip.channels];
        _clip.GetData(floatData, 0);

        //convert to byte array
        byte[] byteData = new byte[floatData.Length * 4];
        Buffer.BlockCopy(floatData, 0, byteData, 0, byteData.Length);

        return (byteData);
    }


    public static AudioClip ByteToAudio(byte[] vs)
    {
        float[] samples = new float[vs.Length / 4]; //size of a float is 4 bytes

        Buffer.BlockCopy(vs, 0, samples, 0, vs.Length);

        int channels = 1; //Assuming audio is mono because microphone input usually is
        int sampleRate = 44100; //Assuming your samplerate is 44100 or change to 48000 or whatever is appropriate


        print(samples.Length);
        AudioClip clip = AudioClip.Create("ClipName", samples.Length, channels, sampleRate, false);
        clip.SetData(samples, 0);


        return clip;
    }


}

[Serializable]
public class ChatKeywordData
{
    public List<string> mainkey;
    public List<string> subkey1;
    public List<string> subkey2;

}

