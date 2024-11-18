using System;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class SkillBase : MonoBehaviour
{
    public SkillDataSO skillData;
    public event Action<float, float> coolChangeEvent; 
    
    protected float _currentCool;
    protected Player _player;
    
    public void Init(Player player)
    {
        this._player = player;
    }

    protected virtual void Update()
    {
        if (_currentCool > 0)
        {
            _currentCool -= Time.deltaTime;
            if (_currentCool <= 0)
            {
                _currentCool = 0;
            }

            coolChangeEvent?.Invoke(_currentCool, skillData.defaultCool);
        }
    }

    public virtual bool SkillUseAbleCheck()
    {
        if (skillData.skillType == PlayerSkillType.None)
            return false;
        
        if (_currentCool <= 0)
        {
            _currentCool = skillData.defaultCool;
            return true;
        }

        return false;
    }

    public abstract void UseSkill();

    public PlayerSkillType GetSkillType() => skillData.skillType;
}