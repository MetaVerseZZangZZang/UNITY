using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agora.Rtc;
using ClipperLib;
using UnityEngine.UI; 


#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
using UnityEngine.Android;
#endif


public class AgoraManager : MonoBehaviour
{
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    private ArrayList permissionList = new ArrayList() { Permission.Camera, Permission.Microphone };
#endif
    // Fill in your app ID.
    private string _appID = "5eca99dd50134c1bb6f330bce6bce9b2";
    // Fill in your channel name.
    private string _channelName = "zzang";
    // Fill in the temporary token you obtained from Agora Console.
    private string _token = "007eJxTYGhK1Vg66dG+z3GW35awXpJy/u3XOXum/veV6yw0Y/TqOawUGExTkxMtLVNSTA0MjU2SDZOSzNKMjQ2SklPNgNgyyej+w4zkhkBGhsT/6xkYoRDEZ2WoqkrMS2dgAAC3ByGV";
    // A variable to save the remote user uid.
    private uint remoteUid;
    internal VideoSurface LocalView;
    internal VideoSurface webView;
    //internal VideoSurface RemoteView;
    internal IRtcEngine RtcEngine;
    internal IRtcEngineEx RtcEngineEx = null;

    public GameObject myCam;
    public GameObject webViewCam;

    public static List<VideoSurface> FriendCamList=new List<VideoSurface>();
    
    public GameObject FriendCams;

    public static AgoraManager Instance;
    
    public static bool camFlag=true;

    public static bool voiceFlag=true;

    public List<GameObject> FriendList;
    private int count = 1;


    private CameraCapturerConfiguration _config2;
    public uint UID2 = 456;

    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckPermissions();
        if (Input.GetKeyDown(KeyCode.P))
        {
            OnWebview();
        }
    }

    private void CheckPermissions()
    {
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    foreach (string permission in permissionList)
    {
        if (!Permission.HasUserAuthorizedPermission(permission))
        {
            Permission.RequestUserPermission(permission);
        }
    }
#endif
    }

    private void SetupUI()
    {
        //GameObject go = GameObject.Find("LocalView");
        myCam.AddComponent<VideoSurface>();
        LocalView = myCam.GetComponent<VideoSurface>();
        LocalView.transform.Rotate(0.0f, 0.0f, 0);
        //go = GameObject.Find("RemoteView");

        for (int i = 0; i < FriendCams.transform.childCount; i++)
        {
            FriendCams.transform.GetChild(i).GetComponent<VideoSurface>().transform.Rotate(0.0f, 0.0f, 0f);
            FriendCamList.Add(FriendCams.transform.GetChild(i).GetComponent<VideoSurface>());

        }

    }


    public void OnWebview()
    {
        var ret = RtcEngine.StartSecondaryCameraCapture(_config2);
        ChannelMediaOptions options2 = new ChannelMediaOptions();

        options2.clientRoleType.SetValue(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        ret = RtcEngine.JoinChannel(_token, _channelName);
        Debug.Log(ret);
    }



    internal static void MakeVideoView(uint uid, string channelId = "", VIDEO_SOURCE_TYPE videoSourceType = VIDEO_SOURCE_TYPE.VIDEO_SOURCE_CAMERA)
    {
        var go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null))
        {
            return; // reuse
        }

        // create a GameObject and assign to this new user
        VideoSurface videoSurface = null;

        if (videoSourceType == VIDEO_SOURCE_TYPE.VIDEO_SOURCE_CAMERA)
        {
            videoSurface = MakeImageSurface("MainCameraView");
        }
        else if (videoSourceType == VIDEO_SOURCE_TYPE.VIDEO_SOURCE_CAMERA_SECONDARY)
        {
            videoSurface = MakeImageSurface("SecondCameraView");
        }
        else
        {
            videoSurface = MakeImageSurface(uid.ToString());
        }

        if (ReferenceEquals(videoSurface, null)) return;
        // configure videoSurface
        videoSurface.SetForUser(uid, channelId, videoSourceType);

        videoSurface.OnTextureSizeModify += (int width, int height) =>
        {
            float scale = (float)height / (float)width;
            videoSurface.transform.localScale = new Vector3(-5, 5 * scale, 1);
            Debug.Log("OnTextureSizeModify: " + width + "  " + height);
        };

        videoSurface.SetEnable(true);
    }


    private static VideoSurface MakeImageSurface(string goName)
    {
        GameObject go = new GameObject();

        if (go == null)
        {
            return null;
        }

        go.name = goName;
        // to be renderered onto
        go.AddComponent<RawImage>();
        // make the object draggable
        //go.AddComponent<UIElementDrag>();
        var canvas = GameObject.Find("VideoCanvas");
        if (canvas != null)
        {
            go.transform.parent = canvas.transform;
            Debug.Log("add video view");
        }
        else
        {
            Debug.Log("Canvas is null video view");
        }

        // set up transform
        go.transform.Rotate(0f, 0.0f, 180.0f);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = new Vector3(2f, 3f, 1f);

        // configure videoSurface
        var videoSurface = go.AddComponent<VideoSurface>();
        return videoSurface;
    }

    private void SetupVideoSDKEngine()
    {
        // Create an instance of the video SDK.
        RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
        // Specify the context configuration to initialize the created instance.
        RtcEngineContext context = new RtcEngineContext(_appID, 0, 
        CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_COMMUNICATION,
        AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT, AREA_CODE.AREA_CODE_CN, null);
        // Initialize the instance.
        RtcEngine.Initialize(context);
    }

    private void InitEventHandler()
    {
        // Creates a UserEventHandler instance.
        UserEventHandler handler = new UserEventHandler(this);
        RtcEngine.InitEventHandler(handler);
    }

    public void Join()
    {
        SetupVideoSDKEngine();
        InitEventHandler();
        SetupUI();
        
        // Enable the video module.
        RtcEngine.EnableVideo();
        RtcEngine.EnableAudio();
        // Set the user role as broadcaster.
        RtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        // Set the local video view.
        LocalView.SetForUser(0, "", VIDEO_SOURCE_TYPE.VIDEO_SOURCE_CAMERA);
        // Start rendering local video.
        LocalView.SetEnable(true);
        // Join a channel.
        RtcEngine.JoinChannel(_token, _channelName);



    }


    public void Leave()
    {
        // Leaves the channel.
        RtcEngine.LeaveChannel();
        // Disable the video modules.
        RtcEngine.DisableVideo();
        RtcEngine.DisableAudio();
        // Stops rendering the remote video.
        PhotonManager.Instance.LeaveRoom();         
        for (int i = 0; i < FriendCams.transform.childCount; i++)
        {
            Destroy(FriendCams.transform.GetChild(i).gameObject);
        }
        // Stops rendering the local video.
        LocalView.SetEnable(false);
        UI_PlayerSlot.Instance.DelPlayerSlot(UI_StartPanel.Instance.name);
        FriendCamList.Clear();
    }
    

    internal class UserEventHandler : IRtcEngineEventHandler
    {
        private readonly AgoraManager _videoSample;
        
        internal UserEventHandler(AgoraManager videoSample)
        {
            _videoSample = videoSample;
        }
        // This callback is triggered when the local user joins the channel.
        public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
        {
            Debug.Log("You joined channel: " + connection.channelId);
        }

        public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
        {
            Debug.Log("inuserjoin");
            // Setup remote view.
            GameObject newFriendCam = Instantiate(Resources.Load<GameObject>("Prefabs/FriendCam"));
            newFriendCam.transform.SetParent(_videoSample.FriendCams.transform);
            newFriendCam.transform.localScale=new Vector3(1, 1, 1);
            FriendCamList.Add(newFriendCam.GetComponent<VideoSurface>());
            FriendCamList[FriendCamList.Count-1].SetEnable(true);
            
            FriendCamList[FriendCamList.Count-1].SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
            // Save the remote user ID in a variable.
            _videoSample.remoteUid = uid;

            _videoSample.FriendList[Math.Min(_videoSample.count, _videoSample.FriendList.Count - 1)].SetActive(true);
            _videoSample.count += 1;
            
        }

        // This callback is triggered when a remote user leaves the channel or drops offline.
        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {
            
            foreach (VideoSurface RemoteView in FriendCamList)
            {
                if (RemoteView.Uid==uid)
                {
                    RemoteView.SetEnable(false);
                    Destroy(RemoteView.gameObject);
                    FriendCamList.Remove(RemoteView);
                }
            }
        }

        public override void OnRemoteVideoStateChanged(RtcConnection connection, uint remoteUid,
            REMOTE_VIDEO_STATE state, REMOTE_VIDEO_STATE_REASON reason, int elapsed)
        {
            Debug.Log("uid: "+remoteUid);
            Debug.Log("state "+state);
            
            foreach (VideoSurface RemoteView in FriendCamList)
            {
                if (RemoteView.Uid==remoteUid)
                {
                    if (state == 0)
                    {
                        UI_MainPanel.Instance.friendCamOff(RemoteView);
                    }
                    else if (state==REMOTE_VIDEO_STATE.REMOTE_VIDEO_STATE_STARTING)
                    {
                        UI_MainPanel.Instance.friendCamON(RemoteView);
                    }
                }
            }
            
        }

    }

    void OnApplicationQuit()
    {
        if (RtcEngine != null)
        {
            Leave();
            RtcEngine.Dispose();
            RtcEngine = null;
        }
    }


}