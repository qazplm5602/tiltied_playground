using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [SerializeField] RectTransform redScore;
    [SerializeField] RectTransform blueScore;

    private void Start() {
        ShowResult(20, 5);
    }

    public void ShowResult(int red, int blue) {
        CanvasGroup redGroup = redScore.GetComponent<CanvasGroup>();
        CanvasGroup blueGroup = blueScore.GetComponent<CanvasGroup>();

        TextMeshProUGUI redText = redScore.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI blueText = blueScore.GetComponentInChildren<TextMeshProUGUI>();
        
        Sequence sequence = DOTween.Sequence();
        
        sequence.Join(redGroup.DOFade(1, 0.5f));
        sequence.Join(blueGroup.DOFade(1, 0.5f));

        sequence.Join(DOTween.To(() => 0, v => redText.text = v.ToString(), red, 1f).SetEase(Ease.Linear));
        sequence.Join(DOTween.To(() => 0, v => blueText.text = v.ToString(), blue, 1f).SetEase(Ease.Linear));

        // 왕관
        if (red != blue) {
            RectTransform crown = (red > blue ? redScore : blueScore).Find("Crown") as RectTransform;
            Image crownImage = crown.GetComponent<Image>();

            crown.sizeDelta = new Vector2(50, 50);
            crownImage.color = new Color(1, 1, 1, 0);

            sequence.Append(crownImage.DOFade(1, 0.5f).SetEase(Ease.OutBack));
            sequence.Join(crown.DOSizeDelta(new Vector2(80, 80), 0.5f).SetEase(Ease.OutBack));
            
            sequence.AppendInterval(0.5f);

            // 사이즈 커지기
            RectTransform winner = red > blue ? redScore : blueScore;
            RectTransform loser = red > blue ? blueScore : redScore;

            sequence.Append(winner.DOScale(Vector2.one * 1.2f, 0.3f).SetEase(Ease.OutBack));
            sequence.Join(loser.DOScale(Vector2.one * 0.8f, 0.3f).SetEase(Ease.OutBack));
        }


    }
}
