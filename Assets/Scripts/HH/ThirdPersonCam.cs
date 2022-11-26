using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    public GameObject Target;               // 카메라가 따라다닐 타겟

    public float offsetX = 0.0f;            // 카메라의 x좌표
    public float offsetY = 3.0f;           // 카메라의 y좌표
    public float offsetZ = -3.0f;          // 카메라의 z좌표

    public float CameraSpeed = 10.0f;       // 카메라의 속도
    Vector3 TargetPos;                      // 타겟의 위치
    
    public float angleX = 35.0f;
    public float angleY = 0.0f;
    public float angleZ = 0.0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (UI_MainPanel.Instance.conferenceStart)
        {
            Target = PhotonManager.Instance.player;
            // 타겟의 x, y, z 좌표에 카메라의 좌표를 더하여 카메라의 위치를 결정
            TargetPos = new Vector3(
                Target.transform.position.x + offsetX,
                Target.transform.position.y + offsetY,
                Target.transform.position.z + offsetZ
            );

            // 카메라의 움직임을 부드럽게 하는 함수(Lerp)
            transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);

            Quaternion currentRot = transform.rotation;

            currentRot = Quaternion.Lerp(currentRot, Quaternion.Euler(angleX, angleY, angleZ), 3.0f * Time.deltaTime);

            transform.rotation = currentRot;
        }
    }
}
