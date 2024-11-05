using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
public class UI_Manager : MonoBehaviour
{
    #region Single
    private static UI_Manager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static UI_Manager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    #region GettingUI
    [SerializeField] private GameObject charectorSelectUI;
    [SerializeField] private GameObject settingUI;
    [SerializeField] private Image noTouchUI;
    [SerializeField] private Image sliderUI;
    #endregion

    private GameObject isOpenObj;
    private int openCount = 0;
    private bool isTweening = false;


    public void StartButton()
    {
        openCount++;
        UIOpenOrClose(charectorSelectUI, true);
    }
    public void SettingButton()
    {
        openCount++;
        UIOpenOrClose(settingUI, true);
    }

    public void CloseCurrentPanel()
    {
        openCount--;
        UIOpenOrClose(isOpenObj, false);
    }



    public void UIOpenOrClose(GameObject ui_obj, bool isActive)
    {
        isOpenObj = ui_obj;
        isTweening = true;
        Sequence sq = DOTween.Sequence();
        sq.AppendCallback(() => noTouchUI.gameObject.SetActive(true));
        sq.Append(sliderUI.rectTransform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutExpo));
        sq.AppendCallback(() => ui_obj.SetActive(isActive));
        sq.AppendInterval(1f);
        sq.Append(sliderUI.rectTransform.DOLocalMoveY(-1080, 0.5f).SetEase(Ease.OutExpo));
        sq.AppendCallback(() =>
        {
            noTouchUI.gameObject.SetActive(false);
            sliderUI.rectTransform.localPosition = new Vector3(0, 1080);
            isTweening = false;
        });
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isTweening == false && openCount > 0)
            CloseCurrentPanel();
    }
    public void GameExit() => Application.Quit();
}
