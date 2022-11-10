using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChatInputField : MonoBehaviour
{
    public static UI_ChatInputField Instance;
    
    public TextMeshProUGUI myName;
    public TMP_InputField  myInputField;
    public TMP_InputField  replyInputField;
    public string id;

    public void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        myName.text = UI_StartPanel.Instance.userName;
    }

    // Start is called before the first frame update
    public void sendChatBtn()
    {
        if (myInputField.text.Length > 0)
        {
            InputFieldText myinput = new InputFieldText();
            //myinput.nickname = Server.Instance.sid;
            myinput.context = myInputField.text;

            string json = JsonUtility.ToJson(myinput);
            Server.Instance.InputFieldEmit(json);

        }
        
        myInputField.text = "";
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
            Server.Instance.InputFieldEmit(json);
            Debug.Log(json);
        }
        
        replyInputField.text = "";
    }
    

}
