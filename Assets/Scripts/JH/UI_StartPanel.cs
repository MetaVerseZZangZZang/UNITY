using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using UnityEngine.UI;

public class UI_StartPanel : MonoBehaviour
{
    public static UI_StartPanel Instance;

    public InputField nameInput;
    public string userName;

    public GameObject AlarmPopup;
    
    public float animTime = 2f;         // Fade 애니메이션 재생 시간 (단위:초).  
    private Image fadeImage;            // UGUI의 Image컴포넌트 참조 변수.  

    private float start = 1f;           // Mathf.Lerp 메소드의 첫번째 값.  
    private float end = 0f;             // Mathf.Lerp 메소드의 두번째 값.  
    private float time = 0f;   
    
    private bool isPlaying = false;  
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
        {
            if (isPlaying == true)  
                return;  
   
            StartCoroutine("FadeIn");  
        }

        else
        {
            Hide();
            PhotonManager.Instance.Connect();
            UI_CharPanel.Instance.Show();
        }

    }
    IEnumerator FadeIn() {
        for (float ff = 0.0f; ff <= 1.0f;) {
            ff += 0.2f;
            AlarmPopup.GetComponent<Image>().color = new Color(0.83f, 0.83f ,0.83f, ff);
            AlarmPopup.transform.GetChild(0).GetComponent<Text>().color = new Color(0.18f, 0.18f, 0.18f, ff);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine("FadeOut");  
    }
 
//페이드 인
    public IEnumerator FadeOut()
    {
        AlarmPopup.SetActive(true);
        for (float ff = 1.0f; ff >= 0.0f;) {
            ff -= 0.2f;
            AlarmPopup.GetComponent<Image>().color = new Color(0.83f, 0.83f, 0.83f, ff);
            AlarmPopup.transform.GetChild(0).GetComponent<Text>().color = new Color(0.18f, 0.18f, 0.18f, ff);
            yield return new WaitForSeconds(0.1f);
        }
        
    }


}
