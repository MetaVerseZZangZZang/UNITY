using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatPlayerManager : MonoBehaviour
{
    public static ChatPlayerManager Instance;
    public List<ChatPlayer> ChatPlayersList = new List<ChatPlayer>();

    void Awake()
    {
        Instance = this;
    }

    public ChatPlayer findChatPlayerById(string id)
    {
        ChatPlayer cc = null;
        foreach(ChatPlayer cp in ChatPlayersList)
        {
            if (cp.id == id)
            {
                cc = cp;
            }
        }
        return cc;
    }
    
    
}
