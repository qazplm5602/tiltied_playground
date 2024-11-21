using System.Collections.Generic;
using UnityEngine;

public enum PlayerSkillType
{
    //없음
    None,
    //방귀
    Fart,
    //명상
    Meditation,
    //대쉬
    Dash,
    //벽생성
    BuildWall,
    //거대해지기
    Bigger,
    //점프
    Jump,
    //시야
    UnvisibleUi

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
            if(_skillBaseDic.ContainsKey(skillBase.GetSkillType()))
                return;

            _skillBaseDic.Add(skillBase.GetSkillType(), skillBase);
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
