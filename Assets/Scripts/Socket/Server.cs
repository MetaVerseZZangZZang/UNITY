<<<<<<< HEAD:Assets/Socket/Server.cs
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

public class Server : MonoBehaviour
{
    private SocketIOUnity m_Socket;
    private bool m_Connected = false;
    public AudioSource mic;
    public AudioClip micClip;


    private void Start()
    {
        mic = transform.GetComponent<AudioSource>();
        var uri = new Uri("http://192.168.0.37:5100");
        m_Socket = new SocketIOUnity(uri, new SocketIOOptions()
        {
            Query = new Dictionary<string, string>{{"token", "UNITY" }},Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });
        m_Socket.Connect();

        m_Socket.On("connection", (response) =>
        {
            Debug.LogError("연결 완");
            Debug.LogError(response.GetValue<string>());

            m_Connected = true;
        });

        m_Socket.On("message", (response) =>
        {
            Debug.Log(response.GetValue<string>());

        });

        m_Socket.On("receiveVoice", (response) =>
        {
            Debug.LogError("receive");
            //Debug.LogError(response.GetValue<byte>());
            //var data = response.GetType()
            test1 = response.GetValue<byte[]>();
            //print(receiveVoice.GetType());
            m_Connected = true;
        });
        //StartCoroutine(Test());

    }

    private IEnumerator Test()
    {
        var wait = new WaitForEndOfFrame();

        int a = 1;
        mic.clip = Microphone.Start(Microphone.devices[0].ToString(), true, 3, 44100);
        test = GetClipData(mic.clip);


        while (true)
        {
            yield return wait;

            if(!m_Connected)
            {
                continue;
            }

            //mic.clip = Microphone.Start(Microphone.devices[0].ToString(), true, 3, 44100);
            //byte[] audioStream = GetClipData(mic.clip);
            

            //m_Socket.On("message", (response) =>
            //{
            //    Debug.Log("소켓 답장" + response.GetValue<string>());

            //});
            m_Socket.Emit("message", test);


        }
    }


    public bool calling = false;
    public byte[] test;
    //public bool recording = false;
    public byte[] test1;

    public float[] floatTest;
    public byte[] receiveVoice;



    private void Update()
    {
  
        if (Input.GetKeyDown(KeyCode.A))
        {
            print(1);
            mic.clip = Microphone.Start(Microphone.devices[0].ToString(), false, 3, 44100);
            //test = GetClipData(mic.clip);
            //floatTest = Send2(mic.clip);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            print(2);
            test = GetClipData(mic.clip);
            m_Socket.Emit("message", test);
            mic.Play();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            // byte to audioclip
            float[] samples = new float[test.Length / 4]; //size of a float is 4 bytes

            Buffer.BlockCopy(test, 0, samples, 0, test.Length);

            int channels = 1; //Assuming audio is mono because microphone input usually is
            int sampleRate = 44100; //Assuming your samplerate is 44100 or change to 48000 or whatever is appropriate


            //print(samples.Length);
            AudioClip clip = AudioClip.Create("ClipName", samples.Length, channels, sampleRate, false);
            clip.SetData(samples, 0);
            mic.clip = clip;
            mic.Play();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {  
            float[] samples = new float[test1.Length / 4]; //size of a float is 4 bytes

            Buffer.BlockCopy(test1, 0, samples, 0, test1.Length);

            int channels = 1; //Assuming audio is mono because microphone input usually is
            int sampleRate = 44100; //Assuming your samplerate is 44100 or change to 48000 or whatever is appropriate


            print(samples.Length);
            AudioClip clip = AudioClip.Create("ClipName", samples.Length, channels, sampleRate, false);
            clip.SetData(samples, 0);


            mic.clip = clip;
            mic.Play();
        }
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


}

=======
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Server : MonoBehaviour
{

    public static Server instance;


    #region private members
    private TcpListener tcpListener;
    private Thread tcpListenerThread;
    private TcpClient connectedTcpClient;
    #endregion

    void Awake()
    {
        Debug.Log("Start Server");
        instance = this;

        // Start TcpServer background thread
        tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequest));
        tcpListenerThread.IsBackground = true;
        tcpListenerThread.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Runs in background TcpServerThread; Handles incomming TcpClient requests
    private void ListenForIncommingRequest()
    {
        try
        {
            // Create listener on 192.168.0.2 port 50001
            tcpListener = new TcpListener(IPAddress.Parse("192.168.0.11"), 50001);
            tcpListener.Start();
            Debug.Log("Server is listening");

            while (true)
            {
                using (connectedTcpClient = tcpListener.AcceptTcpClient())
                {
                    // Get a stream object for reading
                    using (NetworkStream stream = connectedTcpClient.GetStream())
                    {
                        // Read incomming stream into byte array.
                        do
                        {
                            // TODO
                        } while (true);
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("SocketException " + socketException.ToString());
        }
    }
}

>>>>>>> f4dda1898801515cfe645b8d4cd17c3e81410d68:Assets/Scripts/Socket/Server.cs
