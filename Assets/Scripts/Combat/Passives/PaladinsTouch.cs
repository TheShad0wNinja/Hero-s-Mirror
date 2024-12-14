using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Passives/PaladinsTouch")]
public class PaladinsTouch : PassiveSO
{
    public override void SubscribeToEvent(Passive instance)
    {
        // if (CombatEvent.Instance != null)
        // CombatEvent.Instance.UnitStatusEffect = Execute;
    }

    public override void UnsubscribeToEvent(Passive instance)
    {
        // throw new NotImplementedException();
    }

    // void Execute(Unit unit, StatusEffect statusEffect)
    // {
    //     if (statusEffect.IsBuff && statusEffect.name == "shield" || statusEffect.name == "heal")
    //     {
    //         ActionQueueManager.EnqueueStatusEffectAction(unit, statusEffect);
    //     }
    // }
}