using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Combat Event", fileName = "Combat Event")]
public class CombatEvent : ScriptableObject
{
    public UnityEvent<Character, Character> OnDeath;
    public UnityEvent<Character, StatusEffect> OnEffect;
    public UnityEvent<Character, int> OnDamage;
    public UnityEvent<Character, int> OnShieldDamage;
    public UnityEvent<Character, SkillSO, Character> OnSkill;
    public UnityEvent<CombatManager> OnNewTurn;


    public static CombatEvent Instance { get; private set; }

    void OnEnable()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

        OnDeath = new();
        OnEffect = new();
        OnDamage = new();
        OnSkill = new();
        OnNewTurn = new();
    }

    public void RaiseOnDeathEvent(Character killer, Character victim)
    {
        Debug.Log("OnDeath Event Raised");
        OnDeath?.Invoke(killer, victim);
    }
    public void RaiseOnEffectEvent(Character owner, StatusEffect effect)
    {
        Debug.Log($"OnEffect Event Raised: {owner.name} -> {effect.name}");
        OnEffect?.Invoke(owner, effect);
    }
    public void RaiseOnDamageEvent(Character owner, int damage)
    {
        Debug.Log("OnDamage Event Raised");
        OnDamage?.Invoke(owner, damage);
    }
    public void RaiseOnShieldDamageEvent(Character owner, int damage)
    {
        Debug.Log($"OnShieldDamage Event Raised: {owner.name}");
        OnShieldDamage?.Invoke(owner, damage);
    }
    public void RaiseOnSkillEvent(Character attacker, SkillSO skill, Character victim)
    {
        Debug.Log($"OnSkill Event Raised: {attacker.name} -> {victim.name} = {skill.name}");
        OnSkill?.Invoke(attacker, skill, victim);
    }
    public void RaiseOnNewTurnEvent(CombatManager combatManager)
    {
        Debug.Log("OnNewTurn Event Raised");
        OnNewTurn?.Invoke(combatManager);
    }
}