using System.Collections;

using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class MainManager : MonoBehaviour
{
    

    // 싱글턴

    public static MainManager instance {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<MainManager>();
            }

            return m_instance;
        }
    
    }
    public static MainManager m_instance;


    // 스캔 전체 창
    public GameObject scanFullFrameUI;
    private Animator scanFullFrameAnimator;

    // 로딩 창
    public GameObject loadingUI;
    private Animator loadingAnimator;

    // 알림 창
    public GameObject noticeMessageUIObject;
    private Animator noticeMessageAnimator;
    public TextMeshProUGUI noticeMessageText;

    // 이미지 트래킹 서비스
    public ImageTrackingService ImageTrackingService;

    private void Awake()
    {

        scanFullFrameAnimator = scanFullFrameUI.GetComponent<Animator>();
        loadingAnimator = loadingUI.GetComponent<Animator>();
        noticeMessageAnimator = noticeMessageUIObject.GetComponent<Animator>();
       

        scanFullFrameUI.SetActive(false);
        loadingUI.SetActive(false);
        noticeMessageUIObject.SetActive(false);
    }

    IEnumerator Start()
    {
        loadingUI.SetActive(true);
        yield return new WaitForSeconds(1f);
        loadingUI.SetActive(false);
        scanFullFrameUI.SetActive(true);
    }




    // 스캔 성공시 뜸
    public void AppearNoticeMessage(string message)
    {
        noticeMessageText.text = message;
        if (noticeMessageUIObject.activeSelf == false) noticeMessageUIObject.SetActive(true);
        else noticeMessageAnimator.SetTrigger("windowIn");
    }

    public void DisAppearNoticeMessage()
    {
        noticeMessageAnimator.SetTrigger("windowOut");
    }

    public void GotoMapScene()
    {
        SceneManager.LoadScene("MapScene");
    }


}
