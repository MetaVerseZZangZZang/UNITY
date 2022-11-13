using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using TMPro;
using UnityEngine;

public class UI_ReplyInputField : MonoBehaviour
{
    public static UI_ReplyInputField Instance;
    private TMP_InputField replyInputField;
    public string id;
    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        replyInputField = GetComponent<TMP_InputField>();
        Hide();
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    public void sendReplyBtn()
    {
        if (replyInputField.text.Length > 0)
        {
            ReplyInputField myinput = new ReplyInputField();
            //myinput.nickname = Server.Instance.sid;
            myinput.context = replyInputField.text;
            myinput.id = id;
            
            string json = JsonUtility.ToJson(myinput);
            Server.Instance.ReplyFieldEmit(json);
            Debug.Log(json);
        }
        
        replyInputField.text = "";
        UI_ChatInputField.Instance.Show();
        Hide();
    }
}
