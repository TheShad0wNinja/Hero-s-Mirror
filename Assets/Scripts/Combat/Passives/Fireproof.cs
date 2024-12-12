using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Passives/Fireproof")]
public class Fireproof : PassiveSO
{
    [SerializeField] UnitSO self;

    public override void SubscribeToEvent(Passive instance)
    {
        if (CombatEvent.Instance != null)
            CombatEvent.Instance.UnitStatusEffect += Execute;
    }

    private void Execute(Unit unit, StatusEffect statusEffect, StatusEffectAction.ActionType actionType)
    {
        if (statusEffect.effectData is Flame flame && 
            actionType == StatusEffectAction.ActionType.TICK 
            && unit.UnitName == self.unitName)
        {
            unit.Heal(flame.flameDamage);
        }
    }
}