using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AdvancedPeopleSystem;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using TreeEditor;
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

    public bool webviewStart = false;
    public bool noteStart = false;

    public int playerUID;

    public Dictionary<int, string> idUint = new Dictionary<int, string>();


    public Vector2 drawPosition;

   
    void Awake()
    {
        //playerName.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName;
        //playerName.color = pv.IsMine ? Color.green : Color.red;
    }
    
    void Start()
    {    

        gameObject.name = pv.IsMine ? PhotonNetwork.NickName+"(user)" : pv.Owner.NickName+"(user)";

        /*
        animsList.Add(ShirtsAnim);

        Debug.Log(UI_Character.Instance.SelectedShirts);
        */

        if (pv.IsMine)
        {

            playerUID = (int)UnityEngine.Random.Range(1000, 2000);
            ScreenShareWhileVideoCall.Instance.Uid2= (uint)playerUID;

            idUint.Add(playerUID, PhotonNetwork.NickName);

            //ScreenShareWhileVideoCall.Instance.aig.Add(playerUID);

        }
    }
    //public GameObject sayingObject;
    void Update()
    {
        if (noteStart == true)
        {
            drawPosition = Drawable.Instance.sendPositionValue;
            //drawPosition = Drawable.Instance.sendPositionValue;


        }
        if (pv.IsMine)
        {

            float axis_X = Input.GetAxisRaw("Horizontal");
            float axis_Y = Input.GetAxisRaw("Vertical");
            if (axis_X == 1)
            {
                transform.rotation=Quaternion.Euler(0,90,0);
                transform.Translate(0, 0, 2f * Time.deltaTime);
                playerAnim.SetBool("walk",true);
            }

            if (axis_X == -1)   //왼
            {
                transform.rotation=Quaternion.Euler(0,-90,0);
                transform.Translate(0, 0, 2f * Time.deltaTime);
                playerAnim.SetBool("walk",true);
            }

            if (axis_Y == 1)   //위
            {
                transform.rotation=Quaternion.Euler(0,0,0);
                transform.Translate(0, 0, 2f * Time.deltaTime);
                playerAnim.SetBool("walk",true);
                
            }

            if (axis_Y == -1)    //아래
            {
                transform.rotation=Quaternion.Euler(0,180,0);
                transform.Translate(0, 0, 2f * Time.deltaTime);
                playerAnim.SetBool("walk",true);
            }

            if (axis_X == 0 && axis_Y == 0)
            {
                playerAnim.SetBool("walk",false);
            }

        }
    }

    public void DrawStream(Vector2 position)
    {
        Drawable.Instance.PenBrush(position);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(webviewStart);
            stream.SendNext(playerUID);
            stream.SendNext(idUint);
            stream.SendNext(CharCustomManager.Instance.selectedGender);
            stream.SendNext(CharCustomManager.Instance.selectedHairIndex);
            stream.SendNext(CharCustomManager.Instance.selectedShirtsIndex);
            stream.SendNext(CharCustomManager.Instance.selectedPantsIndex);
            stream.SendNext(CharCustomManager.Instance.selectedShoesIndex);
            stream.SendNext(CharCustomManager.Instance.selectedHatIndex);
            
            
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
            webviewStart = (bool)stream.ReceiveNext();
            playerUID = (int)stream.ReceiveNext();
            ScreenShareWhileVideoCall.Instance.playerdict = (Dictionary<int, string>)stream.ReceiveNext();
            //test = (Texture)stream.SendNext(test);
            transform.position = curPos;
            string gender = (string)stream.ReceiveNext();
            int hairIndex = (int)stream.ReceiveNext();
            int shirtsIndex = (int)stream.ReceiveNext();
            int pantsIndex = (int)stream.ReceiveNext();
            int shoesIndex = (int)stream.ReceiveNext();
            int hatIndex = (int)stream.ReceiveNext();
            
            GetComponent<CharacterCustomization>().SwitchCharacterSettings(gender);
            GetComponent<CharacterCustomization>().SetElementByIndex(CharacterElementType.Hair,hairIndex );
            GetComponent<CharacterCustomization>().SetElementByIndex(CharacterElementType.Shirt,shirtsIndex );
            GetComponent<CharacterCustomization>().SetElementByIndex(CharacterElementType.Pants,pantsIndex );
            GetComponent<CharacterCustomization>().SetElementByIndex(CharacterElementType.Shoes,shoesIndex );
            GetComponent<CharacterCustomization>().SetElementByIndex(CharacterElementType.Hat,hatIndex );


            drawPosition = (Vector2)stream.ReceiveNext();
            DrawStream(drawPosition);

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
