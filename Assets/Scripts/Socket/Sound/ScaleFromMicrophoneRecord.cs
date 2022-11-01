using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFromMicrophoneRecord : MonoBehaviour
{
    public static ScaleFromMicrophoneRecord Instance;
    public AudioSource source;
    public Vector3 minScale;
    public Vector3 maxScale;
    public AudioLoudnessDetection detector;


    public float loudnessSensibility = 100;
    public float threshold = 0.1f;



    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {

    }

    public bool isSaying = false;
    public bool startSaying = false;

    public AudioSource testMic;
    void Update()
    {
        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;
        //Debug.Log(loudness);
        if (loudness < threshold)
            loudness = 0;

        transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);

        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("input A");
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("up A");
        }


    }
}
