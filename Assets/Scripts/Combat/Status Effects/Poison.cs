using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Poison")]
public class Poison : StatusEffectSO
{
    [SerializeField] int poisonDamage = 1;

    public override void ApplyEffect(Unit target)
    {
        Debug.Log($"Added poison to {target.name}");
    }

    public override void RemoveEffect(Unit target)
    {
        Debug.Log($"Removed poison to {target.name}");
    }

    public override void TickEffect(Unit target)
    {
        target.TakeRawDamage(null, poisonDamage);
    }
}