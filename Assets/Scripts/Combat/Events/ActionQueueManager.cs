using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ActionQueueManager : MonoBehaviour
{
    Queue<ActionQueueItem> actionQueue = new();
    bool isProcessing = false;

    public static ActionQueueManager Instance { get; private set; }

    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    public static void EnqueueSkillAction(Unit unit, SkillSO skillSO, Unit target)
    {
        if (Instance != null)
        {
            Instance.actionQueue.Enqueue(new SkillAction(unit, skillSO, target));
            Instance.StartQueue();
        }
    }

    public static void EnqueueStatusEffectAction(Unit unit, StatusEffect statusEffect)
    {
        if (Instance != null)
        {
            Instance.actionQueue.Enqueue(new StatusEffectAction(unit, statusEffect));
            Instance.StartQueue();
        }
    }

    public static void EnqueueStatusEffectAction(Unit unit, StatusEffect statusEffect, StatusEffectAction.EffectAction effectAction)
    {
        if (Instance != null)
        {
            Instance.actionQueue.Enqueue(new StatusEffectAction(unit, statusEffect, effectAction));
            Instance.StartQueue();
        }
    }

    public static void EnqueueDamageAction(Unit unit, int damage)
    {
        if (Instance != null)
        {
            Instance.actionQueue.Enqueue(new DamageAction(unit, damage));
            Instance.StartQueue();
        }
    }

    public static void EnqueueDeathAction(Unit killer, Unit victim)
    {
        if (Instance != null)
        {
            Instance.actionQueue.Enqueue(new DeathAction(killer, victim));
            Instance.StartQueue();
        }
    }

    void StartQueue()
    {
        if (!isProcessing)
            StartCoroutine(ProcessQueue());
    }

    IEnumerator ProcessQueue()
    {
        isProcessing = true;

        while (actionQueue.Count > 0)
        {
            ActionQueueItem actionItem = actionQueue.Dequeue();
            yield return StartCoroutine(actionItem.ExecuteAction());
        }

        isProcessing = false;
    }
}

public abstract class ActionQueueItem
{
    public abstract IEnumerator ExecuteAction();
}

public class SkillAction : ActionQueueItem
{
    public Unit unit;
    public SkillSO skill;
    public Unit target;
    public SkillAction(Unit unit, SkillSO skill, Unit target)
    {
        this.unit = unit;
        this.skill = skill;
        this.target = target;
    }
    public override IEnumerator ExecuteAction()
    {
        yield return unit.StartCoroutine(unit.AnimateAction(skill));
        skill.ExecuteSkill(unit, target);
        CombatEvent.Instance.RaiseOnSkillEvent(unit, skill, target);
        yield return null;
    }
}

public class StatusEffectAction : ActionQueueItem
{
    public enum EffectAction
    {
        APPLY,
        REMOVE,
        TICK
    };

    Unit unit;
    StatusEffect statusEffect;
    EffectAction effectAction;

    public StatusEffectAction(Unit unit, StatusEffect statusEffect)
    {
        this.unit = unit;
        this.statusEffect = statusEffect;
        this.effectAction = EffectAction.APPLY;
    }

    public StatusEffectAction(Unit unit, StatusEffect statusEffect, EffectAction effectAction)
    {
        this.unit = unit;
        this.statusEffect = statusEffect;
        this.effectAction = effectAction;
    }

    public override IEnumerator ExecuteAction()
    {
        switch (effectAction)
        {
            case EffectAction.APPLY:
                unit.AddStatusEffect(statusEffect);
                statusEffect.ApplyEffect(unit);
                break;
            case EffectAction.REMOVE:
                unit.RemoveStatusEffect(statusEffect);
                statusEffect.RemoveEffect(unit);
                break;
            case EffectAction.TICK:
                statusEffect.TickEffect(unit);
                break;
        }
        CombatEvent.Instance.RaiseOnEffectEvent(unit, statusEffect);
        yield return null;
    }
}

public class DamageAction : ActionQueueItem
{
    Unit unit;
    int damage;

    public DamageAction(Unit unit, int damage)
    {
        this.unit = unit;
        this.damage = damage;
    }

    public override IEnumerator ExecuteAction()
    {
        yield return unit.StartCoroutine(unit.AnimateAction(true));
        var (actualDamage, shieldLeft) = unit.CalculateDamage(damage);
        if (unit.Shield != 0)
        {
            unit.Shield = shieldLeft;
            CombatEvent.Instance.RaiseOnShieldDamageEvent(unit, actualDamage);
        }
        unit.TakeRawDamage(unit, actualDamage);
        CombatEvent.Instance.RaiseOnDamageEvent(unit, actualDamage);
        yield return null;
    }
}

public class DeathAction : ActionQueueItem
{
    Unit attacker;
    Unit victim;

    public DeathAction(Unit attacker, Unit victim)
    {
        this.attacker = attacker;
        this.victim = victim;
    }

    public override IEnumerator ExecuteAction()
    {
        victim.isDead = true;
        CombatEvent.Instance.RaiseOnDeathEvent(attacker, victim);
        yield return null;
    }
}