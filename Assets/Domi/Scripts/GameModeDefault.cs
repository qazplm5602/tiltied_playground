using System;
using System.Collections;
using UnityEngine;

public class GameModeDefault : GameMode
{
    bool progress = false; // 경기 진행중
    BallGoalSimulateManager simulateManager;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    public override void GameStart()
    {
        soccerBall.BallReset();
        progress = true;
    }

    protected override void HandleBallGoal(BallAreaType type)
    {
        if (!progress) return;

        StartCoroutine(WaitBallReset());
    }

    protected override void HandleBallOut()
    {
        if (!progress) return;

        StartCoroutine(WaitBallReset());
    }

    IEnumerator WaitBallReset() {
        progress = false; // 진행 중단 ㄱㄱㄱㄱ

        yield return new WaitForSeconds(5f);

        soccerBall.BallReset();
        CameraManager.Instance.Transition.FadeChangeCam(CameraType.Main);

        progress = true;
    }
}
