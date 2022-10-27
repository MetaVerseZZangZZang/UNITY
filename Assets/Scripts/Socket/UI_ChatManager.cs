using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChatManager : MonoBehaviour
{

    public static UI_ChatManager Instance; 
    public Text[] chatText;


    private void Awake()
    {
        Instance = this;
    }


    public void ChatRPC(string msg)
    {
        print("enter");
        bool isInput = false;
        for (int i = 0; i < chatText.Length; i++)
            if (chatText[i].text == "")
            {
                isInput = true;
                chatText[i].text = msg;

                break;
            }
        if (!isInput) // 꽉차면 한칸씩 위로 올림
        {
            for (int i = 1; i < chatText.Length; i++) chatText[i - 1].text = chatText[i].text;
            chatText[chatText.Length - 1].text = msg;
        }
    }
}
