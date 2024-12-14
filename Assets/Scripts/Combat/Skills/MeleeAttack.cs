using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Melee Attack", fileName = "Melee Attack")]
public class MeleeAttack : SkillSO
{
    [SerializeField] int damage = 2;

    protected override void Execute(Unit owner, Unit target)
    {
        ActionQueueManager.EnqueueDamageAction(owner, target, damage);
    }

    protected override void RegisterParallelTypes()
    {
        ActionQueueManager.EnqueueParallelType(typeof(DamageAction));
    }
}
