using System.Collections;
using UnityEngine;

public class SoccerTimer : GameModeCompo
{
    private readonly float SPEED = 2;

    private bool isPlay = false;
    private float time = 0;

    public event System.Action<float> OnChangeValue;

    public SoccerTimer(GameMode mode) : base(mode)
    {
    }

    public void Loop() {
        if (!isPlay || time <= 0) return;
        SetTime(Mathf.Max(time - Time.deltaTime * SPEED, 0));
    }
    
    private void SetTime(float value) {
        time = value;
        OnChangeValue?.Invoke(value);

        if (value <= 0 && isPlay)
            isPlay = false; // 끗
    }
}
