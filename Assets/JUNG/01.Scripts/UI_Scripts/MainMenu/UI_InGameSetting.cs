using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_InGameSetting : MonoBehaviour
{
    private PlayerControlSO escapeSO;
    private GameObject childObj;
    private bool IsOpenChildObj = false;
    private bool IsDoingOpen = false;
    void Start()
    {
        childObj = GetComponentInChildren<GameObject>();
        escapeSO.CloseUIEvent += HandleOpenOrCloseStop;
    }

    private void HandleOpenOrCloseStop()
    {

        if (IsOpenChildObj && IsDoingOpen == false)
        {
            IsDoingOpen = true;
            childObj.SetActive(true);
            childObj.transform.DOMoveY(0f, 0.3f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                IsOpenChildObj = false;

            });

        }
        else if (IsOpenChildObj == false && IsDoingOpen == false)
        {
            childObj.SetActive(true);
            childObj.transform.DOLocalMoveY(1080f, 0.3f).SetEase(Ease.OutExpo).OnComplete(() =>
            {
                IsOpenChildObj = true;
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
