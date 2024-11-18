using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStatShow : MonoBehaviour
{
    [SerializeField] private Image heightValue;
    [SerializeField] private Image weightValue;
    [SerializeField] private Image speedValue;
    [SerializeField] private Image powerValue;

    float normalizedheightValue;
    float normalizedweightValue;
    float normalizedspeedValue;
    float normalizedpowerValue;


    public void OnStatChange(PlayerStatsSO stat)
    {
        normalizedheightValue = Mathf.Clamp01(stat.height / 250f) * 100f + 50f;
        normalizedweightValue = Mathf.Clamp01(stat.weight / 250f) * 100f + 50f;
        normalizedspeedValue = Mathf.Clamp01(stat.defaultSpeed.GetValue() / 250f) * 100f + 50f;
        normalizedpowerValue = Mathf.Clamp01(stat.shootPower.GetValue() / 250f) * 100f + 50f;

        heightValue.rectTransform.DOSizeDelta(new Vector2(normalizedheightValue, 50), 0.2f);
        weightValue.rectTransform.DOSizeDelta(new Vector2(normalizedweightValue, 50), 0.2f);
        speedValue.rectTransform.DOSizeDelta(new Vector2(normalizedspeedValue, 50), 0.2f);
        powerValue.rectTransform.DOSizeDelta(new Vector2(normalizedpowerValue, 50), 0.2f);
    }
}
