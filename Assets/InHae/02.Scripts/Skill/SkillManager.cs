using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum PlayerSkillType
{
    None,
}

public class SkillManager : MonoSingleton<SkillManager>
{
    [SerializeField] private List<SkillBase> _skillList;
    private Dictionary<PlayerSkillType, SkillBase> _skillBaseDic;

    protected override void Awake()
    {
        base.Awake();

        _skillBaseDic = new Dictionary<PlayerSkillType, SkillBase>();
        SkillInit();
    }

    public void SkillInit()
    {
        foreach (SkillBase skillBase in _skillList)
        {
            if(_skillBaseDic.ContainsKey(skillBase.skillType))
                return;

            _skillBaseDic.Add(skillBase.skillType, skillBase);
        }
    }

    public SkillBase GetSkill(PlayerSkillType type)
    {
        if (_skillBaseDic.TryGetValue(type, out SkillBase skillBase))
        {
            return skillBase;
        }

        return null;
    }
}
