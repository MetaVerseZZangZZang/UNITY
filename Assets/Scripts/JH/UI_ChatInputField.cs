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
    private TMP_InputField  myInputField;

    public void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        myName.text = UI_StartPanel.Instance.userName;
        myInputField = GetComponent<TMP_InputField>();
        Show();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    public void Show()
    {
        gameObject.SetActive(true);
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
}
