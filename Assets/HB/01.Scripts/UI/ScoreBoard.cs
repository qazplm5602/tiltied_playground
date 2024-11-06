using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _redScoreText;
    [SerializeField] private TextMeshProUGUI _blueScoreText;

    private void UpdateRedScoreText(int score)
    {
        _redScoreText.text = score.ToString();
    }

    private void UpdateBlueScoreText(int score)
    {
        _blueScoreText.text = score.ToString();
    }
}
