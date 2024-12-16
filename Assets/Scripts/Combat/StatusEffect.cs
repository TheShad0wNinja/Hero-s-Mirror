using System;
using UnityEngine;

[Serializable]
public class StatusEffect
{
    public string name;
    public int duration;
    public bool IsExpired => duration <= 0;
    public bool IsBuff => effectData.isBuff;
    public Sprite Image => effectData.image;
    public StatusEffectSO effectData;
    public StatusEffect(StatusEffectSO effectSO)
    {
        effectData = effectSO;
        duration = effectData.duration;
        name = effectSO.GetType().FullName;
    }

    public void ApplyEffect(Unit target)
    {
        effectData.ApplyEffect(target);
    }
    public void RemoveEffect(Unit target)
    {
        effectData.RemoveEffect(target);
    }
    public void TickEffect(Unit target)
    {
        effectData.TickEffect(target);
        duration--;
    }

    public void ApplyMultiplier(float amount)
    {
        effectData.multiplier = amount;
    }

    public void RemoveMultiplier()
    {
        effectData.multiplier = 1;
    }
}