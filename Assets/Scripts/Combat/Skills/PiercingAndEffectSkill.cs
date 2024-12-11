using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Piercing And Effect Skill")]
public class PiercingAndEffectSkill : SkillSO
{
    [SerializeField] StatusEffectSO statusEffect;
    [SerializeField] int damage;
    protected override void Execute(Unit owner, Unit target)
    {
        target.TakeRawDamage(owner, damage);
        ActionQueueManager.EnqueueStatusEffectAction(target, new StatusEffect(statusEffect));
    }
}