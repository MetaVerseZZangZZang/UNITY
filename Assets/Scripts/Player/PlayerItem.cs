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
    public int speed=5;
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
            
            if (axis_X == 1)   //오
            {
                transform.rotation=Quaternion.Euler(0,90,0);
                transform.Translate(5f * Time.deltaTime, 0, 0);
                playerAnim.SetBool("walk",true);
            }

            if (axis_X == -1)   //왼
            {
                transform.rotation=Quaternion.Euler(0,-90,0);
                transform.Translate(-5f * Time.deltaTime, 0, 0);
                playerAnim.SetBool("walk",true);
            }

            if (axis_Y == 1)   //위
            {
                transform.Translate(0, 0, 5f * Time.deltaTime);
                transform.rotation=Quaternion.Euler(0,0,0);
                playerAnim.SetBool("walk",true);
                
            }

            if (axis_Y == -1)    //아래
            {
                transform.Translate(0, 0, -5f * Time.deltaTime);
                transform.rotation=Quaternion.Euler(0,180,0);
                playerAnim.SetBool("walk",true);
            }

            if (axis_X == 0 && axis_Y == 0)
            {
                playerAnim.SetBool("walk",false);
            }

        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
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
