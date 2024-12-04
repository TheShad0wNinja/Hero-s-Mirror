using UnityEngine;

public enum TargetType
{
    PLAYER_UNIT_SINGLE,
    ENEMY_UNIT_SINGLE,
    PLAYER_UNIT_MULTIPLE,
    ENEMY_UNIT_MULTIPLE,
}

public abstract class SkillSO : ScriptableObject
{
    public TargetType targetType;
    public bool isOffensive = false;
    public Sprite sprite;
    public string skillName;
    public void ExecuteSkill(Unit owner, params Unit[] targets)
    {
        foreach (Unit target in targets)
            Execute(owner, target);
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