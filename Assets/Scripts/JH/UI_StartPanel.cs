using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StartPanel : MonoBehaviour
{
    public static UI_StartPanel Instance;
    public GameObject AlarmPopup;
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
        userName = nameInput.text;
        if (userName == "")
            StartCoroutine("FadeIn");
        else
        {
            Hide();
            UI_LobbyPanel.Instance.Show();
            PhotonManager.Instance.Connect();
        }

    }

    IEnumerator FadeIn()
    {
        for (float ff = 0.0f; ff <= 1.0f;)
        {
            ff += 0.2f;
            AlarmPopup.GetComponent<Image>().color = new Color(0.83f, 0.83f, 0.83f, ff);
            AlarmPopup.transform.GetChild(0).GetComponent<Text>().color = new Color(0.18f, 0.18f, 0.18f, ff);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine("FadeOut");
    }

    IEnumerator FadeOut()
    {
        AlarmPopup.SetActive(true);
        for (float ff = 1.0f; ff >= 0.0f;)
        {
            ff -= 0.2f;
            AlarmPopup.GetComponent<Image>().color = new Color(0.83f, 0.83f, 0.83f, ff);
            AlarmPopup.transform.GetChild(0).GetComponent<Text>().color = new Color(0.18f, 0.18f, 0.18f, ff);
            yield return new WaitForSeconds(0.1f);
        }


    }
}
