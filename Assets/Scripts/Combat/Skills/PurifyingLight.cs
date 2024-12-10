using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/PurifyingLight")]
public class PurifyingLight : SkillSO
{
    protected override void Execute(Unit owner, Unit target)
    {
        foreach(var statusEffect in target.activeEffects.ToList())
        {
            if (!statusEffect.IsBuff) 
            {
                statusEffect.RemoveEffect(target);
                target.RemoveStatusEffect(statusEffect);
            }
        }
    }
}