using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
            //myinput.nickname = Server.Instance.sid;
            myinput.context = myInputField.text;

            string json = JsonUtility.ToJson(myinput);
            Server.Instance.InputFieldEmit(json);

        }
        
        GetComponent<TMP_InputField>().text = "";
    }
}
