using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.Json.Nodes;
using UnityEngine.UI;

public class ReceiveKeyword : MonoBehaviour
{
    public GameObject m_KeywordTextPrefab;
    public GameObject keyWordTextParent;



    public static ReceiveKeyword Instance;


    //public Text keyword1;
    //public Text keyword2;


    //public Text recommendkeyword1;
    //public Text recommendKeyword2;


    private void Awake()
    {
        Instance = this;
    }


    public void ReceiveJson(string key1, string key2, string recomkey1, string recomkey2)
    {
        //keyword1.text = key1;
        //keyword2.text = key2;

        //recommendkeyword1.text = recomkey1;
        //recommendKeyword2.text = recomkey2;

    }


    public void InstantiateKeywordText(string msg)
    {
       
    }


}
