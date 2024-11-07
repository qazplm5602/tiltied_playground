using System;
using UnityEngine;

public class GameModeUI : GameModeCompo
{
    private ScoreBoard scoreBoard;
    private TimeBoard timeBoard;

    public GameModeUI(GameMode gameMode) : base(gameMode) {
        scoreBoard = GameObject.FindAnyObjectByType<ScoreBoard>();
        timeBoard = GameObject.FindAnyObjectByType<TimeBoard>();
        gm.OnScoreChange += HandleScoreChange;
        
        if (gameMode is IGameModeTimer mode) {
            mode.Timer.OnChangeValue += HandleTimeChange;
        }
    }

    private void HandleScoreChange(BallAreaType team, int value)
    {
        if (team == BallAreaType.Blue)
            scoreBoard.UpdateBlueScoreText(value);
        else
            scoreBoard.UpdateRedScoreText(value);
    }

    private void HandleTimeChange(float value) => timeBoard.UpdateTimerText((int)value);
}
