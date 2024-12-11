using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Melee And Effect Skill")]
public class MeleeAndEffectSkill : SkillSO
{
    [SerializeField] StatusEffectSO statusEffect;
    [SerializeField] int damageAmount;
    protected override void Execute(Unit owner, Unit target)
    {
        ActionQueueManager.EnqueueDamageAction(target, damageAmount);
        ActionQueueManager.EnqueueStatusEffectAction(target, new (statusEffect));
    }
}