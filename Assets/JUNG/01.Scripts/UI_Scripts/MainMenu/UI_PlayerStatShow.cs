using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStatShow : MonoBehaviour
{

    [Header("Player Stat Image")]

    [SerializeField] private Image heightValue;
    [SerializeField] private Image weightValue;
    [SerializeField] private Image speedValue;
    [SerializeField] private Image powerValue;

    private float normalizedheightValue;
    private float normalizedweightValue;
    private float normalizedspeedValue;
    private float normalizedpowerValue;

    [Header("Player Skill Image")]
    [SerializeField] private Image playerSkill;
    [SerializeField] private TextMeshProUGUI skillInfoText;


    public void OnStatChange(PlayerStatsSO stat)
    {
        playerSkill.sprite = stat.skillData.skillIcon; // 스킬 아이콘 , 스킬 정보 초기화
        skillInfoText.text = stat.skillData.skillInfo;

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
