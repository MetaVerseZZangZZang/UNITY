using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using System.IO;
using System.Text.Json.Serialization;
using BansheeGz.BGDatabase;
//using System.Text.Json;
using Newtonsoft.Json;
using Unity.RenderStreaming;

public class Server : MonoBehaviour
{
    public static Server Instance;
    public string HOST = "http://zzangzzang.site:5100";
    
    private SocketIOUnity m_Socket;
    private bool m_Connected = false;
    public AudioSource mic;
    public AudioClip micClip;
    
    public string proto1;
    public string proto2;
    //json
    private List<Action> m_Actions = new List<Action>();
    private List<Action> m_keyActions = new List<Action>();
    private List<Action> m_ImageDownloadActions = new List<Action>();
    private List<Action> m_SummaryDownload = new List<Action>();
    private List<Action> m_NetworkGraphDownload = new List<Action>();
    private List<Action> m_FileUpload = new List<Action>();
    private List<Action> m_ReplyActions = new List<Action>();
    private List<Action> m_OCRActions = new List<Action>();
    private List<Action> m_VisionActions = new List<Action>();
    private List<ChatPlayer> ChatPlayerList = new List<ChatPlayer>();

    public string sid;
    public bool AIFlag = false;
    
    private void Awake()
    {
        Instance = this;
        
        DontDestroyOnLoad (this);
    }

    public void ChatStart()
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

            string getID = response.GetValue<string>();
            var getJsonToDictionnary = JsonConvert.DeserializeObject<Dictionary<string, string>>(getID);

            sid = getJsonToDictionnary["sid"];

            Debug.LogWarning("Enter Socket Server");

            m_Socket.Emit("userInfo", UI_StartPanel.Instance.userName);

            m_Connected = true;
            
            m_Socket.On("receiveSTT", (response) =>
            {
                m_Actions.Add(() =>
                {
                    string text = response.GetValue<string>();
                    var spilttedText = text.Split("/|*^");

                    if (text.StartsWith("chattingDB"))
                    {
                        if (spilttedText.Length >= 4)
                        {
                            string receive_id = spilttedText[0];
                            string receive_Name = spilttedText[1];
                            string receieveSTT_Word = spilttedText.Length > 2 ? spilttedText[2] : string.Empty;
                            string receieve_filtering = spilttedText[3];
                            UI_Chat.Instance.AddChatText(
                                $"{receive_id}:{receive_Name}:{receieveSTT_Word}:{receieve_filtering}");
                        }
                    }

                    else
                    {
                        if (spilttedText.Length >= 3)
                        {
                            string receive_id = spilttedText[0];
                            string receive_Name = spilttedText[1];
                            string receieveSTT_Word = spilttedText.Length > 2 ? spilttedText[2] : string.Empty;
                            UI_Chat.Instance.AddChatText(
                                $"{receive_id}:{receive_Name}:{receieveSTT_Word}:{string.Empty}");
                        }
                    }
                        
                });  
                
            });
            m_Socket.On("receiveFile", (response) =>
            {
                string test1 = response.GetValue<string>();
                //UI_Chat.Instance.AddFile(data);

                var list = JsonConvert.DeserializeObject<Dictionary<string, string>>(test1); 
                //var data = JsonConvert.DeserializeObject(test1);
                m_keyActions.Add(() =>
                {
                    UI_Chat.Instance.AddFile(list["url"], list["fileName"], list["extension"]);
                });
                
            });
            
            
            m_Socket.On("receiveReply", (response) =>
            {
                m_ReplyActions.Add(() =>
                {
                    string text = response.GetValue<string>();
                    var spilttedText = text.Split("/|*^"); 
                    if (spilttedText.Length >=3)
                    {
                        string receive_id=spilttedText[0];
                        string receive_Name = spilttedText[1];
                        string receieve_Word = spilttedText.Length > 2 ? spilttedText[2] : string.Empty;
                        Debug.Log($"{receive_id}:{receive_Name}: {receieve_Word}");
                        UI_Chat.Instance.AddReplyText($"{receive_id}:{receive_Name}: {receieve_Word}");

                    }
                });  

            });
        
            /*
            m_Socket.On("message", (response) =>
            {
                Debug.Log(response.GetValue<string>());
            });
            */

            if (AIFlag)
            {
                m_Socket.On("getImglist", (response) =>
                {
                    Debug.Log(response.GetValue());
                    string jsonText = response.GetValue<string>();
                    List<KeywordDict> data = JsonConvert.DeserializeObject<List<KeywordDict>>(jsonText);
                    m_Actions.Add(() => { UI_Chat.Instance.AddAIImage(data); });
                });


                m_Socket.On("getGraph", (response) =>
                {
                    m_NetworkGraphDownload.Add(() =>
                    {
                        string url = HOST + response.GetValue<string>();
                        UI_Chat.Instance.AddAIGraph(url);
                    });
                });

                m_Socket.On("getSummary",
                    (response) =>
                    {
                        m_SummaryDownload.Add(() => { UI_Chat.Instance.AddAISummary(response.GetValue<string>()); });
                    });

                m_Socket.On("getKeylist", (response) =>
                {
                    string jsonText = response.GetValue<string>();

                    List<ChatKeywordData2> data = JsonConvert.DeserializeObject<List<ChatKeywordData2>>(jsonText);

                    m_keyActions.Add(() => { UI_Chat.Instance.AddAIKeyword(data); });

                });

                /*
                m_Socket.On("receiveVoice", (response) =>
                {
                    test1 = response.GetValue<byte[]>();
                });
                */

                m_Socket.On("getOcr", (response) =>
                {
                    m_OCRActions.Add(() =>
                    {
                        string ocr = response.GetValue<string>();
                        Debug.Log("getOcr " + ocr);
                        UI_Chat.Instance.AddAISummary(ocr);
                    });
                });

                m_Socket.On("getVisionText", (response) =>
                {
                    m_VisionActions.Add(() =>
                    {
                        string getImgResult = response.GetValue<string>();
                        var list = JsonConvert.DeserializeObject<fileResult>(getImgResult);
                        UI_Chat.Instance.AddAIVisionImage(list.img);
                        string text = "";
                        for (int i = 0; i < 5; i++)
                        {
                            text += list.label[i] + " ";
                        }

                        UI_Chat.Instance.AddAISummary(text);

                    });
                });
            }
        });
    }


    public void VoiceEmit(byte[] a)
    {
        m_Socket.Emit("message",a);
    }

    public void HeartEmit(string text)
    {
        m_Socket.Emit("heart",text);
    }
    
    public void InputFieldEmit(string text)
    {
        m_Socket.Emit("getInputField",text);
    }

    public void ReplyFieldEmit(string text)
    {
        m_Socket.Emit("getReplyInputField",text);
    }
    
    public void ChatStartEmit()
    {
        m_Socket.Emit("chatStart","chatStart");
    }
    
    public void AIResultEmit()
    {
        m_Socket.Emit("giveMeAI","giveMeAI");
    }
    
    public void MeetingEnd()
    {
        m_Socket.Disconnect();
        m_Connected = false;
    }
    
    public bool calling = false;
    public byte[] test;
    public byte[] test1;
    public float[] floatTest;
    public byte[] receiveVoice;
    
    
    private void Update()
    {
        #region multiThreadManager
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


        foreach (var a in m_ImageDownloadActions)
        {
            a.Invoke();
        }

        m_ImageDownloadActions.Clear();


        foreach (var a in m_SummaryDownload)
        {
            a.Invoke();
        }

        m_SummaryDownload.Clear();


        foreach (var a in m_NetworkGraphDownload)
        {
            a.Invoke();
        }

        m_NetworkGraphDownload.Clear();


        foreach (var a in m_FileUpload)
        {
            a.Invoke();
        }
        
        m_FileUpload.Clear();
        
        foreach (var a in m_ReplyActions)
        {
            a.Invoke();
        }
        m_ReplyActions.Clear();
        
        foreach (var a in m_OCRActions)
        {
            a.Invoke();
        }
        m_OCRActions.Clear();
        
        foreach (var a in m_VisionActions)
        {
            a.Invoke();
        }
        m_VisionActions.Clear();


        #endregion

        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    Debug.LogError("녹화 시작");
        //    mic.clip = Microphone.Start(Microphone.devices[0], false, 6, 44100);
        //}

        //if (Input.GetKeyUp(KeyCode.Z))
        //{
        //    Debug.LogError("녹화 중");

        //    //byte[] sendByte = GetClipData(mic.clip);

        //    //m_Socket.Emit("message",sendByte);
        //}
    }
}



[Serializable]
public class ChatKeywordData
{
    public List<string> mainkey;
    public List<string> subkey1;
    public List<string> subkey2;
}

public class ChatKeywordData2
{
    public string Keyword;
    public Dictionary<string, List<KeywordDataElement>> Elements;
}

public class FileUrl
{
    //public string url;
    public Dictionary <string,string> url;
}

public class fileResult
{
    public List<string> img;
    public List<string> label;
}

public class KeywordDataElement
{
    public string Title;
    public string Content;
    public string Url;
}

public class KeywordDict
{
    public string keyword;
    public List<string> Elements;
}

public class InputFieldText
{
    public string nickname;
    public string context;
}

public class ReplyInputField
{
    public string id;
    public string context;
    public string type = "reply";

}
public class UrlKeyword
{
    public string imgURL;
}




