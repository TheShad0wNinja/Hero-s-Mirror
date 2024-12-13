using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Ranged And Effect Skill")]
public class RangedAndEffectSkill : SkillSO
{
    [SerializeField] StatusEffectSO statusEffect;
    [SerializeField] int damageAmount;
    protected override void Execute(Unit owner, Unit target)
    {
        ActionQueueManager.EnqueueDamageAction(owner, target, damageAmount);
        ActionQueueManager.EnqueueStatusEffectAction(target, new(statusEffect));
    }

    protected override void RegisterParallelTypes()
    {
        ActionQueueManager.EnqueueParallelType(typeof(DamageAction), typeof(StatusEffectAction));
    }
}