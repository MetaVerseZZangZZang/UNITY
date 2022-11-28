using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class UI_AIImage : MonoBehaviour
{

    public string url;
    // Start is called before the first frame update

    public void UI_AIImageClick()
    {
        Application.OpenURL(url);
    }
}
