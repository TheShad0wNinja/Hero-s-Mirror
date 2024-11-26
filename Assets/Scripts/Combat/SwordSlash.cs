using UnityEngine;

[CreateAssetMenu(menuName = "Skills/SwordSlash")]
public class SwordSlash : SkillSO
{
    [SerializeField] int damage = 2;

    protected override void Execute(Character owner, Character target)
    {
        target.TakeDamage(owner, damage);
    }
}
