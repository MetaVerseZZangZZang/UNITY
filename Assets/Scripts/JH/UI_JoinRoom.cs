using System.Collections;
using System.Collections.Generic;
using Photon.Pun.Demo.Cockpit;
using UnityEngine;

public class UI_JoinRoom : MonoBehaviour
{

    public static UI_JoinRoom Instance;

    public UI_JoinRoomItem Prefab;
    public Transform ParentTransform;
    public List<UI_JoinRoomItem> Items;
    
    private void Awake()
    {
        Instance = this;
        Hide();
    }    
    
    public void Refresh()
    {
        var dataList = NetManager.Instance.m_roomList;
        int dataCount = dataList.Count;

        int itemCount = Items.Count;

        if (dataCount == 0)
        {
            for (int i = 0; i < itemCount; ++i)
            {
                Items[i].gameObject.SetActive(false);
            }

            return;
        }

        if (itemCount < dataCount)
        {
            for (int i = 0; i < dataCount - itemCount; ++i)
            {
                var newItem = Instantiate<UI_JoinRoomItem>(Prefab);
                newItem.transform.SetParent(ParentTransform);
                newItem.GetComponent<RectTransform>().localScale = Vector3.one;

                Items.Add(newItem);
            }

            itemCount = Items.Count;
        }

        for (int i = 0; i < itemCount; ++i)
        {
            if (i >= dataCount)
            {
                Items[i].gameObject.SetActive(false);
                continue;
            }

            Items[i].gameObject.SetActive(true);
            Items[i].Init(dataList[i]);
        }

    }
    
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }
    
    

}
