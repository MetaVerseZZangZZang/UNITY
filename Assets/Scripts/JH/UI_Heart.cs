using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Heart : MonoBehaviour
{
    public ChatPlayer m_ChatPlayer;
    public Sprite Heart_filled;
    public Sprite Heart_unfilled;
    
    
    public void HeartClick(int num)
    {
        if (m_ChatPlayer.heart == num)
        {
            for (int i = 0; i < this.transform.childCount; ++i)
            {
                this.transform.GetChild(i).GetComponent<Image>().sprite = Heart_unfilled;
                
            }
            m_ChatPlayer.heart = 0;
        }

        else
        {
            for (int i = 0; i < num; ++i)
            {
                this.transform.GetChild(i).GetComponent<Image>().sprite = Heart_filled;
            }

            if (num > 0)
            {
                this.transform.GetChild(num - 1).GetComponent<Animation>().Play();
            }

            for (int i = num; i < this.transform.childCount; ++i)
            {
                this.transform.GetChild(i).GetComponent<Image>().sprite = Heart_unfilled;
            }
            m_ChatPlayer.heart = num;
        }

        string json = JsonUtility.ToJson(m_ChatPlayer);
        print(json);
        Server.Instance.HeartEmit(json);
    }

}
