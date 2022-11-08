using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ChatInputField : MonoBehaviour
{
    
    public TextMeshProUGUI myName;
    public TextMeshProUGUI myInputField;

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
            myinput.nickname = myName.text;
            myinput.context = myInputField.text;

            string json = JsonUtility.ToJson(myinput);
            Server.Instance.InputFieldEmit(json);
        }
    }
}
