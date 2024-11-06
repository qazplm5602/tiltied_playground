using UnityEngine;

public abstract class GameMode : MonoBehaviour
{
    public int RedScore { get; protected set; }
    public int BlueScore { get; protected set; }
    protected SoccerBall soccerBall; // 축구 없는 게임모드는 없으니까 여기에 있어도 되것지???
    
    protected virtual void Awake() {
        // 현재는 있는 축구공을 불러오지만 나중에는 소환 하는 방법으로도 할 수 있음 ㅁㄴㅇㄹ
        soccerBall = FindAnyObjectByType<SoccerBall>();

        soccerBall.OnGoal += HandleBallGoal;
        soccerBall.OnOut += HandleBallOut;
    }

    protected virtual void OnDestroy() {
        soccerBall.OnGoal -= HandleBallGoal;
        soccerBall.OnOut -= HandleBallOut;
    }

    public abstract void GameStart();

    // ===== 축구공 관련
    protected abstract void HandleBallGoal(BallAreaType type);
    protected abstract void HandleBallOut();
}
