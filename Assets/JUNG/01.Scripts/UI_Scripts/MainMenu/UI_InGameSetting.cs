using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_InGameSetting : MonoBehaviour
{
    [SerializeField] private PlayerControlSO escapeSO;
    private GameObject childObj;
    private bool IsDoingOpen = false;
    private bool IsPop = false;
    void Start()
    {
        childObj = GetComponentInChildren<GameObject>();
        escapeSO.CloseUIEvent += HandleOpenOrCloseStop;
    }

    private void HandleOpenOrCloseStop()
    {
        if (IsPop == true && IsDoingOpen == false)
        {
            IsDoingOpen = true;
            childObj.transform.DOMoveY(1080f, 0.3f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                IsDoingOpen = false;
                IsPop = false;
                Time.timeScale = 0;
            });
        }
        else if (IsPop == false && IsDoingOpen == false)
        {
            IsDoingOpen = true;
            childObj.transform.DOMoveY(0f, 0.3f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                IsDoingOpen = false;
                IsPop = true;
                Time.timeScale = 1;
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
