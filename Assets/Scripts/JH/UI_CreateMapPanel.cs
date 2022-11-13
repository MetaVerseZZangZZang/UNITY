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
    public Toggle map_2;
    public Toggle map_3;
    
    public int mapNum;

    
    private void Awake()
    {
        Instance = this;
        Hide();
    }
    
    // Start is called before the first frame update
    public void Hide()
    {
        this.gameObject.SetActive(false);
        map_1.graphic.transform.GetChild(0).gameObject.SetActive(false);
        map_2.graphic.transform.GetChild(0).gameObject.SetActive(false);
        map_3.graphic.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    
    public void onRadio()
    {
        if (map_0.isOn)
        {
            mapNum = 0;
            map_0.graphic.transform.GetChild(0).gameObject.SetActive(true);
            map_1.graphic.transform.GetChild(0).gameObject.SetActive(false);
            map_2.graphic.transform.GetChild(0).gameObject.SetActive(false);
            map_3.graphic.transform.GetChild(0).gameObject.SetActive(false);
        }
        else if (map_1.isOn)
        {
            mapNum = 1;
            map_0.graphic.transform.GetChild(0).gameObject.SetActive(false);
            map_1.graphic.transform.GetChild(0).gameObject.SetActive(true);
            map_2.graphic.transform.GetChild(0).gameObject.SetActive(false);
            map_3.graphic.transform.GetChild(0).gameObject.SetActive(false);
        }
        else if (map_2.isOn)
        {
            mapNum = 2;
            map_0.graphic.transform.GetChild(0).gameObject.SetActive(false);
            map_1.graphic.transform.GetChild(0).gameObject.SetActive(false);
            map_2.graphic.transform.GetChild(0).gameObject.SetActive(true);
            map_3.graphic.transform.GetChild(0).gameObject.SetActive(false);
        }
        else if (map_3.isOn)
        {
            mapNum = 3;
            map_0.graphic.transform.GetChild(0).gameObject.SetActive(false);
            map_1.graphic.transform.GetChild(0).gameObject.SetActive(false);
            map_2.graphic.transform.GetChild(0).gameObject.SetActive(false);
            map_3.graphic.transform.GetChild(0).gameObject.SetActive(true);
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
