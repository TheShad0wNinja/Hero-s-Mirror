using UnityEngine;

[CreateAssetMenu(menuName = "Skills/SwordSlash")]
public class SwordSlash : SkillSO
{
    [SerializeField] int damage = 2;

    protected override void Execute(Unit owner, Unit target)
    {
        ActionQueueManager.EnqueueDamageAction(target, damage);
    }
}
