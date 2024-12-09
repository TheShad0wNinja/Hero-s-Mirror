using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public static void EnqueueSkillAction(Unit unit, SkillSO skillSO, List<Unit> targets)
    {
        if (Instance != null)
        {
            Instance.actionQueue.Enqueue(new SkillAction(unit, skillSO, targets));
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
            yield return actionItem.ExecuteAction();
        }

        isProcessing = false;
        CombatEvent.OnActionsCompleted();
    }
}

public abstract class ActionQueueItem
{
    public abstract IEnumerator ExecuteAction();
}

public class SkillAction : ActionQueueItem
{
    Unit unit;
    SkillSO skill;
    Unit target;
    List<Unit> targets;
    bool isMultipleTargets;
    public SkillAction(Unit unit, SkillSO skill, Unit target)
    {
        this.unit = unit;
        this.skill = skill;
        this.target = target;
        isMultipleTargets = false;
    }

    public SkillAction(Unit unit, SkillSO skill, List<Unit> targets)
    {
        this.unit = unit;
        this.skill = skill;
        this.targets = targets;
        isMultipleTargets = true;
    }

    public override IEnumerator ExecuteAction()
    {
        // CombatCameraManager.SwitchCamera();
        CombatCameraManager.SwitchCamera();
        if (isMultipleTargets)
        {
            List<Unit> localTargetsList = targets.ToList();
            if (skill.attackEachTarget == false)
            {
                yield return unit.AnimateAction(skill);

                foreach (var target in localTargetsList)
                {
                    skill.ExecuteSkill(unit, target);
                    CombatEvent.OnSkillPerformed(unit, skill, target);
                    target.StartCoroutine(target.AnimateAction(true));
                }
                yield return new WaitUntil(() => localTargetsList.All(t => t.animationFinished));
            }
            else
            {
                foreach (Unit target in localTargetsList)
                {
                    ActionQueueManager.EnqueueSkillAction(unit, skill, target);
                }
            }
        }
        else
        {
            yield return unit.AnimateAction(skill);
            skill.ExecuteSkill(unit, target);
            CombatEvent.OnSkillPerformed(unit, skill, target);
            yield return target.AnimateAction(true);
        }

        yield return new WaitForSeconds(1f);
        CombatCameraManager.SwitchCamera();
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
        CombatEvent.Instance.OnUnitStatusEffect(unit, statusEffect);
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
            CombatEvent.OnUnitShieldDamage(unit, actualDamage);
        }
        unit.TakeRawDamage(unit, actualDamage);
        CombatEvent.OnUnitDamage(unit, actualDamage);
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
        CombatEvent.OnUnitDeath(attacker, victim);
        yield return null;
    }
}