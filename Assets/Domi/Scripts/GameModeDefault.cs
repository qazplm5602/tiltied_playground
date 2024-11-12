using System;
using System.Collections;
using UnityEngine;

public class GameModeDefault : GameMode, IGameModeTimer
{
    public GameModeUI IngameUI { get; protected set; }
    SoccerTimer IGameModeTimer.Timer { get => timer; }
    private SoccerTimer timer;

    private BallGoalSimulateManager simulateManager;

    protected override void Awake()
    {
        base.Awake();
        timer = new(this); // 타이머가 먼저임
        IngameUI = new(this);
    }

    private void Update() {
        timer.Loop();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    public override void GameStart()
    {
        soccerBall.BallReset();
        IsPlay = true;

        // 시간
        timer.SetTime(60 * 90);
        timer.Play();
    }

    protected override void HandleBallGoal(BallAreaType type)
    {
        if (type == BallAreaType.Blue)
            BlueScore ++;
        else
            RedScore ++;

        StartCoroutine(WaitBallReset());
    }

    protected override void HandleBallOut()
    {
        StartCoroutine(WaitBallReset());
    }

    IEnumerator WaitBallReset() {
        IsPlay = false; // 진행 중단 ㄱㄱㄱㄱ

        yield return new WaitForSeconds(5f);

        CameraManager.Instance.Transition.FadeChangeCamNoLive(CameraType.Main, () => {
            soccerBall.BallReset();
            IsPlay = true;
        });
    }
}
