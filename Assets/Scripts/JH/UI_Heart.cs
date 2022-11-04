using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Heart : MonoBehaviour
{

    public Sprite Heart_filled;
    public Sprite Heart_unfilled;

    string myid;
    
    public void HeartClick(int num)
    {
        for (int i = 0; i < num; ++i)
        {
            this.transform.GetChild(i).GetComponent<Image>().sprite = Heart_filled;
        }

        for (int i = num; i < this.transform.childCount; ++i)
        {
            this.transform.GetChild(i).GetComponent<Image>().sprite = Heart_unfilled;
        }

        GameObject chatPlayer = this.transform.parent.gameObject;

        var texts = chatPlayer.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (var txtComponent in texts)
        {

            if (txtComponent.name == "IdText")
            {
                myid = txtComponent.text;
            }
        }
        Debug.Log(myid);
        string text = "id: "+myid.ToString() + " score: " + num.ToString();
        Server.Instance.HeartEmit(text);

    }
}
