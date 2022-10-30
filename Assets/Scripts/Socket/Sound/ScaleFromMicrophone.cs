using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFromMicrophone : MonoBehaviour
{

    public static ScaleFromMicrophone Instance;
    public AudioSource source;
    public Vector3 minScale;
    public Vector3 maxScale;
    //public AudioLoudnessDetection detector;


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
    void Update()
    {
        if (startSaying == true)
        {
            float loudness = Server.Instance.GetLoudnessFromMicrophone() * loudnessSensibility;

            if (loudness < threshold)
                loudness = 0;


            transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);


            if (transform.localScale.x >= 2)
            {
                isSaying = true;
            }
            if (transform.localScale.x < 1.4)
            {
                isSaying = false;
            }
        }
        

    }
}
