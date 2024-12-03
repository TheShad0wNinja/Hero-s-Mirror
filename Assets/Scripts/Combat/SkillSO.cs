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
    public void ExecuteSkill(Character owner, params Character[] targets)
    {
        foreach (Character target in targets)
            Execute(owner, target);
    }
    protected abstract void Execute(Character owner, Character target);
}