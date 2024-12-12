using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Status Effect Skill")]
public class StatusEffectSkill : SkillSO
{
    public StatusEffectSO statusEffectSO;
    protected override void Execute(Unit owner, Unit target)
    {
        if (isParallel)
            ActionQueueManager.Instance.parallelItemType = typeof(StatusEffectAction);
        ActionQueueManager.EnqueueStatusEffectAction(target, new StatusEffect(statusEffectSO));
    }
}