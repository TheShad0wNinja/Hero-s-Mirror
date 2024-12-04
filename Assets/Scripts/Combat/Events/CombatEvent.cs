using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Combat Event", fileName = "Combat Event")]
public class CombatEvent : ScriptableObject
{
    public UnityEvent<Unit, Unit> OnDeath;
    public UnityEvent<Unit, StatusEffect> OnEffect;
    public UnityEvent<Unit, int> OnDamage;
    public UnityEvent<Unit, int> OnShieldDamage;
    public UnityEvent<Unit, SkillSO, Unit> OnSkill;
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

    public void RaiseOnDeathEvent(Unit killer, Unit victim)
    {
        Debug.Log("OnDeath Event Raised");
        OnDeath?.Invoke(killer, victim);
    }
    public void RaiseOnEffectEvent(Unit owner, StatusEffect effect)
    {
        Debug.Log($"OnEffect Event Raised: {owner.name} -> {effect.name}");
        OnEffect?.Invoke(owner, effect);
    }
    public void RaiseOnDamageEvent(Unit owner, int damage)
    {
        Debug.Log("OnDamage Event Raised");
        OnDamage?.Invoke(owner, damage);
    }
    public void RaiseOnShieldDamageEvent(Unit owner, int damage)
    {
        Debug.Log($"OnShieldDamage Event Raised: {owner.name}");
        OnShieldDamage?.Invoke(owner, damage);
    }
    public void RaiseOnSkillEvent(Unit attacker, SkillSO skill, Unit victim)
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