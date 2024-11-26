using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class Character : MonoBehaviour
{

    [Header("Character Information")]
    [SerializeField] CharacterSO characterData;
    [SerializeField] CombatEvent combatEvent;
    [SerializeField] public List<SkillSO> skills;
    [SerializeField] List<StatusEffect> activeEffects;

    public string CharacterName => characterData.characterName;
    public bool IsEnemy => characterData.isEnemy;

    SpriteRenderer sr;
    public int currentHealth;
    public int currentMana;
    public int shield = 0;
    public float attackbonus = 1;
    public float critChance = 0.1f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = characterData.sprite;

        if (characterData.flipped)
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

        currentHealth = characterData.baseHealth;
        currentMana = characterData.baseMana;
        shield = characterData.baseShield;
        attackbonus = characterData.baseAttackBonus;
        critChance = characterData.baseCritChance;

        SetupEvents();
    }

    void SetupEvents()
    {
        combatEvent.OnNewTurn.AddListener(TriggerEffects);
    }

    public void TriggerEffects(CombatManager m)
    {
        foreach (var effect in activeEffects.ToList())
        {
            combatEvent.RaiseOnEffectEvent(this, effect);
            effect.TickEffect(this);

            if (effect.IsExpired)
                RemoveStatusEffect(effect);
        }
    }

    public void AddStatusEffect(StatusEffect effect)
    {
        activeEffects.Add(effect);
        effect.ApplyEffect(this);
    }

    public void RemoveStatusEffect(StatusEffect effect)
    {
        activeEffects.Remove(effect);
        effect.RemoveEffect(this);
    }

    public void PerformSkill(List<Character> target, int idx)
    {
        combatEvent.RaiseOnAttackEvent(this, target[0]);
        skills[idx].ExecuteSkill(this, target.ToArray());
    }

    public void PerformSkill(Character target, int idx)
    {
        combatEvent.RaiseOnAttackEvent(this, target);
        skills[idx].ExecuteSkill(this, target);
    }

    public virtual void TakeRawDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            Kill();
        else
            combatEvent.RaiseOnDamageEvent(this);
    }
    public virtual void TakeDamage(Character attacker, int damage)
    {
        if (shield > 0)
        {
            if (shield < damage)
            {
                damage -= shield;
                shield = 0;
            }
            else
            {
                shield -= damage;
                damage = 0;
            }
            combatEvent.RaiseOnShieldDamageEvent(this);
        }
        currentHealth -= damage;

        if (currentHealth <= 0)
            Kill();
        else
            combatEvent.RaiseOnDamageEvent(this);
    }

    void Kill()
    {
        combatEvent.RaiseOnKillEvent(this, this);
    }
}