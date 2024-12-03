using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Bleed")]
public class Bleed : StatusEffectSO
{
    [SerializeField] int bleedDamage = 1;

    public override void ApplyEffect(Character target)
    {
        Debug.Log($"Added bleeding to {target.name}");
    }

    public override void RemoveEffect(Character target)
    {
        Debug.Log($"Removed bleeding to {target.name}");
    }

    public override void TickEffect(Character target)
    {
        target.TakeRawDamage(null, bleedDamage);
    }
}