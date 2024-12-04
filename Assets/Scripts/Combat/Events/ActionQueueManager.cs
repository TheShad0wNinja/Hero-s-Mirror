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

    public static void EnqueueSkillAction(Unit character, SkillSO skillSO, Unit target)
    {
        if (Instance != null)
        {
            Instance.actionQueue.Enqueue(new SkillAction(character, skillSO, target));
            Instance.StartQueue();
        }
    }

    public static void EnqueueStatusEffectAction(Unit character, StatusEffect statusEffect)
    {
        if (Instance != null)
        {
            Instance.actionQueue.Enqueue(new StatusEffectAction(character, statusEffect));
            Instance.StartQueue();
        }
    }

    public static void EnqueueStatusEffectAction(Unit character, StatusEffect statusEffect, StatusEffectAction.EffectAction effectAction)
    {
        if (Instance != null)
        {
            Instance.actionQueue.Enqueue(new StatusEffectAction(character, statusEffect, effectAction));
            Instance.StartQueue();
        }
    }

    public static void EnqueueDamageAction(Unit character, int damage)
    {
        if (Instance != null)
        {
            Instance.actionQueue.Enqueue(new DamageAction(character, damage));
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
    public Unit character;
    public SkillSO skill;
    public Unit target;
    public SkillAction(Unit character, SkillSO skill, Unit target)
    {
        this.character = character;
        this.skill = skill;
        this.target = target;
    }
    public override IEnumerator ExecuteAction()
    {
        yield return character.StartCoroutine(character.AnimateAction(skill));
        skill.ExecuteSkill(character, target);
        CombatEvent.Instance.RaiseOnSkillEvent(character, skill, target);
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

    Unit character;
    StatusEffect statusEffect;
    EffectAction effectAction;

    public StatusEffectAction(Unit character, StatusEffect statusEffect)
    {
        this.character = character;
        this.statusEffect = statusEffect;
        this.effectAction = EffectAction.APPLY;
    }

    public StatusEffectAction(Unit character, StatusEffect statusEffect, EffectAction effectAction)
    {
        this.character = character;
        this.statusEffect = statusEffect;
        this.effectAction = effectAction;
    }

    public override IEnumerator ExecuteAction()
    {
        switch (effectAction)
        {
            case EffectAction.APPLY:
                character.AddStatusEffect(statusEffect);
                statusEffect.ApplyEffect(character);
                break;
            case EffectAction.REMOVE:
                character.RemoveStatusEffect(statusEffect);
                statusEffect.RemoveEffect(character);
                break;
            case EffectAction.TICK:
                statusEffect.TickEffect(character);
                break;
        }
        CombatEvent.Instance.RaiseOnEffectEvent(character, statusEffect);
        yield return null;
    }
}

public class DamageAction : ActionQueueItem
{
    Unit character;
    int damage;

    public DamageAction(Unit character, int damage)
    {
        this.character = character;
        this.damage = damage;
    }

    public override IEnumerator ExecuteAction()
    {
        yield return character.StartCoroutine(character.AnimateAction(true));
        var (actualDamage, shieldLeft) = character.CalculateDamage(damage);
        if (character.Shield != 0)
        {
            character.Shield = shieldLeft;
            CombatEvent.Instance.RaiseOnShieldDamageEvent(character, actualDamage);
        }
        character.TakeRawDamage(character, actualDamage);
        CombatEvent.Instance.RaiseOnDamageEvent(character, actualDamage);
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