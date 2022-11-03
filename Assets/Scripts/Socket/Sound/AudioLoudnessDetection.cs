using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SocketIOClient;

public class AudioLoudnessDetection : MonoBehaviour
{

    public static AudioLoudnessDetection Instance;
    public int sampleWindow = 64;
    public AudioSource mic;
    public AudioClip microphoneClip;

    public AudioSource sendAudio;
    public AudioClip sendClip;

    byte[] sendByte;


    //마이크 옵션
    public bool recording = false;
    public int inputSoundSensibility = 20;
    public int stopSoundSensibility = 100;



    private SocketIOUnity m_Socket;
    private string HOST = "http://192.168.0.21:5100";



    private void Awake()
    {
        Instance = this;
        var uri = new Uri(HOST);
        m_Socket = new SocketIOUnity(uri, new SocketIOOptions()
        {
            Query = new Dictionary<string, string> { { "token", "UNITY" } },
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });
        m_Socket.Connect();
        m_Socket.On("connection", (response) =>
        {
            Debug.LogWarning("Enter Socket Server");

        });

        m_Socket.On("receiveSTT", (response) =>
        {
            string text = response.GetValue<string>();
            Debug.Log(text);

        });
    }

    private void Start()
    {
        MicrophoneToAudioClip();
    }



    public float loudnessSensibility = 100;
    public float threshold = 0.1f;
    public Vector3 minScale;
    public Vector3 maxScale;
    private void Update()
    {
        float loudness = GetLoudnessFromMicrophone();
        if (loudness < threshold)
            loudness = 0;

        transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);
    }


    public void MicrophoneToAudioClip()
    {
        string microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, true , 20, AudioSettings.outputSampleRate);

    }

    
    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
    }



    int startPosition1;
    int stopPosition;
    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;


        //Debug.Log(startPosition);
        if (startPosition < 0) return 0;


        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);

        float totalLoudness = 0;

        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);
        }

        float loudness = totalLoudness / sampleWindow;
        //Debug.Log(loudness*100);


        if (loudness * inputSoundSensibility >= 1.2f && recording == false)
        {

            recording = true;
            Debug.Log("음성 녹음 해야대");
            sendClip = null;
            startPosition1 = Microphone.GetPosition(Microphone.devices[0]);


        }

        if (loudness * stopSoundSensibility <= 0.03f && recording == true)
        {
            recording = false;
            stopPosition = Microphone.GetPosition(Microphone.devices[0]);
            Debug.Log("녹음 중지");
            AudioDetect(startPosition1, clip);

            
        }

        return loudness;

    }



    public void AudioDetect(int clipPosition, AudioClip clip)
    {
        //Microphone.End(Microphone.devices[0]);
        int startPosition = clipPosition - sampleWindow;

        if (startPosition < 0)
        {
            //return null;
            startPosition = 0;
        }

        float[] samples = new float[clip.samples];

        clip.GetData(samples, 0);

        float[] cutSamples = new float[startPosition];


        Array.Copy(samples, cutSamples, cutSamples.Length);

        sendClip = AudioClip.Create("Notice", cutSamples.Length , 1, 44100, false);

        Debug.Log(cutSamples.Length - 1);

        sendClip.SetData(cutSamples, 0);

        sendByte = GetClipData(sendClip);

        //Server.Instance.VoiceEmit(sendByte);
        m_Socket.Emit("userInfo", "이승민");
        m_Socket.Emit("message", sendByte);
        Debug.Log("emit byte");
        //microphoneClip = Microphone.Start(Microphone.devices[0], true, 20, AudioSettings.outputSampleRate);
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




    //Byte to audioclip (바이트가 변환이 이상하다 싶을때 쓰는 확인 코드)
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
