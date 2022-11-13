using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agora.Rtc;
using ClipperLib;
using System.Runtime.Remoting.Lifetime;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;


#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
using UnityEngine.Android;
#endif


public class AgoraManager : MonoBehaviour
{
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    private ArrayList permissionList = new ArrayList() { Permission.Camera, Permission.Microphone };
#endif
    // Fill in your app ID.
    private string _appID = "88a3697b8dcd499f8f01fdcb6cdb6db9";
    // Fill in your channel name.
    private string _channelName = "zzang";
    // Fill in the temporary token you obtained from Agora Console.
    private string _token =
        "007eJxTYDCb0TK3pPMBd1DC3ob5GbzNc7e9efrw7/PIxn+3xIqWr+lUYLCwSDQ2szRPskhJTjGxtEyzSDMwTEtJTjJLTkkyS0my3BZRmNwQyMjgJRTKwsgAgSA+K0NVVWJeOgMDAEymIqg=";
    // A variable to save the remote user uid.
    private uint remoteUid;
    internal VideoSurface LocalView;
    //internal VideoSurface RemoteView;
    internal IRtcEngine RtcEngine;

    public GameObject myCam;

    public static List<VideoSurface> FriendCamList=new List<VideoSurface>();
    
    public GameObject FriendCams;

    public static AgoraManager Instance;
    
    public static bool camFlag=true;

    public static bool voiceFlag=true;

    public List<GameObject> FriendList;
    private int count = 1;
    
    
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