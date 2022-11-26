    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class UI_CreateMapPanel : MonoBehaviour
{
    public static UI_CreateMapPanel Instance;

    public InputField RoomNameInputField;

    public Toggle map_0;
    public Toggle map_1;

    
    public int mapNum;

    
    private void Awake()
    {
        Instance = this;
        Hide();
    }
    
    // Start is called before the first frame update
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        map_0.isOn = true;
        map_1.isOn = false;
        
        map_0.graphic.transform.GetChild(0).gameObject.SetActive(true);
        map_1.graphic.transform.GetChild(0).gameObject.SetActive(false);
        
        RoomNameInputField.text = "";
        gameObject.SetActive(true);
    }

    
    public void onRadio()
    {
        if (map_0.isOn)
        {
            mapNum = 0;
            map_0.graphic.transform.GetChild(0).gameObject.SetActive(true);
            map_1.graphic.transform.GetChild(0).gameObject.SetActive(false);
        }
        else if (map_1.isOn)
        {
            mapNum = 1;
            map_0.graphic.transform.GetChild(0).gameObject.SetActive(false);
            map_1.graphic.transform.GetChild(0).gameObject.SetActive(true);
        }

    }
    
    public void CreateRoom()
    {
        PhotonManager.Instance.CreateRoom();
        Hide();
        UI_LobbyPanel.Instance.Hide();
        //UI_MainPanel.Instance.Show();
    }
}
