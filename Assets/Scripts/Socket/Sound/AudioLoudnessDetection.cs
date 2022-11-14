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

    public AudioSource sendAudio;
    public AudioClip sendClip;

    byte[] sendByte;


    //public AudioSource test;

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
    public void MicrophoneToAudioClip()
    {
        string microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, true, 400, 44100);
    }

    public float loudnessSensibility = 100;
    public float threshold = 0.1f;
    public Vector3 minScale;
    public Vector3 maxScale;

    public bool joined = false;
    private void Update()
    {
        if (joined)
        {

            float loudness = GetLoudnessFromMicrophone();
            if (loudness < threshold)
                loudness = 0;

            transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);

        }
        /*
        if (Input.GetKeyDown(KeyCode.A))
        {
            test.Play();
        }
        */
    }

    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
    }



    int startPosition1;
    int stopPosition;
    public float time;

    public float sensibility;
    public float outputsensibility;
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

        sensibility = loudness * inputSoundSensibility;
        outputsensibility = loudness * inputSoundSensibility;
        if (loudness * inputSoundSensibility >= 2f && recording == false)
        {
            Debug.Log($"Start input sensibility:  {loudness * inputSoundSensibility}");
            recording = true;
            Debug.Log("음성 녹음 해야대");
            sendClip = null;
            startPosition1 = Microphone.GetPosition(Microphone.devices[0]);
        }
        
        if (loudness * stopSoundSensibility <= 0.2f && recording == true)
        {
            
            time += Time.deltaTime;
            if (time > 0.1f)
            {
                //Debug.Log($"Stop input sensibility:  {loudness * stopSoundSensibility}");
                recording = false;
                stopPosition = Microphone.GetPosition(Microphone.devices[0]);

                Debug.Log("녹음 중지");
                AudioDetect(startPosition1, stopPosition, clip);
                time = 0;

            }
        }

        return loudness;

    }



    public void AudioDetect(int clipStartPosition, int clipStopPosition, AudioClip clip)
    {
        //Microphone.End(Microphone.devices[0]);
        int startPosition = clipStartPosition;

        if (startPosition == 0)
        {
            //return null;
            return;
        }

        float[] samples = new float[clip.samples];

        clip.GetData(samples, 0);


        float[] cutSamples = new float[(clipStopPosition - startPosition) + 10000];

        //Debug.Log($"samples.Length : {samples.Length},cutSamples.Length : {cutSamples.Length}");

        Array.Copy(samples, startPosition - 10000 < 0 ? 0 : startPosition - 10000, cutSamples, 0, cutSamples.Length);

        sendClip = AudioClip.Create("Notice", cutSamples.Length, 1, 44100, false);
        sendClip.SetData(cutSamples, 0);
        //test.clip = sendClip;
        //SaveWave.Save(@"D:\voice", sendClip);

        sendByte = GetClipData(sendClip);

        Server.Instance.VoiceEmit(sendByte);

        Debug.Log("emit byte");
        //microphoneClip = Microphone.Start(Microphone.devices[0], true, 20, AudioSettings.outputSampleRate);
        microphoneClip = Microphone.Start(Microphone.devices[0], true, 20, 44100);
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