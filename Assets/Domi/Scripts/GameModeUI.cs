using System;
using UnityEngine;

public class GameModeUI : GameModeCompo
{
    private ScoreBoard scoreBoard;
    private TimeBoard timeBoard;

    public GameModeUI(GameMode gameMode) : base(gameMode) {
        scoreBoard = GameObject.FindAnyObjectByType<ScoreBoard>();
        gm.OnScoreChange += HandleScoreChange;
    }

    private void HandleScoreChange(BallAreaType team, int value)
    {
        if (team == BallAreaType.Blue)
            scoreBoard.UpdateBlueScoreText(value);
        else
            scoreBoard.UpdateRedScoreText(value);
    }
}
