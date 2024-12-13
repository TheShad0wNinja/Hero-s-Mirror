using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Piercing And Effect Skill")]
public class PiercingAndEffectSkill : SkillSO
{
    [SerializeField] StatusEffectSO statusEffect;
    [SerializeField] int damage;
    protected override void Execute(Unit owner, Unit target)
    {
        ActionQueueManager.EnqueueRawDamageAction(owner, target, damage);
        ActionQueueManager.EnqueueStatusEffectAction(target, new StatusEffect(statusEffect));
    }

    protected override void RegisterParallelTypes()
    {
        ActionQueueManager.EnqueueParallelType(typeof(DamageAction), typeof(StatusEffectAction));
    }
}