using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CallManager : MonoBehaviour
{
    public InputField userName;
    
    public void Login()
    {
        if (string.IsNullOrEmpty(userName.text))
        {
//            MessageManager.Instance.ShowMessage("please input username!");
            return;
        }
 //       ChatManager.Instance.Login(userName.text);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Login();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
