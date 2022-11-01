using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoudnessDetection : MonoBehaviour
{

    public static AudioLoudnessDetection Instance;
    public int sampleWindow = 64;
    public AudioSource mic;
    public AudioClip microphoneClip;


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

        
        if (loudness*100 >= 2f && recording == false)
        {
            recording = true;
            Debug.Log("음성 녹음 해야대");

            
        }

        if (loudness * 100 <= 0.3f && recording == true)
        {
            //StartCoroutine(Recording());
            recording = false;
            Debug.Log("녹음 중지");
        }

        return totalLoudness / sampleWindow;

    }
    public bool recording = false;


    IEnumerator Recording()
    {
        yield return new WaitForSeconds(2f);
        
        
    }
}
