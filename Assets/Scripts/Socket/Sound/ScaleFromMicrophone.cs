using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFromMicrophone : MonoBehaviour
{
    public AudioSource source;
    public Vector3 minScale;
    public Vector3 maxScale;
    public AudioLoudnessDetection detector;


    public float loudnessSensibility = 20;
    public float threshold = 0.1f;


    public AudioSource recordAudio;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;

        if (loudness < threshold)
            loudness = 0;


        transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);


        if (3 <=  transform.localScale.x)
        {
            Debug.Log("음성 감지");

            recordAudio.clip = Microphone.Start(Microphone.devices[1], true, 5, 46100);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            recordAudio.Play();
        }

        if (2 > transform.localScale.x)
        {

            float time = 0;
            time = Time.deltaTime;
            if (time == 1)
            {
                Microphone.End(Microphone.devices[1]);
                Debug.Log("음성 녹음 중지");
            }
        }
    }
}
