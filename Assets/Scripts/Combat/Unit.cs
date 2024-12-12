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
    public string UnitName => unitData.unitName;
    public bool IsEnemy => unitData.isEnemy;
    public int Shield { get; set; }
    public bool HasTurn { get; set; }
    public bool IsDead { get; set; }

    SpriteRenderer sr;
    Animator anim;
    int currentHealth;
    int currentMana;
    float attackbonus = 1;
    float critChance = 0.1f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        if (unitData.sprite != null)
            sr.sprite = unitData.sprite;

        if (unitData.flipped)
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

        currentHealth = unitData.baseHealth;
        currentMana = unitData.baseMana;
        Shield = unitData.baseShield;
        attackbonus = unitData.baseAttackBonus;
        critChance = unitData.baseCritChance;

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
        currentHealth -= damage;
        if (currentHealth <= 0)
            Kill(attacker);
    }

    private void Kill(Unit attacker)
    {
        ActionQueueManager.EnqueueDeathAction(attacker, this);
    }

    public void Heal(int amount)
    {
        if (currentHealth + amount > unitData.baseHealth)
            currentHealth = unitData.baseHealth;
        else
            currentHealth += amount;
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

    public IEnumerator AnimateAction(bool hit)
    {
        Debug.Log("ANIMATING HIT " + this.name);
        AnimationFinished = false;
        if (hit)
        {
            transform.rotation = Quaternion.Euler(0, 0, -16);
            yield return new WaitForSeconds(0.5f);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        AnimationFinished = true;
        yield return null;
    }
}