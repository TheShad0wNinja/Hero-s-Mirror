using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effects/Shield")]
public class Shield : StatusEffectSO
{
    [SerializeField] int shieldAmount = 4;
    List<AppliedUnit> appliedUnits = new();
    struct AppliedUnit
    {
        public Unit unit;
        public int prevShieldValue;
    }
    public override void ApplyEffect(Unit target)
    {
        Debug.Log($"Added Shield to {target.name}");
        AppliedUnit appliedUnit = new() { unit = target, prevShieldValue = target.Shield };
        appliedUnits.Add(appliedUnit);
        target.Shield = shieldAmount;
    }

    public override void RemoveEffect(Unit target)
    {
        Debug.Log($"Removed Shield Blessing from {target.name}");
        AppliedUnit appliedUnit = appliedUnits.Find(au => au.unit = target);
        int currShield = target.Shield;
        int prevShield = appliedUnit.prevShieldValue;

        if (currShield >= prevShield && currShield >= (prevShield + shieldAmount))
            target.Shield -= shieldAmount;
        else if (currShield >= prevShield && currShield < (prevShield + shieldAmount))
            target.Shield -= currShield - shieldAmount;
    }

    public override void TickEffect(Unit target)
    {
    }
}