using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Bleed")]
public class Bleed : StatusEffectSO
{
    [SerializeField] int bleedDamage = 1;

    public override void ApplyEffect(Unit target)
    {
        Debug.Log($"Added bleeding to {target.name}");
    }

    public override void RemoveEffect(Unit target)
    {
        Debug.Log($"Removed bleeding to {target.name}");
    }

    public override void TickEffect(Unit target)
    {
        target.TakeRawDamage(null, bleedDamage);
    }
}