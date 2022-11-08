using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    public Text playerName;
    public Image shirts;
    public Sprite[] shirtsSprites;
    // Start is called before the first frame update
    
    public Rigidbody2D rb;
    public Animator anim; 
    //public SpriteRenderer spriteRenderer;
    public PhotonView pv;
    
    Vector3 curPos;
    
    private ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    
    private Player player;
    
    public void SetPlayerInfo(Player _player)
    {
        playerName.text = _player.NickName;
        playerName.color = pv.IsMine ? Color.green : Color.red;
        player = _player;
        playerProperties["shirts"] = Array.IndexOf(shirtsSprites, UI_Character.Instance.SelectedShirts);
        
        PhotonNetwork.SetCustomProperties(playerProperties);
        UpdatePlayerItem(player);
    }


    public override void OnPlayerPropertiesUpdate(Player targetPlayer,ExitGames.Client.Photon.Hashtable playerProperties)
    {
        if (player == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
        }
    }

    void UpdatePlayerItem(Player player)
    {
        if (player.CustomProperties.ContainsKey("shirts"))
        {
            shirts.sprite = shirtsSprites[(int)player.CustomProperties["shirts"]];
            playerProperties["shirts"] = (int)player.CustomProperties["shirts"];
        }
        else
        {
            playerProperties["shirts"] = 0;
        }
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
