using System;
using UnityEngine;

// TODO: Use heal action
[CreateAssetMenu(menuName = "Passives/Fireproof")]
public class Fireproof : PassiveSO
{
    [SerializeField] UnitSO self;

    public override void SubscribeToEvent(Passive instance)
    {
        // if (CombatEvent.Instance != null)
        //     CombatEvent.Instance.UnitStatusEffect += Execute;
    }

    public override void UnsubscribeToEvent(Passive instance)
    {
        // throw new NotImplementedException();
    }

    private void Execute(Unit unit, StatusEffect statusEffect, StatusEffectAction.ActionType actionType)
    {
        if (statusEffect.effectData is Flame flame && 
            actionType == StatusEffectAction.ActionType.TICK 
            && unit.name == self.name)
        {
            unit.Heal(flame.flameDamage);
        }
    }
}