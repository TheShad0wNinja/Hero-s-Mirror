using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Status Effect Skill")]
public class StatusEffectSkill : SkillSO
{
    public StatusEffectSO statusEffectSO;
    protected override void Execute(Unit owner, Unit target)
    {
        ActionQueueManager.EnqueueStatusEffectAction(target, new StatusEffect(statusEffectSO));
    }

    protected override void RegisterParallelTypes()
    {
        ActionQueueManager.EnqueueParallelType(typeof(StatusEffectAction));
    }
}