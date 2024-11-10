using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SkillBoard : MonoBehaviour
{
    [SerializeField] private Skill _skill;

    private void Start()
    {
        CoolDown(_skill.skillImages[1]);
    }

    private void CoolDown(Image skillImage)
    {
        skillImage.DOFillAmount(1, _skill.coolTime);
    }
}
