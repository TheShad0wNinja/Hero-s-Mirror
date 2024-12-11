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

    private void Execute(Unit unit, StatusEffect statusEffect)
    {
        if (statusEffect.effectData is Flame flame && unit.UnitName == self.unitName) 
        {
            unit.Heal(flame.flameDamage);
        }
    }
}