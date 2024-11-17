using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    public PlayerSkillType skillType;
    
    public float defaultCool;
    public Sprite skillIcon;
    
    [HideInInspector] public float currentCool;
    [HideInInspector] public Player player;
    
    public void Init(Player player)
    {
        this.player = player;
    }

    protected virtual void Update()
    {
        if (currentCool > 0)
        {
            currentCool -= Time.deltaTime;
            if (currentCool <= 0)
            {
                currentCool = 0;
            }
        }
    }

    public virtual bool SkillUseAbleCheck()
    {
        if (skillType == PlayerSkillType.None)
            return false;
        
        if (currentCool <= 0)
        {
            currentCool = defaultCool;
            return true;
        }

        return false;
    }

    public abstract void UseSkill();
}