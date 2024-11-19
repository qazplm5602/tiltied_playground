using UnityEngine;

[CreateAssetMenu(fileName = "SkillDataSO", menuName = "Scriptable Objects/SkillDataSO")]
public class SkillDataSO : ScriptableObject
{
    [Tooltip("스킬 타입")] public PlayerSkillType skillType;
    [Tooltip("스킬 쿨타임")] public float defaultCool;
    [Tooltip("스킬 이미지")] public Sprite skillIcon;
    [Tooltip("스킬 정보")] public string skillInfo;
}
