using System;
using System.Collections;
using System.Collections.Generic;
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
    
    Vector3 curPos;
    
    private ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    
    private Player player;

   
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
    /*
    public void SetPlayerInfo(Player _player)
    {
        playerName.text = _player.NickName;
        playerName.color = pv.IsMine ? Color.green : Color.red;
        player = _player;
        //playerProperties["shirts"] = Array.IndexOf(shirtsSprites, UI_Character.Instance.SelectedShirts);
        //PhotonNetwork.SetCustomProperties(playerProperties);
        //UpdatePlayerItem(player);
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
*/
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
