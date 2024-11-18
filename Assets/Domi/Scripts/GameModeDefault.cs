using System;
using System.Collections;
using UnityEngine;

public class GameModeDefault : GameMode, IGameModeTimer, ICutsceneCallback
{
    public GameModeUI IngameUI { get; protected set; }
    SoccerTimer IGameModeTimer.Timer { get => timer; }
    private SoccerTimer timer;

    private BallGoalSimulateManager simulateManager;
    private PlayerManager playerManager;
    private ResultUI resultUI;
    private SoccerCutscene startCutscene;

    protected override void Awake()
    {
        base.Awake();
        timer = new(this); // 타이머가 먼저임
        IngameUI = new(this);

        playerManager = ManagerManager.GetManager<PlayerManager>();
        resultUI = FindAnyObjectByType<ResultUI>();
        startCutscene = new(this, "StartDirector");
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
        print($"checked Cutscene {startCutscene.IsProgress()}");
        startCutscene.Run();
    }

    public void CutsceneFinish()
    {
        // 캠캠캠
        CameraManager.Instance.Transition.FadeChangeCamNoLive(CameraType.Main);

        soccerBall.BallReset();
        playerManager.ResetPos();
        IsPlay = true;

        // 시간
        timer.OnFinishTime += GameStop;

        timer.SetTime(60  * 90);
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
            playerManager.ResetPos();
            IsPlay = true;
        });
    }

    public override void GameStop()
    {
        timer.OnFinishTime -= GameStop;
        IsPlay = false;

        WhistleSound whistle = ManagerManager.GetManager<WhistleSound>();
        
        if (whistle) // 휘슬 시스템이 있당
            whistle.PlayEndSound();

        if (resultUI)
            resultUI.StartScene();
    }

    [ContextMenu("testGameStop")]
    private void ImmediatelyGameStopTest() {
        RedScore = UnityEngine.Random.Range(1, 50);
        BlueScore = UnityEngine.Random.Range(0, 50);

        timer.SetTime(5);
    }
}
