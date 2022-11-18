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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agora.Rtc;

//namespace Agora_RTC_Plugin.API_Example.Examples.Advanced.ScreenShareWhileVideoCall

public class ScreenShareWhileVideoCall : MonoBehaviour
{

    public static ScreenShareWhileVideoCall Instance;

    private Texture2D _texture;
    private Rect _rect;
    private int i = 0;
    private WebCamTexture _webCameraTexture;
    //public RawImage RawImage;
    public Vector2 CameraSize = new Vector2(1280, 960);
    public int CameraFPS = 15;
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

    public uint Uid1=123;
    public uint Uid2=345;

    public static List<VideoSurface> FriendCamList = new List<VideoSurface>();

    public GameObject FriendCams;
    public GameObject myCam;
    internal VideoSurface LocalView;

    public bool startWebview = false;


    public Dictionary<uint, string> playerdict = new Dictionary<uint, string>();
    public List<uint> aig = new List<uint>();

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
        UI_PlayerSlot.Instance.DelPlayerSlot(UI_StartPanel.Instance.name);
        FriendCamList.Clear();
    }
    
    // Use this for initialization
    public void AgoraStart()
    {
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
        JoinChannel();
    }

    
    public void WebviewStart()
    {
        LoadAssetData();
        Safari.SetActive(true);
        if (CheckAppId())
        {
            Debug.LogError(Uid2 + "(user)");

            GameObject playerID = GameObject.Find(UI_StartPanel.Instance.userName +"(user)");

            PlayerItem playerScript = playerID.GetComponent<PlayerItem>();

            playerScript.webviewStart = true;

            InitCameraDevice();
            InitTexture();
            InitEngine();
            SetExternalVideoSource();
            JoinChannel3();
            startWebview = true;


            Transform playerCanvas = playerID.transform.GetChild(0).GetChild(2);
            playerCanvas.gameObject.SetActive(true);
            RawImage playerWebImage = playerCanvas.GetComponent<RawImage>();
            StartCoroutine(BringWebTexture(playerWebImage));

            
        }
    }

    IEnumerator BringWebTexture(RawImage webImageTexture)
    {
        yield return new WaitForSeconds(0.5f);
        webImageTexture.texture = WebViewObject.Instance.tx.texture;

    }

    private void JoinChannel3()
    {
        RtcEngine.EnableAudio();
        RtcEngine.EnableVideo();
        RtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);

        ChannelMediaOptions options1 = new ChannelMediaOptions();
        options1.autoSubscribeAudio.SetValue(true);
        options1.autoSubscribeVideo.SetValue(true);

        options1.publishCameraTrack.SetValue(true);
        options1.publishScreenTrack.SetValue(false);
        options1.enableAudioRecordingOrPlayout.SetValue(true);
        options1.clientRoleType.SetValue(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        Uid1 = (uint)UnityEngine.Random.Range(1000,2000);
        
        var ret = RtcEngine.JoinChannelEx(_token, new RtcConnection(_channelName, Uid1), options1);
    }

    private void SetExternalVideoSource()
    {
        var ret = RtcEngine.SetExternalVideoSource(true, false, EXTERNAL_VIDEO_SOURCE_TYPE.VIDEO_FRAME, new SenderOptions());
        this.Log.UpdateLog("SetExternalVideoSource returns:" + ret);
    }

    private void InitCameraDevice()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        _webCameraTexture = new WebCamTexture(devices[0].name, (int)_rect.width, (int)_rect.height, CameraFPS);
        _webCameraTexture.Play();
    }
    private void InitTexture()
    {
        _rect = new Rect(0f, 0f, 1400f, 640f);
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
    public int moveX;
    public int moveY;
    public Rect rect;

    private IEnumerator ShareScreen()
    {
        yield return new WaitForEndOfFrame();
        IRtcEngine rtc = Agora.Rtc.RtcEngine.Instance;
        if (rtc != null)
        {

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

    private void JoinChannel()
    {
        RtcEngine.EnableAudio();
        RtcEngine.EnableVideo();
        RtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);

        
        ChannelMediaOptions options = new ChannelMediaOptions();
        options.autoSubscribeAudio.SetValue(true);
        options.autoSubscribeVideo.SetValue(true);

        options.publishCameraTrack.SetValue(true);
        options.publishScreenTrack.SetValue(false);
        options.enableAudioRecordingOrPlayout.SetValue(true);
        options.clientRoleType.SetValue(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);


        RtcEngine.JoinChannel(_token, _channelName, Uid2, options);

        myCam.AddComponent<VideoSurface>();
        LocalView = myCam.GetComponent<VideoSurface>();
        LocalView.SetForUser(0, "", VIDEO_SOURCE_TYPE.VIDEO_SOURCE_CAMERA);
        LocalView.SetEnable(true);
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

    //private void PrepareScreenCapture()
    //{
    //    _winIdSelect = GameObject.Find("winIdSelect").GetComponent<Dropdown>();

    //    if (_winIdSelect == null || RtcEngine == null) return;

    //    _winIdSelect.ClearOptions();

    //    SIZE t = new SIZE();
    //    t.width = 360;
    //    t.height = 240;
    //    SIZE s = new SIZE();
    //    s.width = 360;
    //    s.height = 240;
    //    var info = RtcEngine.GetScreenCaptureSources(t, s, true);

    //    _winIdSelect.AddOptions(info.Select(w =>
    //            new Dropdown.OptionData(
    //                string.Format("{0}: {1}-{2} | {3}", w.type, w.sourceName, w.sourceTitle, w.sourceId)))
    //        .ToList());
    //}

    //private void EnableUI()
    //{
    //    _startShareBtn = GameObject.Find("startShareBtn").GetComponent<Button>();
    //    _stopShareBtn = GameObject.Find("stopShareBtn").GetComponent<Button>();
    //    if (_startShareBtn != null) _startShareBtn.onClick.AddListener(OnStartShareBtnClick);
    //    if (_stopShareBtn != null)
    //    {
    //        _stopShareBtn.onClick.AddListener(OnStopShareBtnClick);
    //        _stopShareBtn.gameObject.SetActive(false);
    //    }
    //}

//    private void OnStartShareBtnClick()
//    {
//        if (RtcEngine == null) return;

//        if (_startShareBtn != null) _startShareBtn.gameObject.SetActive(false);
//        if (_stopShareBtn != null) _stopShareBtn.gameObject.SetActive(true);

//#if UNITY_ANDROID || UNITY_IPHONE
//            var parameters2 = new ScreenCaptureParameters2();
//            parameters2.captureAudio = true;
//            parameters2.captureVideo = true;
//            var nRet = RtcEngine.StartScreenCapture(parameters2);
//            this.Log.UpdateLog("StartScreenCapture :" + nRet);
//#else
//        RtcEngine.StopScreenCapture();
//        if (_winIdSelect == null) return;
//        var option = _winIdSelect.options[_winIdSelect.value].text;
//        if (string.IsNullOrEmpty(option)) return;

//        if (option.Contains("ScreenCaptureSourceType_Window"))
//        {
//            var windowId = option.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1];
//            Log.UpdateLog(string.Format(">>>>> Start sharing {0}", windowId));
//            var nRet = RtcEngine.StartScreenCaptureByWindowId(ulong.Parse(windowId), default(Rectangle),
//                    default(ScreenCaptureParameters));
//            this.Log.UpdateLog("StartScreenCaptureByWindowId:" + nRet);
//        }
//        else
//        {
//            var dispId = uint.Parse(option.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1]);
//            Log.UpdateLog(string.Format(">>>>> Start sharing display {0}", dispId));
//            var nRet = RtcEngine.StartScreenCaptureByDisplayId(dispId, default(Rectangle),
//                new ScreenCaptureParameters { captureMouseCursor = true, frameRate = 30 });
//            this.Log.UpdateLog("StartScreenCaptureByDisplayId:" + nRet);
//        }
//#endif

//        //ScreenShareJoinChannel();
//    }

    //private void OnStopShareBtnClick()
    //{
    //    ScreenShareLeaveChannel();
    //    if (_startShareBtn != null) _startShareBtn.gameObject.SetActive(true);
    //    if (_stopShareBtn != null) _stopShareBtn.gameObject.SetActive(false);
    //    RtcEngine.StopScreenCapture();
    //}

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

    internal static void MakeVideoView(uint uid, string channelId = "", VIDEO_SOURCE_TYPE videoSourceType = VIDEO_SOURCE_TYPE.VIDEO_SOURCE_CAMERA)
    {
        var go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null))
        {
            return; // reuse
        }

        // create a GameObject and assign to this new user
        VideoSurface videoSurface = new VideoSurface();

        if (videoSourceType == VIDEO_SOURCE_TYPE.VIDEO_SOURCE_CAMERA)
        {
            videoSurface = MakeImageSurface("MainCameraView");
        }
        else if (videoSourceType == VIDEO_SOURCE_TYPE.VIDEO_SOURCE_SCREEN)
        {
            videoSurface = MakeImageSurface("ScreenShareView");
        }
        else
        {
            videoSurface = MakeImageSurface(uid.ToString());
        }
        if (ReferenceEquals(videoSurface, null)) return;
        // configure videoSurface
        videoSurface.SetForUser(uid, channelId, videoSourceType);
        videoSurface.SetEnable(true);

        videoSurface.OnTextureSizeModify += (width, height) =>
        {
            float scale = (float)height / (float)width;
            videoSurface.transform.localScale = new Vector3(5, 5 * scale, 1);
            Debug.Log("OnTextureSizeModify: " + width + "  " + height);
        };

    }

    // VIDEO TYPE 1: 3D Object
    private static VideoSurface MakePlaneSurface(string goName)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Plane);

        if (go == null)
        {
            return null;
        }

        go.name = goName;
        // set up transform
        go.transform.Rotate(0f, 0.0f, 0.0f);
        go.transform.position = Vector3.zero;
        go.transform.localScale = new Vector3(1f, 1f, 1f);

        // configure videoSurface
        var videoSurface = go.AddComponent<VideoSurface>();
        return videoSurface;
    }

    // Video TYPE 2: RawImage
    private static VideoSurface MakeImageSurface(string goName)
    {
        var go = new GameObject();

        if (go == null)
        {
            return null;
        }

        go.name = goName;
        // to be renderered onto
        go.AddComponent<RawImage>();
        // make the object draggable
        go.AddComponent<UIElementDrag>();
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
        go.transform.Rotate(0f, 0.0f, 0f);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = new Vector3(3f, 4f, 1f);

        // configure videoSurface
        var videoSurface = go.AddComponent<VideoSurface>();
        return videoSurface;
    }

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
            
            if (uid != _videoSample.Uid1 && uid != _videoSample.Uid2 )
            {

                if (!_videoSample.playerdict.ContainsKey(uid))
                {
                    Debug.LogError("nulllllllllll");
                    _videoSample.userCount = FriendCamList.Count();

                    GameObject newFriendCam = Instantiate(Resources.Load<GameObject>("Prefabs/FriendCam"));
                    newFriendCam.transform.SetParent(_videoSample.FriendCams.transform);
                    newFriendCam.transform.localScale = new Vector3(1, 1, 1);


                    FriendCamList.Add(newFriendCam.GetComponent<VideoSurface>());
                    FriendCamList[FriendCamList.Count - 1].SetEnable(true);
                    FriendCamList[FriendCamList.Count - 1].SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);


                    _videoSample.remoteUid = uid;
                    _videoSample.FriendList[Math.Min(_videoSample.count, _videoSample.FriendList.Count - 1)].SetActive(true);
                    _videoSample.count += 1;
                    _videoSample.idList.Add(uid);
                }
                else if (_videoSample.playerdict.ContainsKey(uid))
                {
                    GameObject player = GameObject.Find(_videoSample.playerdict[uid]);
                    PlayerItem playerScript = player.GetComponent<PlayerItem>();

                    Debug.LogError(playerScript.webviewStart);
                    if (playerScript.webviewStart == true)
                    {
                        GameObject newFriendCam = Instantiate(Resources.Load<GameObject>("Prefabs/FriendCam"));
                        newFriendCam.transform.SetParent(player.transform.GetChild(0));
                        newFriendCam.transform.localScale = new Vector3(1, 1, 1);


                        FriendCamList.Add(newFriendCam.GetComponent<VideoSurface>());
                        FriendCamList[FriendCamList.Count - 1].SetEnable(true);
                        FriendCamList[FriendCamList.Count - 1].SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);


                        _videoSample.remoteUid = uid;
                        _videoSample.FriendList[Math.Min(_videoSample.count, _videoSample.FriendList.Count - 1)].SetActive(true);
                        _videoSample.count += 1;
                        _videoSample.idList.Add(uid);
                    }
                    else
                    {
                        //Debug.LogError("nulllllllllll");
                        _videoSample.userCount = FriendCamList.Count();

                        GameObject newFriendCam = Instantiate(Resources.Load<GameObject>("Prefabs/FriendCam"));
                        newFriendCam.transform.SetParent(_videoSample.FriendCams.transform);
                        newFriendCam.transform.localScale = new Vector3(1, 1, 1);


                        FriendCamList.Add(newFriendCam.GetComponent<VideoSurface>());
                        FriendCamList[FriendCamList.Count - 1].SetEnable(true);
                        FriendCamList[FriendCamList.Count - 1].SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);


                        _videoSample.remoteUid = uid;
                        _videoSample.FriendList[Math.Min(_videoSample.count, _videoSample.FriendList.Count - 1)].SetActive(true);
                        _videoSample.count += 1;
                        _videoSample.idList.Add(uid);
                    }
                }
                else
                {
                    //Debug.LogError("nulllllllllll");
                    _videoSample.userCount = FriendCamList.Count();

                    GameObject newFriendCam = Instantiate(Resources.Load<GameObject>("Prefabs/FriendCam"));
                    newFriendCam.transform.SetParent(_videoSample.FriendCams.transform);
                    newFriendCam.transform.localScale = new Vector3(1, 1, 1);


                    FriendCamList.Add(newFriendCam.GetComponent<VideoSurface>());
                    FriendCamList[FriendCamList.Count - 1].SetEnable(true);
                    FriendCamList[FriendCamList.Count - 1].SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);


                    _videoSample.remoteUid = uid;
                    _videoSample.FriendList[Math.Min(_videoSample.count, _videoSample.FriendList.Count - 1)].SetActive(true);
                    _videoSample.count += 1;
                    _videoSample.idList.Add(uid);
                }



                //Debug.LogError(_videoSample.Uid2 + "(user)");
                //PlayerItem playerScript = player.GetComponent<PlayerItem>();
                //Debug.LogError(player);


                //if (playerScript.webviewStart == true)
                //{
                //    Debug.Log("!!!!!!!!!!!!!!!!!!!!!"+playerScript.webviewStart);
                //}


                //PlayerItem playerScript = playerID.GetComponent<PlayerItem>();

                //if (playerScript.webviewStart == true)
                //{
                //    Debug.LogError("들어옴");
                //    GameObject canvas = playerID.transform.GetChild(0).GetComponent<GameObject>();
                //    GameObject newFriendCam = Instantiate(Resources.Load<GameObject>("Prefabs/FriendCam"));
                //    newFriendCam.transform.SetParent(canvas.transform);
                //}

                //else
                //{
                
                //}
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
                }
            }
        }

        //public override void OnRemoteVideoStateChanged(RtcConnection connection, uint remoteUid,
        //    REMOTE_VIDEO_STATE state, REMOTE_VIDEO_STATE_REASON reason, int elapsed)
        //{
        //    Debug.Log("uid: " + remoteUid);
        //    Debug.Log("state " + state);
            
        //    foreach (VideoSurface RemoteView in FriendCamList)
        //    {
        //        if (RemoteView.Uid == remoteUid)
        //        {
        //            if (state == 0)
        //            {
        //                UI_MainPanel.Instance.friendCamOff(RemoteView);
        //            }
        //            else if (state == REMOTE_VIDEO_STATE.REMOTE_VIDEO_STATE_STARTING)
        //            {
        //                UI_MainPanel.Instance.friendCamON(RemoteView);
        //            }
        //        }
        //    }

        //}

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