using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [SerializeField] RectTransform redScore;
    [SerializeField] RectTransform blueScore;
    [SerializeField] Button backBtn;

    private void Awake() {
        backBtn.onClick.AddListener(HandleBackClick);
    }

    private void Start() {
        ShowResult(200, 50);
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

            if (red > blue) {
                sequence.Join(redScore.DOAnchorPosX(-440 + 60, 0.5f).SetEase(Ease.OutBack));
                // sequence.Join(blueScore.DOAnchorPosX(-193 + 60, 0.5f).SetEase(Ease.OutBack));
            } else {
                sequence.Join(redScore.DOAnchorPosX(-440 - 60, 0.5f).SetEase(Ease.OutBack));
            }
        }
    
        // 돌아가기 버튼임
        CanvasGroup btnGroup = backBtn.GetComponent<CanvasGroup>();
        btnGroup.blocksRaycasts = true;
        btnGroup.interactable = true;

        sequence.Append(btnGroup.DOFade(1, 0.5f));
    }

    private void HandleBackClick() {
        LoadingManager.LoadScene("TitleScene");
    }
}
