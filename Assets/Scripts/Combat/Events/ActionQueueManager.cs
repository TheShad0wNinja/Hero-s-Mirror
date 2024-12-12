using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ActionQueueManager : MonoBehaviour
{
    LinkedList<ActionQueueItem> actionQueue = new();
    bool isProcessing = false;

    int parallelizationAttempts = 0;
    const int parallelizationMaxAttempts = 3;
    public bool hasParallel = false;
    public Type parallelItemType = null;
    List<ActionQueueItem> parallelItems = new();

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
            Instance.actionQueue.AddLast(new DamageAction(attacker, victim, damage));
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
            if (hasParallel)
            {
                if (actionItem.GetType() == parallelItemType)
                {
                    parallelItems.Add(actionItem);
                    continue;
                }

                bool itemExistsInQueue = actionQueue.Any(it => it.GetType() == parallelItemType);

                if (parallelItems.Count > 0 && !itemExistsInQueue)
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
                    hasParallel = false;
                    parallelItemType = null;
                    parallelizationAttempts = 0;
                }
            }
            else
            {
                yield return actionItem.ExecuteAction();
            }
        }

        if (hasParallel)
        {
            yield return ProcessParallelItems();
        }

        isProcessing = false;
        CombatEvent.OnActionsCompleted();
    }

    IEnumerator ProcessParallelItems()
    {
        hasParallel = false;
        foreach (var item in parallelItems)
            StartCoroutine(item.ExecuteAction());

        yield return new WaitUntil(() => parallelItems.All(i => i.hasFinished));

        parallelItems.Clear();
        parallelItemType = null;
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
        hasFinished = true;
        CombatCameraManager.SwitchToDefaultCamera();
        yield return CombatActionMovement.Instance.DisengageUnits();
        hasFinished = false;
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
        CombatCameraManager.SwitchCamera();
        if (skill.targetType == TargetType.UNIT_ALL && skill is RandomSkill randomSkill)
        {
            List<Unit> localTargetsList = targets.ToList();

            var (chosenSkill, targetList) = randomSkill.GetFate(localTargetsList);

            ActionQueueManager.EnqueueSkillAction(unit, chosenSkill, targetList);
        }
        else if (targets != null)
        {
            List<Unit> localTargetsList = targets.ToList();
            if (skill.animationName != "")
                yield return unit.AnimateAction(skill);
            else
                yield return new WaitForSeconds(0.2f);

            // localTargetsList.ForEach(u => skill.ExecuteSkill(unit, target));

            // ActionQueueManager.EnqueueParallelDamage(unit, localTargetsList);
            // ActionQueueManager.Instance.hasParallel = true;
            // ActionQueueManager.Instance.parallelItem = typeof(DamageAction);
            skill.ExecuteSkill(unit, localTargetsList.ToArray());
            // localTargetsList.ForEach(t => skill.ExecuteSkill(unit, t));

            // skill.ExecuteSkill(unit, localTargetsList.ToArray());

            // yield return CombatActionMovement.Instance.EngageUnits(unit, targets, isPlayerAction);
            // if (skill.animationName != "")
            //     yield return unit.AnimateAction(skill);

            // foreach (var target in localTargetsList)
            // {
            //     skill.ExecuteSkill(unit, target);
            //     CombatEvent.OnSkillPerformed(unit, skill, target);

            //     if (skill.isOffensive)
            //         target.StartCoroutine(target.AnimateAction(true));
            // }

            // if (skill.isOffensive)
            //     yield return new WaitUntil(() => localTargetsList.All(t => t.AnimationFinished));
            // else
            //     yield return new WaitForSeconds(0.4f);
        }
        else
        {
            if (skill.animationName != "")
                yield return unit.AnimateAction(skill);
            else
                yield return new WaitForSeconds(0.2f);

            skill.ExecuteSkill(unit, target);
            // CombatEvent.OnSkillPerformed(unit, skill, target);
            // yield return CombatActionMovement.Instance.EngageUnits(unit, new List<Unit>() { target }, isPlayerAction);

            // // yield return unit.AnimateRangedAction(skill, rangedAttack.projectile);
            // // else if (skill.animationName != "")

            // skill.ExecuteSkill(unit, target);
            // CombatEvent.OnSkillPerformed(unit, skill, target);

            // if (skill.isOffensive)
            //     yield return target.AnimateAction(true);
            // else
            //     yield return new WaitForSeconds(0.4f);
        }

        // CombatCameraManager.SwitchCamera();
        // yield return CombatActionMovement.Instance.DisengageUnits();
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

    public DamageAction()
    { }
    public DamageAction(Unit attacker, Unit victim, int damage)
    {
        this.attacker = attacker;
        this.victim = victim;
        this.damage = damage;
    }

    public override IEnumerator ExecuteAction()
    {
        hasFinished = false;
        Debug.Log($"BEFORE HIT {victim}");
        victim.TakeDamage(attacker, damage);
        yield return victim.AnimateAction(true);
        Debug.Log($"AFTER HIT {victim}");
        // var (actualDamage, shieldLeft) = unit.CalculateDamage(damage);
        // if (unit.Shield != 0)
        // {
        //     unit.Shield = shieldLeft;
        //     CombatEvent.OnUnitShieldDamage(unit, actualDamage);
        // }
        // unit.TakeRawDamage(unit, actualDamage);
        // CombatEvent.OnUnitDamage(unit, actualDamage);
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