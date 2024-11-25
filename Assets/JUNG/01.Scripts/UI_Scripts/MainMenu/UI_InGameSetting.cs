using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_InGameSetting : MonoBehaviour
{
    [SerializeField] private PlayerControlSO escapeSO;
    [SerializeField] private GameObject baseSettingObj;
    [SerializeField] private GameObject keyboardSettingObj;
    private bool IsDoingOpen = false;
    private bool IsPop = false;
    void Start()
    {
        escapeSO.CloseUIEvent += HandleOpenOrCloseStop;
    }

    private void HandleOpenOrCloseStop()
    {
        if (IsPop == true && IsDoingOpen == false)
        {
            Time.timeScale = 1;
            IsDoingOpen = true;
            baseSettingObj.transform.DOLocalMoveY(1080f, 0.5f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                IsDoingOpen = false;
                IsPop = false;
            });
        }
        else if (IsPop == false && IsDoingOpen == false)
        {
            IsDoingOpen = true;
            baseSettingObj.transform.DOLocalMoveY(0f, 0.5f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                IsDoingOpen = false;
                IsPop = true;
                Time.timeScale = 0;
            });

        }
    }

    

    private void OnDestroy()
    {
        escapeSO.CloseUIEvent -= HandleOpenOrCloseStop;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }
}
