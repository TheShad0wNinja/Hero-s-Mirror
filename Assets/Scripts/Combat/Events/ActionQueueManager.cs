using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ActionQueueManager : MonoBehaviour
{
    public static ActionQueueManager Instance { get; private set; }

    LinkedList<ActionQueueItem> actionQueue = new();
    bool isProcessing = false;

    // Parallelization Variables
    const int parallelizationMaxAttempts = 3;
    int parallelizationAttempts = 0;
    public bool hasParallelProcess = false;
    List<Type> parallelItemTypes = new();
    List<ActionQueueItem> currParallelItems = new();


    void Start()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    public static void EnqueueParallelType(params Type[] types)
    {
        if (Instance != null)
        {
            foreach (var type in types)
                Instance.parallelItemTypes.Add(type);
        }
    }

    public static void EnqueueEngageUnitsAction(Unit selectedUnit, List<Unit> targets, bool isPlayerAction)
    {
        if (Instance != null)
        {
            Instance.actionQueue.AddLast(new EngageUnitsAction(selectedUnit, targets, isPlayerAction));
            Instance.StartQueue();
        }
    }

    public static void EnqueueDisengageUnitsAction()
    {
        if (Instance != null)
        {
            Instance.actionQueue.AddLast(new DisengageUnitsAction());
            Instance.StartQueue();
        }
    }

    public static void EnqueueSkillAction(Unit unit, SkillSO skillSO, Unit target)
    {
        if (Instance != null)
        {
            Instance.actionQueue.AddLast(new SkillAction(unit, skillSO, target, true));
            Instance.StartQueue();
        }
    }

    public static void EnqueueSkillAction(Unit unit, SkillSO skillSO, List<Unit> targets)
    {
        if (Instance != null)
        {
            Instance.actionQueue.AddLast(new SkillAction(unit, skillSO, targets, true));
            Instance.StartQueue();
        }
    }

    public static void EnqueueSkillAction(Unit unit, SkillSO skillSO, Unit target, bool isPlayerAction)
    {
        if (Instance != null)
        {
            Instance.actionQueue.AddLast(new SkillAction(unit, skillSO, target, isPlayerAction));
            Instance.StartQueue();
        }
    }

    public static void EnqueueSkillAction(Unit unit, SkillSO skillSO, List<Unit> targets, bool isPlayerAction)
    {
        if (Instance != null)
        {
            Instance.actionQueue.AddLast(new SkillAction(unit, skillSO, targets, isPlayerAction));
            Instance.StartQueue();
        }
    }

    public static void EnqueueStatusEffectAction(Unit unit, StatusEffect statusEffect)
    {
        if (Instance != null)
        {
            Instance.actionQueue.AddLast(new StatusEffectAction(unit, statusEffect));
            Instance.StartQueue();
        }
    }

    public static void EnqueueStatusEffectAction(Unit unit, StatusEffect statusEffect, StatusEffectAction.ActionType effectAction)
    {
        if (Instance != null)
        {
            Instance.actionQueue.AddLast(new StatusEffectAction(unit, statusEffect, effectAction));
            Instance.StartQueue();
        }
    }

    public static void EnqueueDamageAction(Unit attacker, Unit victim, int damage)
    {
        if (Instance != null)
        {
            Instance.actionQueue.AddLast(new DamageAction(attacker, victim, damage, false));
            Instance.StartQueue();
        }
    }

    public static void EnqueueRawDamageAction(Unit attacker, Unit victim, int damage)
    {
        if (Instance != null)
        {
            Instance.actionQueue.AddLast(new DamageAction(attacker, victim, damage, true));
            Instance.StartQueue();
        }
    }

    public static void EnqueueDeathAction(Unit killer, Unit victim)
    {
        if (Instance != null)
        {
            Instance.actionQueue.AddLast(new DeathAction(killer, victim));
            Instance.StartQueue();
        }
    }

    public static void EnqueueHealAction(Unit unit, int healAmount)
    {
        if (Instance != null)
        {
            Instance.actionQueue.AddLast(new HealAction(unit, healAmount));
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
            ActionQueueItem actionItem = actionQueue.First.Value;
            actionQueue.RemoveFirst();

            Debug.Log($"New Action: {actionItem}");
            if (hasParallelProcess)
            {
                if (parallelItemTypes.Contains(actionItem.GetType()))
                {
                    currParallelItems.Add(actionItem);
                    continue;
                }

                bool itemExistsInQueue = actionQueue.Any(it => parallelItemTypes.Contains(actionItem.GetType()));

                if (currParallelItems.Count > 0 && !itemExistsInQueue)
                {
                    actionQueue.AddFirst(actionItem);
                    yield return ProcessParallelItems();
                }
                else if (itemExistsInQueue)
                {
                    actionQueue.AddAfter(actionQueue.First, actionItem);
                }
                else if (parallelizationAttempts < parallelizationMaxAttempts)
                {
                    parallelizationAttempts++;
                    actionQueue.AddFirst(actionItem);
                }
                else
                {
                    actionQueue.AddFirst(actionItem);
                    currParallelItems.Clear();
                    parallelItemTypes.Clear();
                    hasParallelProcess = false;

                    parallelizationAttempts = 0;
                }
            }
            else
            {
                yield return actionItem.ExecuteAction();
            }
        }

        if (hasParallelProcess)
        {
            yield return ProcessParallelItems();
        }

        isProcessing = false;
        CombatEvent.OnActionsCompleted();
    }

    IEnumerator ProcessParallelItems()
    {
        foreach (var item in currParallelItems)
            StartCoroutine(item.ExecuteAction());

        yield return new WaitUntil(() => currParallelItems.All(i => i.hasFinished));

        currParallelItems.Clear();
        parallelItemTypes.Clear();
        hasParallelProcess = false;

        yield return null;
    }

}


public abstract class ActionQueueItem
{
    public bool hasFinished = false;
    public abstract IEnumerator ExecuteAction();
}

public class EngageUnitsAction : ActionQueueItem
{
    Unit selectedUnit;
    List<Unit> units;
    bool isPlayerAction;
    public EngageUnitsAction(Unit selectedUnit, List<Unit> units, bool isPlayerAction)
    {
        this.selectedUnit = selectedUnit;
        this.units = units;
        this.isPlayerAction = isPlayerAction;
    }

    public override IEnumerator ExecuteAction()
    {
        hasFinished = false;
        CombatCameraManager.SwitchToActionCamera();
        yield return CombatActionMovement.Instance.EngageUnits(selectedUnit, units, isPlayerAction);
        hasFinished = true;
    }
}

public class DisengageUnitsAction : ActionQueueItem
{
    public override IEnumerator ExecuteAction()
    {
        hasFinished = false;
        yield return new WaitForSeconds(0.5f);
        yield return CombatActionMovement.Instance.DisengageUnits();
        CombatCameraManager.SwitchToDefaultCamera();
        hasFinished = true;
    }
}

public class SkillAction : ActionQueueItem
{
    Unit unit;
    SkillSO skill;
    Unit target;
    List<Unit> targets;
    bool isPlayerAction;

    public SkillAction(Unit unit, SkillSO skill, Unit target, bool isPlayerAction)
    {
        this.unit = unit;
        this.skill = skill;
        this.target = target;
        this.isPlayerAction = isPlayerAction;
    }

    public SkillAction(Unit unit, SkillSO skill, List<Unit> targets, bool isPlayerAction)
    {
        this.unit = unit;
        this.skill = skill;
        this.targets = targets;
        this.isPlayerAction = isPlayerAction;
    }

    public override IEnumerator ExecuteAction()
    {
        hasFinished = false;
        if (skill.targetType == TargetType.UNIT_ALL && skill is RandomSkill randomSkill)
        {
            List<Unit> localTargetsList = targets.ToList();

            var (chosenSkill, targetList) = randomSkill.GetFate(localTargetsList);

            ActionQueueManager.EnqueueEngageUnitsAction(unit, targetList, isPlayerAction);

            ActionQueueManager.EnqueueSkillAction(unit, chosenSkill, targetList);
        }
        else if (targets != null)
        {
            List<Unit> localTargetsList = targets.ToList();
            if (skill.animationName != "")
            {
                if (skill.hasEarlyAnimationFinish)
                {
                    unit.StartCoroutine(unit.AnimateAction(skill));
                    yield return new WaitUntil(() => unit.AnimationFinished);
                }
                else
                {
                    yield return unit.AnimateAction(skill);
                }
            }

            skill.ExecuteSkill(unit, localTargetsList.ToArray());
        }
        else
        {
            if (skill.animationName != "")
            {
                if (skill.hasEarlyAnimationFinish)
                {
                    unit.StartCoroutine(unit.AnimateAction(skill));
                    yield return new WaitUntil(() => unit.AnimationFinished);
                }
                else
                {
                    yield return unit.AnimateAction(skill);
                }
            }
            else
                yield return new WaitForSeconds(1f);

            skill.ExecuteSkill(unit, target);
        }

        hasFinished = true;
        yield return null;
    }
}

public class StatusEffectAction : ActionQueueItem
{
    public enum ActionType
    {
        APPLY,
        REMOVE,
        TICK
    };

    Unit unit;
    StatusEffect statusEffect;
    ActionType effectAction;

    public StatusEffectAction(Unit unit, StatusEffect statusEffect)
    {
        this.unit = unit;
        this.statusEffect = statusEffect;
        this.effectAction = ActionType.APPLY;
    }

    public StatusEffectAction(Unit unit, StatusEffect statusEffect, ActionType effectAction)
    {
        this.unit = unit;
        this.statusEffect = statusEffect;
        this.effectAction = effectAction;
    }

    public override IEnumerator ExecuteAction()
    {
        hasFinished = false;
        switch (effectAction)
        {
            case ActionType.APPLY:
                unit.AddStatusEffect(statusEffect);
                statusEffect.ApplyEffect(unit);
                break;
            case ActionType.REMOVE:
                unit.RemoveStatusEffect(statusEffect);
                statusEffect.RemoveEffect(unit);
                break;
            case ActionType.TICK:
                statusEffect.TickEffect(unit);
                break;
        }
        CombatEvent.Instance.OnUnitStatusEffect(unit, statusEffect, effectAction);
        hasFinished = true;
        yield return null;
    }
}

public class DamageAction : ActionQueueItem
{
    Unit attacker, victim;
    int damage;
    bool isRawDamage;

    public DamageAction(Unit attacker, Unit victim, int damage, bool isRawDamage)
    {
        this.attacker = attacker;
        this.victim = victim;
        this.damage = damage;
        this.isRawDamage = isRawDamage;
    }

    public override IEnumerator ExecuteAction()
    {
        hasFinished = false;

        Debug.Log($"BEFORE HIT {victim}");
        if (isRawDamage)
            victim.TakeRawDamage(attacker, damage);
        else
            victim.TakeDamage(attacker, damage);
        yield return victim.AnimateHit();
        Debug.Log($"AFTER HIT {victim}");

        hasFinished = true;
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
        hasFinished = false;
        victim.IsDead = true;
        CombatEvent.OnUnitDeath(attacker, victim);
        hasFinished = true;
        yield return null;
    }
}

public class HealAction : ActionQueueItem
{
    Unit unit;
    int healAmount;

    public HealAction(Unit unit, int healAmount)
    {
        this.unit = unit;
        this.healAmount = healAmount;
    }

    public override IEnumerator ExecuteAction()
    {
        hasFinished = false;
        unit.Heal(healAmount);
        hasFinished = true;
        yield return null;
    }
}