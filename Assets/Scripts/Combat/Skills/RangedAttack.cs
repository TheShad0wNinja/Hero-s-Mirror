using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Ranged Attack")]
public class RangedAttack : SkillSO
{
    public int damage;
    protected override void Execute(Unit owner, Unit target)
    {
        ActionQueueManager.EnqueueDamageAction(owner, target, damage);
    }
}