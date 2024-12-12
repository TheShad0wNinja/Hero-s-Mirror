using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Combat Event", fileName = "Combat Event")]
public class CombatEvent : ScriptableObject
{
    public UnityAction<Unit, Unit> UnitDeath;
    public UnityAction<Unit, StatusEffect, StatusEffectAction.ActionType> UnitStatusEffect;
    public UnityAction<Unit, int> UnitHeal;
    public UnityAction<Unit, int> UnitDamage;
    public UnityAction<Unit, int> UnitShieldDamage;
    public UnityAction<Unit, SkillSO, Unit> SkillPerformed;
    public UnityAction<CombatManager> TurnChanged;
    public UnityAction<CombatManager> NewTurn;
    public UnityAction ActionsCompleted; 

    public static CombatEvent Instance { get; private set; }

    void OnEnable()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    public static void OnUnitDeath(Unit killer, Unit victim)
    {
        if (Instance == null) return;
        Debug.Log("OnDeath Event Raised");
        Instance.UnitDeath?.Invoke(killer, victim);
    }

    public void OnUnitStatusEffect (Unit owner, StatusEffect effect, StatusEffectAction.ActionType actionType)
    {
        if (Instance == null) return;
        Debug.Log($"OnEffect Event Raised: {owner.name} -> {effect.name}");
        Instance.UnitStatusEffect?.Invoke(owner, effect, actionType);
    }
    public static void OnUnitDamage(Unit owner, int damage)
    {
        if (Instance == null) return;
        Debug.Log("OnDamage Event Raised");
        Instance.UnitDamage?.Invoke(owner, damage);
    }
    public static void OnUnitShieldDamage(Unit owner, int damage)
    {
        if (Instance == null) return;
        Debug.Log($"OnShieldDamage Event Raised: {owner.name}");
        Instance.UnitShieldDamage?.Invoke(owner, damage);
    }
    public static void OnSkillPerformed(Unit attacker, SkillSO skill, Unit victim)
    {
        if (Instance == null) return;
        Debug.Log($"OnSkill Event Raised: {attacker} -> {victim} = {skill}");
        Instance.SkillPerformed?.Invoke(attacker, skill, victim);
    }
    public static void OnNewTurn(CombatManager combatManager)
    {
        if (Instance == null) return;
        Debug.Log("OnNewTurn Event Raised");
        Instance.NewTurn?.Invoke(combatManager);
    }

    public static void OnTurnChanged(CombatManager cm)
    {
        if (Instance == null) return;
        Debug.Log("OnNewTurn Event Raised");
        Instance.TurnChanged?.Invoke(cm);
    }

    public static void OnActionsCompleted()
    {
        if (Instance == null) return;
        Debug.Log("Actions Completed");
        Instance.ActionsCompleted?.Invoke();
    }

    public static void OnUnitHeal(Unit unit, int amount)
    {
        if (Instance == null) return;
        Instance.UnitHeal?.Invoke(unit, amount);
    }
}