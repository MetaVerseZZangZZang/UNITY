using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    Vector3 curPos;
    Vector3 curRot;
    
    public int speed = 3;
    public int rotationSpeed = 10;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // jihyun 2022-11-23 -------- 캐릭터 이동, 회전, 애니메이션 --------------------------
        float axis_X = Input.GetAxisRaw("Horizontal");
        float axis_Z = Input.GetAxisRaw("Vertical");

        if (axis_X != 0 || axis_Z != 0)
        {
            Rotate(axis_X, axis_Z);
            Walk();
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }
    }
    
    void Walk()
    {
        anim.SetBool("IsWalking", true);
        // Rotate() 에서 방향을 바꿔주기 때문에 그 방향대로만 가게 해주면 된다
        transform.Translate(Vector3.forward * speed * Time.smoothDeltaTime);
    }

    void Rotate(float h, float v)
    {
        Vector3 dir = new Vector3(h, 0, v).normalized;

        float currentRot = transform.eulerAngles.y;

        currentRot = Mathf.LerpAngle(currentRot, Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg, rotationSpeed * Time.deltaTime);
        
        Quaternion currentRotation = Quaternion.Euler(0, currentRot, 0);

        transform.rotation = currentRotation; // 그 각도로 회전
    }
}
