using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _redScoreText;
    [SerializeField] private TextMeshProUGUI _blueScoreText;
    [SerializeField] private TextMeshProUGUI _redNameText;
    [SerializeField] private TextMeshProUGUI _blueNameText;

    public void UpdateRedScoreText(int score)
    {
        _redScoreText.text = score.ToString();
    }

    public void UpdateBlueScoreText(int score)
    {
        _blueScoreText.text = score.ToString();
    }

    public void SetRedName(string value) {
        _redNameText.text = value;
    }

    public void SetBlueName(string value) {
        _blueNameText.text = value;
    }
}
