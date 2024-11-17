using System;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    private float _defaultCool;
    private float _currentCool;

    private void Update()
    {
        _currentCool -= Time.deltaTime;
    }

    public virtual bool SkillUseAbleCheck()
    {
        if (_currentCool <= 0)
        {
            _currentCool = _defaultCool;
            return true;
        }

        return false;
    }

    public abstract void UseSkill();
}