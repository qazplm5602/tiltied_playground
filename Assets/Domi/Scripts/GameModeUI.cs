using System;
using UnityEngine;

public class GameModeUI
{
    private ScoreBoard scoreBoard;
    private TimeBoard timeBoard;

    private GameMode gm;

    public GameModeUI(GameMode gameMode) {
        gm = gameMode;

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
