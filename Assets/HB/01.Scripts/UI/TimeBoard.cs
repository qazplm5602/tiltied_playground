using System.Collections;
using TMPro;
using UnityEngine;

public class TimeBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;

    public int m, s;
    private int time;

    private bool _isTimerRunning = false;
    private bool _isPaused = false;

    private void Start()
    {
        time = m * 60 + s;
        _isTimerRunning = true;

        StartCoroutine(UpdateTimer());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            PauseTimer();
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            ResumeTimer();
        }
    }

    private IEnumerator UpdateTimer()
    {
        while (_isTimerRunning)
        {
            if (!_isPaused)
            {
                if (time > 0)
                {
                    time -= 1;
                    UpdateTimerText(time);
                    yield return new WaitForSeconds(1);
                }
                else
                {
                    time = 0;
                    _isTimerRunning = false;
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    public void UpdateTimerText(int time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        _timerText.text = $"{minutes}:{seconds}";
    }

    public void PauseTimer()
    {
        _isPaused = true;
    }

    public void ResumeTimer()
    {
        _isPaused = false;
    }
}