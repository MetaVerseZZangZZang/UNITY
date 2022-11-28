using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AIGraph : MonoBehaviour
{
    public string url;
    // Start is called before the first frame update

    public void UI_AIGraphClick()
    {
        Application.OpenURL(url);
    }
}
