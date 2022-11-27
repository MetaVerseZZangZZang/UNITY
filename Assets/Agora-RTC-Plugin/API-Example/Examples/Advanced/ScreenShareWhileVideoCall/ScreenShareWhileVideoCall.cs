using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Agora.Rtc;
using Agora.Util;
using UnityEngine.Serialization;
using Logger = Agora.Util.Logger;
using System.Collections;
using Unity.Collections;
using System.Collections.Generic;


//namespace Agora_RTC_Plugin.API_Example.Examples.Advanced.ScreenShareWhileVideoCall

public class ScreenShareWhileVideoCall : MonoBehaviour
{

    public static ScreenShareWhileVideoCall Instance;

    private Texture2D _texture;
    public Rect _rect;
    private int i = 0;
    private WebCamTexture _webCameraTexture;
    //public RawImage RawImage;
    public Vector2 CameraSize = new Vector2(1280, 960);
    public int CameraFPS = 10;
    private byte[] _shareData;
    // public RawImage test;
    public GameObject Safari;

    [FormerlySerializedAs("appIdInput")]
    [SerializeField]
    private AppIdInput _appIdInput;

    [Header("_____________Basic Configuration_____________")]
    [FormerlySerializedAs("APP_ID")]
    [SerializeField]
    private string _appID = "";

    [FormerlySerializedAs("TOKEN")]
    [SerializeField]
    private string _token = "";

    [FormerlySerializedAs("CHANNEL_NAME")]
    [SerializeField]
    private string _channelName = "";

    public Text LogText;
    internal Logger Log;
    internal IRtcEngineEx RtcEngine = null;

    public uint Uid1 = 123;
    public uint Uid2 = 345;

    public static List<VideoSurface> FriendCamList = new List<VideoSurface>();

    public GameObject FriendCams;
    public GameObject myCam;
    internal VideoSurface LocalView;

    public bool startWebview = false;

    public GameObject note;

    public Dictionary<int, string> playerdict = new Dictionary<int, string>();
    public bool camFlag = true;
    public bool voiceFlag = true;

    private void Awake()
    {
        Instance = this;

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

        UI_PlayerSlot.Instance.DelAll();
        FriendCamList.Clear();
    }

    // Use this for initialization
    public void AgoraStart()
    {
        //Uid1 = (uint)UnityEngine.Random.Range(1000, 2000);
        Safari.SetActive(false);
        LoadAssetData();
        if (CheckAppId())
        {
            InitEngine();
#if UNITY_ANDROID || UNITY_IPHONE
                GameObject.Find("winIdSelect").SetActive(false);
#else
            //PrepareScreenCapture();
#endif
            //EnableUI();
            //JoinChannel();
            StartCoroutine(GetUID());
        }
    }


    IEnumerator GetUID()
    {
        yield return new WaitForSeconds(1f);
        JoinChannelCamera();
    }



    public void WebviewStop()
    {
        GameObject playerID = GameObject.Find(UI_StartPanel.Instance.userName + "(user)");
        Transform playerCanvas = playerID.transform.GetChild(1).GetChild(0);

        PlayerItem playerScript = playerID.GetComponent<PlayerItem>();
        playerScript.webviewStart = false;
        Safari.SetActive(false);
        playerCanvas.gameObject.SetActive(false);

        //ScreenShareLeaveChannel();
        RtcEngine.LeaveChannelEx(new RtcConnection(_channelName, Uid2));
    }

    public void NoteClick()
    {
        if (!note.activeSelf)
            note.SetActive(true);
        else
        {
            NoteStop();
        }
    }

    public void NoteStop()
    {
        note.SetActive(false);
    }


    public void WebviewStart()
    {

        LoadAssetData();
        //Safari.SetActive(true);
        if (CheckAppId())
        {
            if (Safari.activeSelf == false)
            {
                GameObject playerID = GameObject.Find(UI_StartPanel.Instance.userName + "(user)");

                PlayerItem playerScript = playerID.GetComponent<PlayerItem>();

                playerScript.webviewStart = true;
                Safari.SetActive(true);
                WebViewScript.Instance.ChatWebview("http://www.youtube.com");

                //InitCameraDevice();
                InitTexture();
                InitEngine();
                SetExternalVideoSource();
                JoinChannelWebview();
                startWebview = true;

                Transform playerCanvas = playerID.transform.GetChild(1).GetChild(0);
                playerCanvas.gameObject.SetActive(true);
                RawImage playerWebImage = playerCanvas.GetComponent<RawImage>();
                StartCoroutine(BringWebTexture(playerWebImage));
            }
            else
            {
                InitCameraDevice();
                //InitTexture();
                //InitEngine();
                SetExternalVideoSource();
                JoinChannelWebview();
            }
        }
    }

    IEnumerator BringWebTexture(RawImage webImageTexture)
    {
        yield return new WaitForSeconds(1f);
        webImageTexture.texture = WebViewObject.Instance.texture;

    }

    private void SetupUI()
    {
        //GameObject go = GameObject.Find("LocalView");

        //myCam.AddComponent<VideoSurface>();
        LocalView = myCam.GetComponent<VideoSurface>();
        LocalView.transform.Rotate(0.0f, 0.0f, 0);
        //go = GameObject.Find("RemoteView");
        LocalView.SetForUser(0, "", VIDEO_SOURCE_TYPE.VIDEO_SOURCE_CAMERA);
        // Start rendering local video.
        LocalView.SetEnable(true);
    }

    private void JoinChannelWebview()
    {
        //RtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        //options.clientRoleType.SetValue(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        ChannelMediaOptions options1 = new ChannelMediaOptions();
        options1.autoSubscribeAudio.SetValue(false);
        options1.autoSubscribeVideo.SetValue(false);

        options1.publishCameraTrack.SetValue(false);
        options1.publishScreenTrack.SetValue(false);
        options1.enableAudioRecordingOrPlayout.SetValue(false);
        options1.clientRoleType.SetValue(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);

        RtcEngine.JoinChannel(_token,_channelName,Uid2,options1);
        //var ret = RtcEngine.JoinChannelEx(_token, new RtcConnection(_channelName, Uid2), options);
    }

    private void JoinChannelCamera()
    {
        RtcEngine.EnableAudio();
        RtcEngine.EnableVideo();
        RtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);

        SetupUI();

        ChannelMediaOptions options1 = new ChannelMediaOptions();
        options1.autoSubscribeAudio.SetValue(true);
        options1.autoSubscribeVideo.SetValue(false);

        options1.publishCameraTrack.SetValue(false);
        options1.publishScreenTrack.SetValue(false);
        options1.enableAudioRecordingOrPlayout.SetValue(true);
        options1.clientRoleType.SetValue(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);


        RtcEngine.JoinChannelEx(_token, new RtcConnection(_channelName, Uid1), options1);
    }

    private void SetExternalVideoSource()
    {
        var ret = RtcEngine.SetExternalVideoSource(true, false, EXTERNAL_VIDEO_SOURCE_TYPE.VIDEO_FRAME, new SenderOptions());
        //this.Log.UpdateLog("SetExternalVideoSource returns:" + ret);
    }

    private void InitCameraDevice()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        _webCameraTexture = new WebCamTexture(devices[0].name, (int)_rect.width, (int)_rect.height, CameraFPS);
        _webCameraTexture.Play();
    }
    private void InitTexture()
    {
        _rect = new Rect(0f, 0f, 1148, 700);
        _texture = new Texture2D((int)_rect.width, (int)_rect.height, TextureFormat.RGBA32, false);
    }
    private void Update()
    {
        if (startWebview == true)
        {
            PermissionHelper.RequestMicrophontPermission();
            StartCoroutine(ShareScreen());
        }
    }

    private IEnumerator ShareScreen()
    {
        yield return new WaitForEndOfFrame();
        IRtcEngine rtc = Agora.Rtc.RtcEngine.Instance;
        if (rtc != null)
        {
            _texture = new Texture2D((int)_rect.width, (int)_rect.height, TextureFormat.RGBA32, false);

            _texture.ReadPixels(_rect, 0, 0);
            _texture.Apply();

#if UNITY_2018_1_OR_NEWER
            NativeArray<byte> nativeByteArray = _texture.GetRawTextureData<byte>();
            if (_shareData?.Length != nativeByteArray.Length)
            {
                _shareData = new byte[nativeByteArray.Length];
            }
            nativeByteArray.CopyTo(_shareData);
#else
                _shareData = _texture.GetRawTextureData();
#endif

            ExternalVideoFrame externalVideoFrame = new ExternalVideoFrame();
            externalVideoFrame.type = VIDEO_BUFFER_TYPE.VIDEO_BUFFER_RAW_DATA;
            externalVideoFrame.format = VIDEO_PIXEL_FORMAT.VIDEO_PIXEL_RGBA;
            externalVideoFrame.buffer = _shareData;
            externalVideoFrame.stride = (int)_rect.width;
            externalVideoFrame.height = (int)_rect.height;
            externalVideoFrame.cropLeft = 0;
            externalVideoFrame.cropTop = 0;
            externalVideoFrame.cropRight = 0;
            externalVideoFrame.cropBottom = 0;
            externalVideoFrame.rotation = 180;
            externalVideoFrame.timestamp = System.DateTime.Now.Ticks / 10000;
            var ret = rtc.PushVideoFrame(externalVideoFrame);
            //Debug.Log("PushVideoFrame ret = " + ret + "time: " + System.DateTime.Now.Millisecond);
        }
    }

    /// <summary>
    /// /////
    /// </summary>
    /// <returns></returns>
    private bool CheckAppId()
    {
        Log = new Logger(LogText);
        return Log.DebugAssert(_appID.Length > 10, "Please fill in your appId in API-Example/profile/appIdInput.asset");
    }

    //Show data in AgoraBasicProfile
    [ContextMenu("ShowAgoraBasicProfileData")]
    private void LoadAssetData()
    {
        if (_appIdInput == null) return;
        _appID = _appIdInput.appID;
        _token = _appIdInput.token;
        _channelName = _appIdInput.channelName;
    }

    


    private void ScreenShareLeaveChannel()
    {
        RtcEngine.LeaveChannelEx(new RtcConnection(_channelName, Uid2));
    }

    private void UpdateChannelMediaOptions()
    {
        ChannelMediaOptions options = new ChannelMediaOptions();
        options.autoSubscribeAudio.SetValue(false);
        options.autoSubscribeVideo.SetValue(false);

        options.publishCameraTrack.SetValue(false);
        options.publishScreenTrack.SetValue(true);

#if UNITY_ANDROID || UNITY_IPHONE
            options.publishScreenCaptureAudio.SetValue(true);
            options.publishScreenCaptureVideo.SetValue(true);
#endif

        options.clientRoleType.SetValue(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);

        var ret = RtcEngine.UpdateChannelMediaOptions(options);
        Debug.Log("UpdateChannelMediaOptions returns: " + ret);
    }

    private void InitEngine()
    {
        RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngineEx();
        UserEventHandler handler = new UserEventHandler(this);
        RtcEngineContext context = new RtcEngineContext(_appID, 0,
                                    CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING,
                                    AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT);
        RtcEngine.Initialize(context);
        RtcEngine.InitEventHandler(new UserEventHandler(this));


    }

    public void voiceControl(bool flag)
    {
        RtcEngine.SetDefaultAudioRouteToSpeakerphone(flag); // Disables the default audio route.
        RtcEngine.SetEnableSpeakerphone(flag);
    }

    private void OnDestroy()
    {
        Debug.Log("OnDestroy");
        if (RtcEngine == null) return;
        RtcEngine.InitEventHandler(null);
        RtcEngine.LeaveChannel();
        RtcEngine.Dispose();
    }

    internal string GetChannelName()
    {
        return _channelName;
    }

    #region -- Video Render UI Logic ---

    internal static void DestroyVideoView(string name)
    {
        var go = GameObject.Find(name);
        if (!ReferenceEquals(go, null))
        {
            Destroy(go);
        }
    }
    public uint remoteUid;
    public List<GameObject> FriendList;
    public int count = 1;
    public List<uint> idList;
    public int userCount;
    public List<VideoSurface> testList = new List<VideoSurface>();
    #endregion

    internal class UserEventHandler : IRtcEngineEventHandler
    {
        private readonly ScreenShareWhileVideoCall _videoSample;

        internal UserEventHandler(ScreenShareWhileVideoCall videoSample)
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

            if (_videoSample.idList.Contains(uid))
            {
                return;
            }
            else
            {
                _videoSample.idList.Add(uid);

            }

            if (uid != _videoSample.Uid1 && uid != _videoSample.Uid2)
            {

                if (_videoSample.playerdict.ContainsKey((int)uid))
                {
                    Debug.LogError("asdfasdfasdfasdfasdf" + _videoSample.playerdict[(int)uid]);
                    GameObject player = GameObject.Find(_videoSample.playerdict[(int)uid] + "(user)");
                    PlayerItem playerScript = player.GetComponent<PlayerItem>();
                    if (playerScript.webviewStart == true)
                    {
                        //GameObject newFriendCam = Instantiate(Resources.Load<GameObject>("Prefabs/FriendCam"));
                        VideoSurface vs = player.transform.GetChild(1).GetChild(0).GetComponent<VideoSurface>();
                        //newFriendCam.transform.SetParent(player.transform.GetChild(1));
                        //newFriendCam.transform.position = new Vector2(0,0);
                        //newFriendCam.transform.localScale = new Vector3(1, 1, 1);
                        //FriendCamList.Add(newFriendCam.GetComponent<VideoSurface>());
                        vs.SetEnable(true);
                        vs.SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);

                        _videoSample.remoteUid = uid;
                        //_videoSample.FriendList[Math.Min(_videoSample.count, _videoSample.FriendList.Count - 1)].SetActive(true);
                        _videoSample.count += 1;
                        _videoSample.idList.Add(uid);
                    }
                    else
                    {
                        _videoSample.userCount = FriendCamList.Count();

                        GameObject newFriendCam = Instantiate(Resources.Load<GameObject>("Prefabs/FriendCam"));
                        newFriendCam.transform.SetParent(_videoSample.FriendCams.transform);
                        newFriendCam.transform.localScale = new Vector3(1, 1, 1);

                        if (!FriendCamList.Contains(newFriendCam.GetComponent<VideoSurface>()))
                        {
                            FriendCamList.Add(newFriendCam.GetComponent<VideoSurface>());
                            _videoSample.testList.Add(newFriendCam.GetComponent<VideoSurface>());
                            FriendCamList[FriendCamList.Count - 1].SetEnable(true);
                            FriendCamList[FriendCamList.Count - 1].SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
                            _videoSample.testList.Add(newFriendCam.GetComponent<VideoSurface>());
                            Debug.LogError(_videoSample.testList[0]);

                        }

                        _videoSample.remoteUid = uid;
                        _videoSample.FriendList[Math.Min(_videoSample.count, _videoSample.FriendList.Count - 1)].SetActive(true);
                        _videoSample.count += 1;
                        _videoSample.idList.Add(uid);

                    }
                }
                else
                {
                    _videoSample.userCount = FriendCamList.Count();

                    GameObject newFriendCam = Instantiate(Resources.Load<GameObject>("Prefabs/FriendCam"));
                    newFriendCam.transform.SetParent(_videoSample.FriendCams.transform);
                    newFriendCam.transform.localScale = new Vector3(1, 1, 1);

                    _videoSample.testList.Add(newFriendCam.GetComponent<VideoSurface>());
                    if (!FriendCamList.Contains(newFriendCam.GetComponent<VideoSurface>()))
                    {
                        FriendCamList.Add(newFriendCam.GetComponent<VideoSurface>());
                        FriendCamList[FriendCamList.Count - 1].SetEnable(true);
                        FriendCamList[FriendCamList.Count - 1].SetForUser(uid, connection.channelId,
                            VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
                    }

                    _videoSample.remoteUid = uid;
                    _videoSample.FriendList[Math.Min(_videoSample.count, _videoSample.FriendList.Count - 1)].SetActive(true);
                    _videoSample.count += 1;
                    _videoSample.idList.Add(uid);
                }

            }

        }

        // This callback is triggered when a remote user leaves the channel or drops offline.
        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {

            foreach (VideoSurface RemoteView in FriendCamList)
            {
                if (RemoteView.Uid == uid)
                {
                    RemoteView.SetEnable(false);
                    Destroy(RemoteView.gameObject);
                    FriendCamList.Remove(RemoteView);
                    Debug.Log("RemoteView.UserName " + RemoteView.UserName);
                }
            }
        }


    }


    void OnApplicationQuit()
    {
        if (RtcEngine != null)
        {
            //Leave();
            RtcEngine.Dispose();
            RtcEngine = null;
        }
    }
}

#region -- Agora Event ---



#endregion