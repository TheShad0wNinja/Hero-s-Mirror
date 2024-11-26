using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Slash And Stab")]
public class SlashAndStab : SkillSO
{
    public Bleed bleedSO;
    protected override void Execute(Character owner, Character target)
    {
        target.AddStatusEffect(new StatusEffect(bleedSO));
    }
}