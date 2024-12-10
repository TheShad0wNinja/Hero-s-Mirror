using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Buff Skill", fileName = "Buff Skill")]
public class BuffSkill : SkillSO
{
    [SerializeField] StatusEffectSO buffEffect;
    protected override void Execute(Unit owner, Unit target)
    {
        ActionQueueManager.EnqueueStatusEffectAction(target, new StatusEffect(buffEffect));
    }
}