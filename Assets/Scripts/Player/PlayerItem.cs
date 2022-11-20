using System;
using System.Collections;
using System.Collections.Generic;
using AdvancedPeopleSystem;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour, IPunObservable
{
    //public Text playerName;
    // Start is called before the first frame update
    public Rigidbody rb;
    public Animator playerAnim; 
    //public SpriteRenderer spriteRenderer;
    public PhotonView pv;
    public CharacterCustomization cc;
    Vector3 curPos;
    
    private ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    private Player player;
    public string Nickname;

   
    void Awake()
    {
        //playerName.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName;
        //playerName.color = pv.IsMine ? Color.green : Color.red;
    }
    
    void Start()
    {
        /*
        animsList.Add(ShirtsAnim);

        Debug.Log(UI_Character.Instance.SelectedShirts);
        */
    }
    

    //public GameObject sayingObject;
    void Update()
    {
        
        if (pv.IsMine)
        {
            float axis_X = Input.GetAxisRaw("Horizontal");
            float axis_Y = Input.GetAxisRaw("Vertical");
            if (axis_X == 1)   
            {
                transform.rotation=Quaternion.Euler(0,90,0);
                playerAnim.SetBool("walk",true);
            }

            if (axis_X == -1)  
            {
                transform.rotation=Quaternion.Euler(0,-90,0);
                playerAnim.SetBool("walk",true);
            }

            if (axis_Y == 1)
            {
                transform.rotation=Quaternion.Euler(0,0,0);
                playerAnim.SetBool("walk",true);
            }

            if (axis_Y == -1)
            {
                transform.rotation=Quaternion.Euler(0,180,0);
                playerAnim.SetBool("walk",true);
            }

            if (axis_X == 0 && axis_Y == 0)
            {
                playerAnim.SetBool("walk",false);
            }
            
            Vector3 getVel = new Vector3(axis_X, 0, axis_Y) * 4f;
            rb.velocity = getVel;
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext( CharacterManager.Instance.selectedHairIndex );
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
            int hairIndex = (int)stream.ReceiveNext();
           // Debug.Log("HairIndex "+hairIndex);

            if (Nickname == info.Sender.NickName)
            {
                GetComponent<CharacterCustomization>().SetElementByIndex(CharacterElementType.Hair,hairIndex);
            }

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
