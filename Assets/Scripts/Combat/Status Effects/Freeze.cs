using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Poison")]
public class Freeze : StatusEffectSO
{
    public override void ApplyEffect(Unit target)
    {
        Debug.Log($"Added Frost to {target}");
        target.HasTurn = false;
    }

    public override void RemoveEffect(Unit target)
    {
        Debug.Log($"Removed Frost from {target}");
    }

    public override void TickEffect(Unit target)
    {
    }
}