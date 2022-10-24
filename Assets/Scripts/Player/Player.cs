using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
//using Cinemachine;

public class Player : MonoBehaviour, IPunObservable
{
    public Rigidbody2D rb;
    public Animator anim;
    public SpriteRenderer spriteRenderer;
    public PhotonView pv;
    public Text nickNameText;


    Vector3 curPos;
 
    void Awake()
    {
        nickNameText.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName;
        nickNameText.color = pv.IsMine ? Color.green : Color.red;

        //if (pv.IsMine)
        //{
        //    try
        //    {
        //        var cm = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
        //        cm.Follow = transform;
        //        cm.LookAt = transform;
        //    }

        //    catch
        //    {
        //        return;
        //    }
                        
        //} 
    }

    void Update()
    {
        if (pv.IsMine)
        {
            float axis_X = Input.GetAxisRaw("Horizontal");
            float axis_Y = Input.GetAxisRaw("Vertical");
            if (axis_X == 1)
            {
                rb.velocity = Vector2.right * 4f;
                anim.SetBool("Walk_RightArrow",true);
                anim.SetBool("Walk_LeftArrow", false);
                anim.SetBool("Walk_UpArrow", false);
                anim.SetBool("Walk_DownArrow", false);
            }

            if (axis_X == -1)
            {
                rb.velocity = Vector2.left * 4f;
                anim.SetBool("Walk_LeftArrow", true);
                anim.SetBool("Walk_UpArrow", false);
                anim.SetBool("Walk_DownArrow", false);
                anim.SetBool("Walk_RightArrow", false);
            }

            if (axis_Y == 1)
            {
                rb.velocity = Vector2.up * 4f;
                anim.SetBool("Walk_UpArrow", true);
                anim.SetBool("Walk_RightArrow", false);
                anim.SetBool("Walk_LeftArrow", false);
                anim.SetBool("Walk_DownArrow", false);
            }

            if (axis_Y == -1)
            {
                rb.velocity = Vector2.down * 4f;
                anim.SetBool("Walk_DownArrow", true);
                anim.SetBool("Walk_RightArrow", false);
                anim.SetBool("Walk_LeftArrow", false);
                anim.SetBool("Walk_UpArrow", false);
            }

            if (axis_X == 0 && axis_Y == 0)
            {
                rb.velocity = Vector2.zero;
                anim.SetBool("Walk_RightArrow", false);
                anim.SetBool("Walk_LeftArrow", false);  
                anim.SetBool("Walk_UpArrow", false);
                anim.SetBool("Walk_DownArrow", false);

            }

        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);

        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Collider2D col = collision.gameObject.GetComponent<Collider2D>();
            col.isTrigger = true;
        }   
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Collider2D col = collision.gameObject.GetComponent<Collider2D>();
            col.isTrigger = false;
        }
    }


}
