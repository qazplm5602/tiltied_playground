using System.Collections;
using UnityEngine;

public class SoccerTimer : GameModeCompo
{
    private readonly float SPEED = 10;

    private bool isPlay = false;
    private float time = 0;

    public event System.Action<float> OnChangeValue;
    public event System.Action OnFinishTime;

    public SoccerTimer(GameMode mode) : base(mode)
    {
    }

    public void Loop() {
        if (!isPlay || time <= 0) return;
        SetTime(Mathf.Max(time - Time.deltaTime * SPEED, 0));
    }
    
    public void SetTime(float value) {
        time = value;
        OnChangeValue?.Invoke(value);

        if (value <= 0 && isPlay) {
            isPlay = false; // 끗
            OnFinishTime?.Invoke();
        }
    }

    public void Play() => isPlay = true;
    public void Pause() => isPlay = false;
}
