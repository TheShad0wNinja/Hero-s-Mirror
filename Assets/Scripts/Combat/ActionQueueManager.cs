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

    public void EnqueueAction(Character character, SkillSO skillSO, Character target)
    {
        actionQueue.Enqueue(new SkillAction(character, skillSO, target));
        if (!isProcessing)
            StartCoroutine(ProcessQueue());
    }

    public void EnqueueAction(Character character, StatusEffect statusEffect)
    {
        actionQueue.Enqueue(new StatusEffectAction(character, statusEffect));
        CheckQueue();
    }

    public void EnqueueAction(Character character, StatusEffect statusEffect, StatusEffectAction.EffectAction effectAction)
    {
        actionQueue.Enqueue(new StatusEffectAction(character, statusEffect, effectAction));
        CheckQueue();
    }

    public void EnqueueAction(Character character, int damage)
    {
        actionQueue.Enqueue(new DamageAction(character, damage));
        CheckQueue();
    }

    public void EnqueueAction(Character killer, Character victim)
    {
        actionQueue.Enqueue(new DeathAction(killer, victim));
        CheckQueue();
    }

    void CheckQueue()
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
    public Character character;
    public SkillSO skill;
    public Character target;
    public SkillAction(Character character, SkillSO skill, Character target)
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

    Character character;
    StatusEffect statusEffect;
    EffectAction effectAction;

    public StatusEffectAction(Character character, StatusEffect statusEffect)
    {
        this.character = character;
        this.statusEffect = statusEffect;
        this.effectAction = EffectAction.APPLY;
    }

    public StatusEffectAction(Character character, StatusEffect statusEffect, EffectAction effectAction)
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
    Character character;
    int damage;

    public DamageAction(Character character, int damage)
    {
        this.character = character;
        this.damage = damage;
    }

    public override IEnumerator ExecuteAction()
    {
        yield return character.StartCoroutine(character.AnimateAction(true));
        var (actualDamage, shieldLeft) = character.CalculateDamage(damage);
        if (character.Shield != 0) {
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
    Character attacker;
    Character victim;

    public DeathAction(Character attacker, Character victim)
    {
        this.attacker = attacker;
        this.victim = victim;
    }

    public override IEnumerator ExecuteAction()
    {
        // UnityEngine.Object.Destroy(victim.gameObject);
        victim.isDead = true;
        CombatEvent.Instance.RaiseOnDeathEvent(attacker, victim);
        yield return null;
    }
}