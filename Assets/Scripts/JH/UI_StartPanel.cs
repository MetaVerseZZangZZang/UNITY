using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StartPanel : MonoBehaviour
{
    public static UI_StartPanel Instance;

    public InputField nameInput;
    public string userName;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        Show();
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }
    
    public void StartBtn()
    {
        Hide();
        NetManager.Instance.Connect();
        UI_CharPanel.Instance.Show();
        userName = nameInput.text;
    }
}
