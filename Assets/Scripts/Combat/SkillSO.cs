using System.Linq;
using UnityEngine;

public enum TargetType
{
    SELF,
    UNIT_ALL,
    ENEMY_UNIT_RANDOM,
    PLAYER_UNIT_RANDOM,
    PLAYER_UNIT_SINGLE,
    PLAYER_UNIT_MULTIPLE,
    PLAYER_UNIT_ALL,
    ENEMY_UNIT_SINGLE,
    ENEMY_UNIT_MULTIPLE,
    ENEMY_UNIT_ALL,
}

public abstract class SkillSO : ScriptableObject
{
    public TargetType targetType;
    public int numberOfTargets = 1;
    public Sprite sprite;
    public string skillName;
    public string animationName;
    public bool isOffensive = true;
    protected bool isParallel = false;
    public void ExecuteSkill(Unit owner, params Unit[] targets)
    {
        if (targets.Length > 1)
        {
            isParallel = true;
            ActionQueueManager.Instance.hasParallel = true;
        }

        foreach (Unit target in targets)
        {
            Execute(owner, target);
            CombatEvent.OnSkillPerformed(owner, this, target);
        }
    }
    protected abstract void Execute(Unit owner, Unit target);

    public override bool Equals(object other)
    {
        return this.GetHashCode() == other.GetHashCode();
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + skillName.GetHashCode();
            hash = hash * 23 + name.GetHashCode();
            return hash;
        }
    }
}