using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Slash And Stab")]
public class SlashAndStab : SkillSO
{
    public Bleed bleedSO;
    protected override void Execute(Unit owner, Unit target)
    {
        ActionQueueManager.EnqueueStatusEffectAction(target, new StatusEffect(bleedSO));
    }
}