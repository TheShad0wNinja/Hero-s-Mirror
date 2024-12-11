using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Flame")]
public class Flame : StatusEffectSO
{
    [SerializeField] public int flameDamage;
    public override void ApplyEffect(Unit target)
    {
        Debug.Log($"Added Flame to {target.name}");
    }

    public override void RemoveEffect(Unit target)
    {
        Debug.Log($"Remove Flame to {target.name}");
    }

    public override void TickEffect(Unit target)
    {
        target.TakeRawDamage(null, flameDamage);
    }
}