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
        mic.clip = Microphone.Start(Microphone.devices[0].ToString(), true, 5, AudioSettings.outputSampleRate);
        var uri = new Uri("http://192.168.0.37:5100");
        m_Socket = new SocketIOUnity(uri, new SocketIOOptions()
        {
            Query = new Dictionary<string, string> { { "token", "UNITY" } },
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
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

            //AudioClip recieveClip = ByteToAudio(test1);

            //mic.clip = recieveClip;
            //mic.Play();
            //print(receiveVoice.GetType());


            //마이크 녹음 재생
            //mic.Play();


        });
        //StartCoroutine(Record());



    }



    public IEnumerator Record()
    {
        yield return null;
        StartCoroutine(ConvertAudio());
        mic.clip = Microphone.Start(Microphone.devices[0].ToString(), true, 4, AudioSettings.outputSampleRate);

    }


    IEnumerator ConvertAudio()
    {
        yield return new WaitForSeconds(2f);
        mic.clip = Microphone.Start(Microphone.devices[0].ToString(), true, 100, AudioSettings.outputSampleRate);
        //StartCoroutine(Record());
        test = GetClipData(mic.clip);
        m_Socket.Emit("message", test);
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
        time += Time.deltaTime;
        
        if (time >= 5)
        {
            time = 0;
            //print(Microphone.IsRecording(null));
            print(mic.clip.length);
            test = GetClipData(mic.clip);
            m_Socket.Emit("message", test);
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            print(1);
            mic.clip = Microphone.Start(Microphone.devices[0].ToString(), true, 100, 44100);
            test = GetClipData(mic.clip);
            m_Socket.Emit("message", test);
            //test = GetClipData(mic.clip);
            //floatTest = Send2(mic.clip);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            print(2);
            test = GetClipData(mic.clip);
            m_Socket.Emit("message", test);
            m_Socket.Emit("receiveSTT", test);
            //mic.Play();
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

