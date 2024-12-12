using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Ranged And Effect Skill")]
public class RangedAndEffectSkill : SkillSO
{
    [SerializeField] StatusEffectSO statusEffect;
    [SerializeField] int damageAmount;
    protected override void Execute(Unit owner, Unit target)
    {
        if (isParallel)
            ActionQueueManager.Instance.parallelItemType = typeof(DamageAction);
        ActionQueueManager.EnqueueDamageAction(owner, target, damageAmount);
        ActionQueueManager.EnqueueStatusEffectAction(target, new(statusEffect));
    }
}