using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AdvancedPeopleSystem;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using UnityEngine;
using TMPro;

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
    public  TextMeshProUGUI Nickname;

    public bool webviewStart = false;
    public bool noteStart = false;
    

    //public int playerUID;
    public int playerwebID;
    public int playerObjectID;

    //public Dictionary<int, string> idUint = new Dictionary<int, string>();
    public Vector2 drawPosition;


    public bool talking = false;
    public GameObject talkingImage;

        //m_Camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>(); 

    public string stance = "idle";
    public bool isCollide = false;
    public Vector3 chairPos = Vector3.zero;
    public Quaternion chairRot = Quaternion.identity;
    public BoxCollider chairCollider = new BoxCollider();
    
    public bool pvCollider = false;

    void Start()
    {
        gameObject.name = pv.IsMine ? PhotonNetwork.NickName + "(user)" : pv.Owner.NickName + "(user)";
        Nickname.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName;


        //ScreenShareWhileVideoCall.Instance.playerdict = idUint;

        if (pv.IsMine)
        {
            playerwebID = (int)UnityEngine.Random.Range(1000, 2000);
            ScreenShareWhileVideoCall.Instance.Uid2 = (uint)playerwebID;
            //idUint.Add(playerwebID, PhotonNetwork.NickName);
            //ScreenShareWhileVideoCall.Instance.playerdict.Add(playerwebID, PhotonNetwork.NickName);

            playerObjectID = (int)UnityEngine.Random.Range(3000, 5000);
            ScreenShareWhileVideoCall.Instance.Uid1 = (uint)playerObjectID;
            transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            //ScreenShareWhileVideoCall.Instance.aig.Add(playerUID);



             
            //ScreenShareWhileVideoCall.Instance.playerdict = idUint;

            
        }
    }

    //public GameObject sayingObject;
    public int speed = 3;
    public int rotationSpeed = 10;

    //

    
    void Update()
    {
        ///
        //GameObject player = GameObject.Find(ScreenShareWhileVideoCall.Instance.playerdict[(int)ScreenShareWhileVideoCall.Instance.Uid2] + "(user)");
        //PlayerItem playerScript = player.GetComponent<PlayerItem>();
        //playerScript.webviewStart = false;
        //player.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        ///`435
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
            if (webviewStart == false)
            {
                float axis_X = Input.GetAxisRaw("Horizontal");
                float axis_Z = Input.GetAxisRaw("Vertical");
                if (axis_X != 0 || axis_Z != 0)
                {
                    // 앉아있는 중이면 일어서야 하므로 sit false함
                    if (stance == "sitting")
                    {
                        playerAnim.SetBool("Sit", false);
                        stance = "idle";
                        // playerAnim.Play("SitToStand");
                    }
                    else
                    {
                        // 움직일 때는 말해봐야 의미없죠? talk 애니메이션 false
                        if (talking)
                        {
                            playerAnim.SetBool("Talk", false);
                        }
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.C) && stance != "sitting")
                    {
                        // 의자와 닿아있고 사람들이 현재 의자를 사용중이지 않으면 앉기
                        if (isCollide && chairCollider.enabled)
                        {
                            transform.position = chairPos;
                            transform.rotation = chairRot;
                            chairCollider.enabled = false;
                            playerAnim.SetBool("Sit", true);
                            stance = "sitting";
                            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY |
                                             RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.C) && stance == "sitting")
                    {
                        playerAnim.SetBool("Sit", false);
                        stance = "idle";
                    }
                    else
                    {
                        if (talking)
                        {
                            playerAnim.SetBool("Talk", true);

                            // if (stance == "sitting")
                            // {
                            //     playerAnim.Play("SittingTalking");                            
                            // }
                            // else
                            // {
                            //     if (stance != "walking")
                            //     {
                            //         playerAnim.Play("StandTalking");    
                            //     }
                            // }
                        }
                        else
                        {
                            playerAnim.SetBool("Talk", false);
                        }

                        if (stance != "sitting")
                        {
                            stance = "idle";
                            playerAnim.SetBool("Sit", false);
                            playerAnim.SetBool("IsWalking", false);
                        }
                    }
                }

                // 의자에서 일어나는 애니메이션 끝날 때 의자의 collider 다시 켜주기
                if (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("SitToStand"))
                {
                    if (playerAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
                    {
                        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY |
                                         RigidbodyConstraints.FreezeRotationZ;
                        chairCollider.enabled = true;
                    }
                }
            }
            else
            {
                if (chairCollider != null)
                {
                    if (pvCollider)
                    {
                        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
                        chairCollider.enabled = false;
                    }
                    else
                    {
                        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                        chairCollider.enabled = true;
                    }
                }
            }
        }
            

            
           
    }

    // 캐릭터 움직임, 애니메이션 처리
    private void FixedUpdate()
    {
        if (pv.IsMine)
        {
            float axis_X = Input.GetAxisRaw("Horizontal");
            float axis_Z = Input.GetAxisRaw("Vertical");

            if (axis_X != 0 || axis_Z != 0)
            {
                if (stance != "sitting")
                {
                    // 움직이려 했지만 특정 애니메이션일땐 움직이지 않아야 하므로 조건 달음
                    if (!playerAnim.GetCurrentAnimatorStateInfo(0).IsName("SitToStand") && !playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Sitting") && !playerAnim.GetCurrentAnimatorStateInfo(0).IsName("StandToSit") && !playerAnim.GetCurrentAnimatorStateInfo(0).IsName("SittingTalking"))
                    {
                        Rotate(axis_X, axis_Z);
                        Walk(playerAnim);
                    }
                }
            }

        }
    }

    void Walk(Animator anim)
    {
        anim.SetBool("IsWalking", true);
        stance = "walking";
        // Rotate() 에서 방향을 바꿔주기 때문에 그 방향대로만 가게 해주면 된다

        transform.Translate(Vector3.forward * speed * Time.smoothDeltaTime);
    }

    void Rotate(float h, float v)
    {
        Vector3 dir = new Vector3(h, 0, v).normalized;
        
        float currentRot = transform.eulerAngles.y;
        
        currentRot = Mathf.LerpAngle(currentRot, Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg, rotationSpeed * Time.deltaTime);

        Quaternion currentRotation = Quaternion.Euler(0, currentRot, 0);

        transform.rotation = currentRotation;    
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Chair")
        {
            chairCollider = other.gameObject.GetComponent<BoxCollider>();
            isCollide = true;
            chairPos = other.transform.GetChild(0).transform.position;
            chairRot = other.transform.GetChild(0).transform.rotation;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Chair")
        {
            isCollide = false;
        }
    }

    public void DrawStream(Vector2 position)
    {
        //Drawable.Instance.PenBrush(position);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerwebID);
            stream.SendNext(playerObjectID);
            stream.SendNext(webviewStart);
//            stream.SendNext(idUint);
            stream.SendNext(CharCustomManager.Instance.selectedHairIndex);
            stream.SendNext(CharCustomManager.Instance.selectedShirtsIndex);
            stream.SendNext(CharCustomManager.Instance.selectedPantsIndex);
            stream.SendNext(CharCustomManager.Instance.selectedShoesIndex);
            stream.SendNext(CharCustomManager.Instance.selectedHatIndex);
            stream.SendNext(talking);
            stream.SendNext(ScreenShareWhileVideoCall.Instance.camFlag);
            stream.SendNext(ScreenShareWhileVideoCall.Instance.voiceFlag);
            stream.SendNext(isCollide);
        }
        else
        {
            playerwebID = (int)stream.ReceiveNext();
            playerObjectID = (int)stream.ReceiveNext();

            webviewStart = (bool)stream.ReceiveNext();
            //ScreenShareWhileVideoCall.Instance.playerdict = (Dictionary<int, string>)stream.ReceiveNext();
            
            int hairIndex = (int)stream.ReceiveNext();
            int shirtsIndex = (int)stream.ReceiveNext();
            int pantsIndex = (int)stream.ReceiveNext();
            int shoesIndex = (int)stream.ReceiveNext();
            int hatIndex = (int)stream.ReceiveNext();
            
            //GetComponent<CharacterCustomization>().SwitchCharacterSettings(gender);
            GetComponent<CharacterCustomization>().SetElementByIndex(CharacterElementType.Hair, hairIndex);
            GetComponent<CharacterCustomization>().SetElementByIndex(CharacterElementType.Shirt, shirtsIndex);
            GetComponent<CharacterCustomization>().SetElementByIndex(CharacterElementType.Pants, pantsIndex);
            GetComponent<CharacterCustomization>().SetElementByIndex(CharacterElementType.Shoes, shoesIndex);
            GetComponent<CharacterCustomization>().SetElementByIndex(CharacterElementType.Hat, hatIndex);
            
            //drawPosition = (Vector2)stream.ReceiveNext();
            //DrawStream(drawPosition);
            talking = (bool)stream.ReceiveNext();
            transform.GetChild(1).GetChild(0).gameObject.SetActive(webviewStart);



            if ((bool)stream.ReceiveNext())
            {
                UI_MainPanel.Instance.friendCamON(this);
            }
            else
                UI_MainPanel.Instance.friendCamOff(this);
            
            if ((bool)stream.ReceiveNext())
            {
                UI_MainPanel.Instance.friendVoiceON(this);
            }
            else
                UI_MainPanel.Instance.friendVoiceOff(this);

            pvCollider = (bool)stream.ReceiveNext();

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
