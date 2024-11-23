using System;
using System.Collections;
using UnityEngine;

public enum BallState
{
    Out,
    Goal,
}
public class GameModeDefault : GameMode, IGameModeTimer, ICutsceneCallback
{
    public GameModeUI IngameUI { get; protected set; }
    SoccerTimer IGameModeTimer.Timer { get => timer; }
    private SoccerTimer timer;

    private BallGoalSimulateManager simulateManager;
    private PlayerManager playerManager;
    private ResultUI resultUI;
    private SoccerCutscene startCutscene;
    private Ground soccerGround;
    private UI_EffectText currentStateText;

    protected override void Awake()
    {
        base.Awake();
        timer = new(this); // 타이머가 먼저임
        IngameUI = new(this);

        soccerGround = FindAnyObjectByType<Ground>();
        playerManager = ManagerManager.GetManager<PlayerManager>();
        currentStateText = FindAnyObjectByType<UI_EffectText>();
        resultUI = FindAnyObjectByType<ResultUI>();
        startCutscene = new(this, "StartDirector");
    }

    private void Update()
    {
        timer.Loop();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    public override void GameStart()
    {
        print($"checked Cutscene {startCutscene.IsProgress()}");
        BallControlBundle.SetInit(false);
        startCutscene.Run();
        IngameUI.Start();
    }

    public void CutsceneFinish()
    {
        // 캠캠캠
        CameraManager.Instance.Transition.FadeChangeCamNoLive(CameraType.Main, () =>
        {
            soccerBall.BallReset();

            playerManager.GetPlayer(BallAreaType.Blue)?.gameObject?.SetActive(true);
            playerManager.GetPlayer(BallAreaType.Red)?.gameObject?.SetActive(true);
            playerManager.ResetPos();
        });

        IsPlay = true;

        // 시간
        timer.OnFinishTime += GameStop;

        timer.SetTime(60 * 90);
        timer.Play();
    }

    protected override void HandleBallGoal(BallAreaType type)
    {
        if (type == BallAreaType.Blue)
        {
            BlueScore++;
            currentStateText.StartEffect(BallState.Goal.ToString(), Color.blue);
        }
        else
        {
            RedScore++;
            currentStateText.StartEffect(BallState.Goal.ToString(), Color.red);
        }

        // 축구공 소유권 빼기
        Player ballOwner = BallControlBundle.GetBallOwner();
        if (ballOwner)
            ballOwner.ForceReleseBall();
        
        StartCoroutine(WaitBallReset());
    }

    protected override void HandleBallOut()
    {
        currentStateText.StartEffect(BallState.Out.ToString() , new Color(255,0,255));
        StartCoroutine(WaitBallReset());
    }

    IEnumerator WaitBallReset()
    {
        IsPlay = false; // 진행 중단 ㄱㄱㄱㄱ

        yield return new WaitForSeconds(5f);

        CameraManager.Instance.Transition.FadeChangeCamNoLive(CameraType.Main, () =>
        {
            soccerBall.BallReset();
            playerManager.ResetPos();
            soccerGround.ResetGround();
            IsPlay = true;
        });
    }

    public override void GameStop()
    {
        timer.OnFinishTime -= GameStop;
        IsPlay = false;

        SoundPlayHelper soundPlayHelper = ManagerManager.GetManager<SoundPlayHelper>();
        
        WhistleSound whistle = soundPlayHelper.GetHelper<WhistleSound>();
        GameBGMs gameBGM = soundPlayHelper.GetHelper<GameBGMs>();
        
        if (gameBGM)
            gameBGM.GameEndSound();
        
        if (whistle) // 휘슬 시스템이 있당
            whistle.PlayEndSound();

        if (resultUI)
            resultUI.StartScene();
    }

    [ContextMenu("testGameStop")]
    private void ImmediatelyGameStopTest()
    {
        RedScore = UnityEngine.Random.Range(1, 50);
        BlueScore = UnityEngine.Random.Range(0, 50);

        timer.SetTime(5);
    }
}
