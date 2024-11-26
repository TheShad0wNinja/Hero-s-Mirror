using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Combat Event", fileName = "Combat Event")]
public class CombatEvent : ScriptableObject
{
    public UnityEvent<Character, Character> OnKill;
    public UnityEvent<Character, StatusEffect> OnEffect;
    public UnityEvent<Character> OnDamage;
    public UnityEvent<Character> OnShieldDamage;
    public UnityEvent<Character, Character> OnAttack;
    public UnityEvent<CombatManager> OnNewTurn;

    void OnEnable()
    {
        OnKill = new();
        OnEffect = new();
        OnDamage = new();
        OnAttack = new();
        OnNewTurn = new();
    }

    public void RaiseOnKillEvent(Character killer, Character victim)
    {
        Debug.Log("OnKill Event Raised");
        OnKill?.Invoke(killer, victim);
    }
    public void RaiseOnEffectEvent(Character owner, StatusEffect effect)
    {
        Debug.Log("OnEffect Event Raised");
        OnEffect?.Invoke(owner, effect);
    }
    public void RaiseOnDamageEvent(Character owner)
    {
        Debug.Log("OnDamage Event Raised");
        OnDamage?.Invoke(owner);
    }
    public void RaiseOnShieldDamageEvent(Character owner)
    {
        Debug.Log($"OnShieldDamage Event Raised: {owner.name}");
        OnShieldDamage?.Invoke(owner);
    }
    public void RaiseOnAttackEvent(Character attacker, Character victim)
    {
        Debug.Log($"OnAttack Event Raised: {attacker.name} -> {victim.name}");
        OnAttack?.Invoke(attacker, victim);
    }
    public void RaiseOnNewTurnEvent(CombatManager combatManager)
    {
        Debug.Log("OnNewTurn Event Raised");
        OnNewTurn?.Invoke(combatManager);
    }
}