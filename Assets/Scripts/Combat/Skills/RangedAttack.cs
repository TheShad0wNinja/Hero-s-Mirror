using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Ranged Attack")]
public class RangedAttack : SkillSO
{
    public int damage;
    protected override void Execute(Unit owner, Unit target)
    {
        int dmg = owner.CritChance <= Random.Range(0, 1f) ? Mathf.RoundToInt(damage+ (damage * owner.AttackBonus * 1.2f)) : Mathf.RoundToInt(damage + (damage * owner.AttackBonus));
        ActionQueueManager.EnqueueDamageAction(owner, target, dmg);
    }

    protected override void RegisterParallelTypes()
    {
        ActionQueueManager.EnqueueParallelType(typeof(DamageAction));
    }
}