using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    Vector3 curPos;
    Vector3 curRot;
    
    public int speed = 1;
    public int rotationSpeed = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // jihyun 2022-11-23 -------- 캐릭터 이동, 회전, 애니메이션 --------------------------
        float axis_X = Input.GetAxisRaw("Horizontal");
        float axis_Z = Input.GetAxisRaw("Vertical");

        if (axis_X != 0 || axis_Z != 0)
        {
            Rotate(axis_X, axis_Z);
            Walk();
        }
    }
    
    void Walk()
    {
        // Rotate() 에서 방향을 바꿔주기 때문에 그 방향대로만 가게 해주면 된다
        transform.Translate(Vector3.forward * speed * Time.smoothDeltaTime);
    }

    void Rotate(float h, float v)
    {
        Vector3 dir = new Vector3(h, 0, v).normalized;

        Quaternion rot = Quaternion.identity; // Quaternion 값을 저장할 변수 선언 및 초기화

        rot.eulerAngles =
            new Vector3(0, Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg, 0); // 역시 eulerAngles를 이용한 오일러 각도를 Quaternion으로 저장

        transform.rotation = rot; // 그 각도로 회전
    }
}
