using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuarterViewCam : MonoBehaviour
{
    //======================================================================
    // Other Define
    //======================================================================


    //======================================================================
    // Variable
    //======================================================================

    [SerializeField]
    private Transform target;

    // 오브젝트와의 높이와 거리차이
    [SerializeField] private float distance = 10.0f;
    [SerializeField] private float height = 5.0f;

    // 오브젝트 이동시 따라가는 속도
    [SerializeField] private float heightDamping = 2.0f;
    [SerializeField] private float rotationDamping = 3.0f;

    [SerializeField]
    private Vector3 CamPosition = new Vector3(0, 5, -2);
    [SerializeField]
    private Vector3 CamRot = new Vector3(50, -25, 0);

    //======================================================================
    // Property
    //======================================================================

    


    //======================================================================
    // MonoBehaviour
    //======================================================================

    private void Awake()
    {
        //if (GameObject.FindGameObjectWithTag("Player"))
        //{
        //    target = GameObject.FindGameObjectWithTag("Player").transform;
        //}

        transform.position = CamPosition;
        transform.rotation = Quaternion.Euler(CamRot);
    }

    void FixedUpdate()
    {
        if (UI_MainPanel.Instance.conferenceStart)
        {
            target = PhotonManager.Instance.player.transform;

            // 타겟이 없으면
            if (!target)
                return;

            // 타겟의 오일러 앵글값을 할당
            float wantedRotationAngle = target.eulerAngles.y;
            float wantedHeight = target.position.y + height;

            float currentRotationAngle = transform.eulerAngles.y;
            float currentHeight = transform.position.y;

            // 원하는 회전값으로 자연스럽게
            // 마지막 인자는 1이 100%이며 0이 0%
            //currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

            //Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            Vector3 tempDis = target.position;
            //tempDis -= currentRotation * Vector3.forward * distance;

            tempDis.y = currentHeight;
            transform.position = tempDis;

            transform.position = target.position + CamPosition;

            //transform.LookAt(target);

            transform.rotation = Quaternion.Euler(CamRot);
        }

    }


    //======================================================================
    // Private
    //======================================================================


    //======================================================================
    // Protected
    //======================================================================


    //======================================================================
    // Public
    //======================================================================

    public void SetPlayerCam(Transform player)
    {
        target = player;
    }
}
