using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatPanel : MonoBehaviour
{
    public Text Text;
    
    
    public static ChatPanel Instance;

    private void Awake()
    {
        Instance = this;
    }
}
