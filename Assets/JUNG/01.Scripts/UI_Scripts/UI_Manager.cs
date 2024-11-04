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

    [SerializeField] private Image popUpUI;
    [SerializeField] private Image noTouchUI;
    [SerializeField] private Image sliderUI;

    #endregion

    public void PopUpUI()
    {

        popUpUI.gameObject.SetActive(true);
    }

    public void KillUI()
    {
        popUpUI.gameObject.SetActive(false);
    }

}
