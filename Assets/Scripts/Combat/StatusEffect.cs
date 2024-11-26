using System;

[Serializable]
public class StatusEffect
{
    public string name;
    public int duration;
    public bool IsExpired => duration <= 0;

    StatusEffectSO effectData;
    public StatusEffect(StatusEffectSO effectSO)
    {
        effectData = effectSO;
        duration = effectData.duration;
    }

    public void ApplyEffect(Character target)
    {
        effectData.ApplyEffect(target);
    }
    public void RemoveEffect(Character target)
    {
        effectData.RemoveEffect(target);
    }
    public void TickEffect(Character target)
    {
        effectData.TickEffect(target);
        duration--;
    }
}