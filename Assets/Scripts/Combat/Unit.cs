using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{

    [Header("Unit Information")]
    [SerializeField] UnitSO unitData;
    [SerializeField] public List<SkillSO> skills;
    [SerializeField] public List<PassiveSO> passives;
    [SerializeField] public List<StatusEffect> activeEffects;
    [SerializeField] Transform rangedAttackSpawn;
    [SerializeField] GameObject projectile;
    [SerializeField] float rangeAttackDistance = 4f;
    [SerializeField] float rangeAttackDuration = 0.2f;

    public bool AnimationFinished { get; private set; }
    public string UnitName => unitData.name;
    public bool IsEnemy => unitData.isEnemy;
    public int Shield { get; set; }
    public bool HasTurn { get; set; }
    public bool IsDead { get; set; }
    // public int CurrentHealth { get ; private set; }
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }
    public int HealthRegen { get; private set; }
    public int CurrentMana { get; private set; }
    public int MaxMana { get; private set; }
    public int ManaRegen { get; private set; }
    public float AttackBonus { get; set; }
    public float CritChance { get; set; }

    protected SpriteRenderer sr;
    protected Animator anim;

    void InitilizeUnit(Character character)
    {
        var stats = character.currentStats;

        CurrentHealth = stats["health"];
        MaxHealth = CurrentHealth;
        HealthRegen = stats["healthRegeneration"];

        CurrentMana = stats["mana"];
        MaxMana = CurrentMana;
        ManaRegen = stats["manaRegeneration"];
        Shield = stats["shield"];
        CritChance = stats["criticalChance"] / 100;
        AttackBonus = stats["attackBonus"]/100;
        unitData = character.stats;
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (unitData.flipped)
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

        // CurrentHealth = unitData.health;
        // MaxHealth = CurrentHealth;
        // CurrentMana = unitData.mana;
        // MaxMana = CurrentMana;
        // Shield = unitData.shield;
        // critChance = unitData.baseCritChance;

        HasTurn = true;
        AnimationFinished = true;

        SetupEvents();
    }

    void SetupEvents()
    {
        if (CombatEvent.Instance != null)
            CombatEvent.Instance.NewTurn += TriggerEffects;

        foreach (var passive in passives)
        {
            passive.SubscribeToEvent(null);
        }
    }

    void OnDisable()
    {
        foreach (var passive in passives)
        {
            passive.UnsubscribeToEvent(null);
        }
    }


    public void TriggerEffects(CombatManager m)
    {
        foreach (var effect in activeEffects.ToList())
        {
            Debug.Log("TICKING EFFECT " + effect.name + " : " + effect.duration);
            ActionQueueManager.EnqueueStatusEffectAction(this, effect, StatusEffectAction.ActionType.TICK);

            if (effect.IsExpired)
            {
                Debug.Log("REMOVING " + effect.name);
                ActionQueueManager.EnqueueStatusEffectAction(this, effect, StatusEffectAction.ActionType.REMOVE);
            }
        }
    }

    public void AddStatusEffect(StatusEffect effect)
    {
        activeEffects.Add(effect);
    }

    public void RemoveStatusEffect(StatusEffect effect)
    {
        activeEffects.Remove(effect);
    }

    public void TakeDamage(Unit attacker, int damage)
    {
        int damageLeft = DamageShield(damage);
        if (damageLeft == 0)
            return;

        TakeRawDamage(attacker, damageLeft);
    }

    int DamageShield(int damage)
    {
        if (Shield > 0)
        {
            if (Shield < damage)
            {
                CombatEvent.OnUnitShieldDamage(this, Shield);
                damage -= Shield;
                Shield = 0;
            }
            else
            {
                CombatEvent.OnUnitShieldDamage(this, damage);
                Shield -= damage;
                damage = 0;
            }
        }
        return damage;
    }

    public void TakeRawDamage(Unit attacker, int damage)
    {
        CombatEvent.OnUnitDamage(this, damage);
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
            Kill(attacker);
    }

    private void Kill(Unit attacker)
    {
        ActionQueueManager.EnqueueDeathAction(attacker, this);
    }

    public void Heal(int amount)
    {
        CombatEvent.OnUnitHeal(this, amount);
        if (CurrentHealth + amount > unitData.health)
            CurrentHealth = unitData.health;
        else
            CurrentHealth += amount;
    }

    public IEnumerator AnimateAction(SkillSO skill)
    {
        AnimationFinished = false;

        yield return Helper.WaitForAnimation(anim, 0, skill.animationName);

        anim.Play("Idle");

        AnimationFinished = true;
    }

    public IEnumerator AnimateRangedProjectile()
    {
        AnimationFinished = false;

        Vector3 direction;
        if (IsEnemy)
            direction = new(-1, 0);
        else
            direction = new(1, 0);

        var p = Instantiate(projectile, this.transform.position, projectile.transform.rotation);

        yield return Helper.MoveObject(
            rangedAttackSpawn.position,
            rangedAttackSpawn.position + direction * rangeAttackDistance,
            p.transform, rangeAttackDuration);

        Destroy(p);

        AnimationFinished = true;
    }

    public IEnumerator AnimateHit()
    {
        AnimationFinished = false;

        yield return ParticleManager.TriggerHitEffect(gameObject);

        if (unitData.hasHitAnimation)
        {
            yield return Helper.WaitForAnimation(anim, 0, "Hit");
            anim.Play("Idle");
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, -16);
            yield return new WaitForSeconds(0.5f);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }


        AnimationFinished = true;
        yield return null;
    }
}