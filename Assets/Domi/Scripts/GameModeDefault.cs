using UnityEngine;

public class GameModeDefault : GameMode
{
    public override void GameStart()
    {
        soccerBall.BallReset();
    }

    protected override void HandleBallGoal(BallAreaType type)
    {

    }

    protected override void HandleBallOut()
    {
        
    }
}
