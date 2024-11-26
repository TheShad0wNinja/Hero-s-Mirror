using UnityEngine;

public abstract class StatusEffectSO : ScriptableObject
{
    public int duration = 1;
    public bool isBuff = false;
    public abstract void ApplyEffect(Character target);
    public abstract void RemoveEffect(Character target);
    public abstract void TickEffect(Character target);
}