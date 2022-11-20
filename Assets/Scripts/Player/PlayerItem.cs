using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour, IPunObservable
{
    public Text playerName;
    public SpriteRenderer shirts;
    public Sprite[] shirtsSprites;
    // Start is called before the first frame update
    
    public Rigidbody2D rb;
    public Animator PlayerAnim; 
    //public Animator ShirtsAnim; 
    //public SpriteRenderer spriteRenderer;
    public PhotonView pv;
    private List<Animator> animsList = new List<Animator>();
    
    Vector3 curPos;
    
    private ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    
    private Player player;

    public bool webviewStart = false;
    public bool noteStart = false;

    public int playerUID;

    public Dictionary<int, string> idUint = new Dictionary<int, string>();


    public Vector2 drawPosition;

    void Awake()
    {
        playerName.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName;
        playerName.color = pv.IsMine ? Color.green : Color.red;
    }
    
    void Start()
    {    

        gameObject.name = pv.IsMine ? PhotonNetwork.NickName+"(user)" : pv.Owner.NickName+"(user)";



        animsList.Add(PlayerAnim);
        /*
        animsList.Add(ShirtsAnim);

        Debug.Log(UI_Character.Instance.SelectedShirts);
        */

        if (pv.IsMine)
        {
            Debug.Log("Before "+shirts.sprite );
            shirts.sprite = UI_Character.Instance.SelectedShirts;
            Debug.Log("After "+shirts.sprite );

            playerUID = (int)UnityEngine.Random.Range(1000, 2000);
            ScreenShareWhileVideoCall.Instance.Uid2= (uint)playerUID;

            idUint.Add(playerUID, PhotonNetwork.NickName);

            //ScreenShareWhileVideoCall.Instance.aig.Add(playerUID);

        }
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
                rb.velocity = Vector2.right * 4f;

                foreach (Animator ani in animsList)
                {
                    ani.SetBool("Walk_RightArrow",true);
                    ani.SetBool("Walk_LeftArrow", false);
                    ani.SetBool("Walk_UpArrow", false);
                    ani.SetBool("Walk_DownArrow", false);
                }
            }

            if (axis_X == -1)
            {
                rb.velocity = Vector2.left * 4f;
                foreach (Animator ani in animsList)
                {
                    ani.SetBool("Walk_LeftArrow", true);
                    ani.SetBool("Walk_UpArrow", false);
                    ani.SetBool("Walk_DownArrow", false);
                    ani.SetBool("Walk_RightArrow", false);
                        
                }
            }

            if (axis_Y == 1)
            {
                rb.velocity = Vector2.up * 4f;
                
                foreach (Animator ani in animsList)
                {
                    ani.SetBool("Walk_UpArrow", true);
                    ani.SetBool("Walk_RightArrow", false);
                    ani.SetBool("Walk_LeftArrow", false);
                    ani.SetBool("Walk_DownArrow", false);
                }
            }

            if (axis_Y == -1)
            {
                rb.velocity = Vector2.down * 4f;

                foreach (Animator ani in animsList)
                {
                    ani.SetBool("Walk_DownArrow", true);
                    ani.SetBool("Walk_RightArrow", false);
                    ani.SetBool("Walk_LeftArrow", false);
                    ani.SetBool("Walk_UpArrow", false);
                    
                }
                
            }

            if (axis_X == 0 && axis_Y == 0)
            {
                rb.velocity = Vector2.zero;
                foreach (Animator ani in animsList)
                {
                    ani.SetBool("Walk_DownArrow", false);
                    ani.SetBool("Walk_RightArrow", false);
                    ani.SetBool("Walk_LeftArrow", false);
                    ani.SetBool("Walk_UpArrow", false);
                    
                }
            }

        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(webviewStart);
            stream.SendNext(playerUID);
            stream.SendNext(idUint);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
            webviewStart = (bool)stream.ReceiveNext();
            playerUID = (int)stream.ReceiveNext();
            ScreenShareWhileVideoCall.Instance.playerdict = (Dictionary<int, string>)stream.ReceiveNext();
            //test = (Texture)stream.SendNext(test);
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
