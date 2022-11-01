using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoudnessDetection : MonoBehaviour
{

    public static AudioLoudnessDetection Instance;
    public int sampleWindow = 64;
    public AudioSource mic;
    public AudioClip microphoneClip;


    //마이크 옵션
    public bool recording = false;
    public int inputSoundSensibility = 20;
    public int stopSoundSensibility = 100;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        MicrophoneToAudioClip();
    }


    void Update()
    {

    }

    public void MicrophoneToAudioClip()
    {
        string microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, true,20,AudioSettings.outputSampleRate);

    }


    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
    }


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
        }

        if (loudness * stopSoundSensibility <= 0.03f && recording == true)
        {
            recording = false;
            Debug.Log("녹음 중지");
        }

        return ;

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




    //Byte to audioclip (바이트가 변환이 이상하다 싶을때 쓰는 확인 코)
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
