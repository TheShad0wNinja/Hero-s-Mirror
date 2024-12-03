using UnityEngine;

[CreateAssetMenu(menuName = "Skills/SwordSlash")]
public class SwordSlash : SkillSO
{
    [SerializeField] int damage = 2;

    protected override void Execute(Character owner, Character target)
    {
        ActionQueueManager.Instance.EnqueueAction(target, damage);
        // target.TakeDamage(owner, damage);
    }
}
