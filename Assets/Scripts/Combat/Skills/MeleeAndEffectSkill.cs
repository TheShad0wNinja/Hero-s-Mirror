using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Melee And Effect Skill")]
public class MeleeAndEffectSkill : SkillSO
{
    [SerializeField] StatusEffectSO statusEffect;
    [SerializeField] int damageAmount;
    protected override void Execute(Unit owner, Unit target)
    {
        int dmg = owner.CritChance <= Random.Range(0, 1f) ? Mathf.RoundToInt(damageAmount * owner.AttackBonus * 1.2f) : Mathf.RoundToInt(damageAmount * owner.AttackBonus);
        ActionQueueManager.EnqueueDamageAction(owner, target, dmg);
        ActionQueueManager.EnqueueStatusEffectAction(target, new(statusEffect));
    }

    protected override void RegisterParallelTypes()
    {
        ActionQueueManager.EnqueueParallelType(typeof(DamageAction), typeof(StatusEffectAction));
    }
}