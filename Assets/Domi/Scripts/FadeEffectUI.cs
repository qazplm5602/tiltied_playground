using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeEffectUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show(float duration) {
        canvasGroup.DOKill();
        canvasGroup.DOFade(1, duration);
    }

    public void Hide(float duration) {
        canvasGroup.DOKill();
        canvasGroup.DOFade(0, duration);
    }
}
