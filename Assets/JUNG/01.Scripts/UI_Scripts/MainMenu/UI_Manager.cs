using DG.Tweening;
using UnityEngine;
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
            //DontDestroyOnLoad(gameObject);
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


    private bool isTweening = false;


    public void StartButton()
    {
        UIOpenOrClose(charectorSelectUI, true, null);
    }
    public void SettingButton()
    {
        UIOpenOrClose(settingUI, true, null);
    }

    public void UIOpenOrClose(GameObject ui_obj, bool isActive, GameObject ui_closeObj)
    {
        if (isTweening)
            return;
        isTweening = true;
        Sequence sq = DOTween.Sequence();
        sq.AppendCallback(() => noTouchUI.gameObject.SetActive(true));
        sq.Append(sliderUI.rectTransform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutExpo));
        sq.AppendCallback(() =>
        {
            ui_obj.SetActive(isActive);
            if (ui_closeObj != null)
            {
                ui_closeObj.SetActive(false);
            }
        });
        sq.AppendInterval(0.2f);
        sq.Append(sliderUI.rectTransform.DOLocalMoveY(-1080, 0.5f).SetEase(Ease.OutExpo));
        sq.AppendCallback(() =>
        {
            noTouchUI.gameObject.SetActive(false);
            sliderUI.rectTransform.localPosition = new Vector3(0, 1080);
            isTweening = false;
        });
    }

    public void GameExit() => Application.Quit();
}
