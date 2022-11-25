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
    Vector3 curRot;
    private ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    private Player player;
    public string Nickname;

    public bool webviewStart = false;
    public bool noteStart = false;

    //public int playerUID;
    public int playerwebID;
    public int playerObjectID;
    public bool camFlag = true;

    public Dictionary<int, string> idUint = new Dictionary<int, string>();
    public Vector2 drawPosition;
    

    public bool talking = false;
    public GameObject talkingImage;

    void Start()
    {

        gameObject.name = pv.IsMine ? PhotonNetwork.NickName + "(user)" : pv.Owner.NickName + "(user)";
        Nickname=pv.IsMine ? PhotonNetwork.NickName: pv.Owner.NickName;
        /*
        animsList.Add(ShirtsAnim);

        Debug.Log(UI_Character.Instance.SelectedShirts);
        */
        if (pv.IsMine)
        {

            playerwebID = (int)UnityEngine.Random.Range(1000, 2000);
            ScreenShareWhileVideoCall.Instance.Uid2 = (uint)playerwebID;

            idUint.Add(playerwebID, PhotonNetwork.NickName);


            playerObjectID = (int)UnityEngine.Random.Range(3000,5000);
            ScreenShareWhileVideoCall.Instance.Uid1 = (uint)playerObjectID;

            //ScreenShareWhileVideoCall.Instance.aig.Add(playerUID);

        }
    }

    //public GameObject sayingObject;
    public int speed = 1;
    public int rotationSpeed = 2;

    void Update()
    {
        if (noteStart == true)
        {
            //drawPosition = Drawable.Instance.sendPositionValue;
            //drawPosition = Drawable.Instance.sendPositionValue;

        }

        //if (Drawable.drawable.drawing == true)
        //{
        //    x_Position = Drawable.drawable.mouse_world_position.x;
        //    y_Position = Drawable.drawable.mouse_world_position.y;

        //    Vector2 pos = new Vector2(x_Position, y_Position); 
        //    Drawable.drawable.current_brush(pos);
        //    Debug.Log("enter");

        //}

        if (noteStart == true)
        {
            //drawPosition = Drawable.Instance.sendPositionValue;
            //drawPosition = Drawable.Instance.sendPositionValue;
            //Debug.Log(Drawable.Instance.);

        }

        if (pv.IsMine)
        {
            talking = AudioLoudnessDetection.Instance.recording;
            if (talking == true)
            {
                talkingImage.SetActive(true);
                //Debug.Log("talking");
            }
            else if (talking == false)
            {
                talkingImage.SetActive(false);
            }

            // jihyun 2022-11-23 -------- 캐릭터 이동, 회전, 애니메이션 --------------------------
            float axis_X = Input.GetAxisRaw("Horizontal");
            float axis_Z = Input.GetAxisRaw("Vertical");

            if (axis_X != 0 || axis_Z != 0)
            {
                Rotate(axis_X, axis_Z);
                Walk(playerAnim);
            }
            else
            {
                playerAnim.SetBool("IsWalking", false);
            }
            // jihyun 2022-11-23 -------- 캐릭터 이동, 회전, 애니메이션 --------------------------

        }

    }

    // jihyun 2022-11-23 -------- 캐릭터 이동, 회전, 애니메이션, speed 변수는 프리팹에서 나중에 바꿔주삼 --------------------------
    void Walk(Animator anim)
    {
        anim.SetBool("IsWalking", true);
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
    // jihyun 2022-11-23 -------- 캐릭터 이동, 회전, 애니메이션 --------------------------

 
    public void DrawStream(Vector2 position)
    {
        //Drawable.Instance.PenBrush(position);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.rotation.eulerAngles);
            stream.SendNext(transform.position);
            stream.SendNext(playerwebID);
            stream.SendNext(playerObjectID);
            stream.SendNext(webviewStart);
            stream.SendNext(idUint);
            stream.SendNext(CharCustomManager.Instance.selectedHairIndex);
            stream.SendNext(CharCustomManager.Instance.selectedShirtsIndex);
            stream.SendNext(CharCustomManager.Instance.selectedPantsIndex);
            stream.SendNext(CharCustomManager.Instance.selectedShoesIndex);
            stream.SendNext(CharCustomManager.Instance.selectedHatIndex);
            stream.SendNext(talking);
            stream.SendNext(ScreenShareWhileVideoCall.Instance.camFlag);
        }
        else
        {
            curRot = (Vector3)stream.ReceiveNext();
            curPos = (Vector3)stream.ReceiveNext();
            playerwebID = (int)stream.ReceiveNext();
            playerObjectID = (int)stream.ReceiveNext();
            
            webviewStart = (bool)stream.ReceiveNext();
            ScreenShareWhileVideoCall.Instance.playerdict = (Dictionary<int, string>)stream.ReceiveNext();


            int hairIndex = (int)stream.ReceiveNext();
            int shirtsIndex = (int)stream.ReceiveNext();
            int pantsIndex = (int)stream.ReceiveNext();
            int shoesIndex = (int)stream.ReceiveNext();
            int hatIndex = (int)stream.ReceiveNext();

            transform.rotation = Quaternion.Euler(curRot);
            transform.position = curPos;
            //GetComponent<CharacterCustomization>().SwitchCharacterSettings(gender);
            GetComponent<CharacterCustomization>().SetElementByIndex(CharacterElementType.Hair, hairIndex);
            GetComponent<CharacterCustomization>().SetElementByIndex(CharacterElementType.Shirt, shirtsIndex);
            GetComponent<CharacterCustomization>().SetElementByIndex(CharacterElementType.Pants, pantsIndex);
            GetComponent<CharacterCustomization>().SetElementByIndex(CharacterElementType.Shoes, shoesIndex);
            GetComponent<CharacterCustomization>().SetElementByIndex(CharacterElementType.Hat, hatIndex);

            //drawPosition = (Vector2)stream.ReceiveNext();
            //DrawStream(drawPosition);
            talking = (bool)stream.ReceiveNext();

            if (!(bool)stream.ReceiveNext())
            {
                UI_MainPanel.Instance.friendCamON(this);
            }
            else
                UI_MainPanel.Instance.friendCamOff(this);
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

