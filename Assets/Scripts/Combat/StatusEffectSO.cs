using UnityEngine;

public abstract class StatusEffectSO : ScriptableObject
{
    public int duration = 1;
    public bool isBuff = false;
    public float multiplier = 1;
    public abstract void ApplyEffect(Unit target);
    public abstract void RemoveEffect(Unit target);
    public abstract void TickEffect(Unit target);
}