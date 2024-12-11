using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Ranged Attack")]
public class RangedAttack : SkillSO
{
    public GameObject projectile; 
    public int damage;
    protected override void Execute(Unit owner, Unit target)
    {
        ActionQueueManager.EnqueueDamageAction(owner, damage);
    }
}