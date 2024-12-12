using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum TurnState
{
    PLAYER_TURN,
    PLAYER_UNIT_SELECTED,
    PLAYER_SKILL_SELECTED,
    PLAYER_TARGET_SELECTED,
    PLAYER_ACTION_PERFORMING,
    ENEMY_TURN,
    ENEMY_UNIT_SELECTED,
    ENEMY_SKILL_SELECTED,
    ENEMY_TARGET_SELECTED,
    ENEMY_ACTION_PERFORMING,
}

public class CombatManager : MonoBehaviour
{
    [Header("Initilize Channels")]
    public CombatUIChannel uiChannel;
    public MouseChannel mouseChannel;

    [Header("Initilize combat")]
    public CombatEvent combatEvent;
    public ActionQueueManager actionQueueManager;
    public List<Unit> units;
    public List<Unit> playerUnits = new();
    public List<Unit> enemyUnits = new();

    public List<Transform> playerLocations, enemyLocations;

    [Header("Debugging a turn")]
    public TurnState turnState = TurnState.PLAYER_TURN;
    public int currentRound = 1;

    SkillSO selectedSkill;
    List<Unit> selectedTargets = new();

    Unit selectedUnit = null;

    void Start()
    {
        int enemyIdx = 0, playerIdx = 0;
        foreach (var unit in units)
        {
            if (unit.IsEnemy)
            {
                enemyUnits.Add(unit);
                unit.transform.position = enemyLocations[enemyIdx++].transform.position;
            }
            else
            {
                playerUnits.Add(unit);
                unit.transform.position = playerLocations[playerIdx++].transform.position;
            }
        }
        SetupEvents();
    }

    void SetupEvents()
    {
        if (mouseChannel != null)
        {
            mouseChannel.OnUnitHover += HandleUnitHover;
            mouseChannel.OnUnitSelect += HandleUnitSelect;
        }

        if (CombatEvent.Instance != null)
        {
            CombatEvent.Instance.ActionsCompleted += HandleActionsCompleted;
            // CombatEvent.Instance.OnSkill.AddListener(HandleOnSkill);
            CombatEvent.Instance.UnitDeath += HandleOnDeath;
        }

        if (uiChannel != null)
        {
            uiChannel.SkillSelected += HandleSkillSelected;
        }

        uiChannel.OnTurnChange(turnState, null);
        uiChannel.OnNewTurn(currentRound);
    }

    void HandleActionsCompleted()
    {
        switch (turnState)
        {
            case TurnState.PLAYER_ACTION_PERFORMING or TurnState.ENEMY_ACTION_PERFORMING:
                ActionQueueManager.EnqueueDisengageUnitsAction();
                AdvanceTurn();
                break;
        }

    }

    private void HandleOnDeath(Unit arg0, Unit arg1)
    {
        if (arg1.IsEnemy)
        {
            foreach (var c in enemyUnits)
            {
                if (c.IsDead)
                    c.gameObject.SetActive(false);
            }
            enemyUnits.RemoveAll(c => c.IsDead);
        }
        else
        {
            foreach (var c in playerUnits)
            {
                if (c.IsDead)
                    c.gameObject.SetActive(false);
            }
            playerUnits.RemoveAll(c => c.IsDead);
        }
    }

    void HandleSkillSelected(SkillSO skill)
    {
        turnState = TurnState.PLAYER_SKILL_SELECTED;

        selectedSkill = skill;

        switch (selectedSkill.targetType)
        {
            case TargetType.UNIT_ALL:
                List<Unit> allUnits = new(enemyUnits.Count + playerUnits.Count);
                allUnits.AddRange(enemyUnits);
                allUnits.AddRange(playerUnits.FindAll(u => u != selectedUnit));
                ExecuteSelectedSkill(allUnits);
                break;
            case TargetType.SELF:
                ExecuteSelectedSkill(selectedUnit);
                break;
            case TargetType.PLAYER_UNIT_ALL:
                ExecuteSelectedSkill(playerUnits.ToList().FindAll(u => u != selectedUnit));
                break;
            case TargetType.ENEMY_UNIT_ALL:
                ExecuteSelectedSkill(enemyUnits.ToList().FindAll(u => u != selectedUnit));
                break;
        }
    }

    void HandleUnitSelect(Unit unit)
    {
        switch (turnState)
        {
            case TurnState.PLAYER_TURN or TurnState.PLAYER_UNIT_SELECTED:
                HandleMainUnitSelect(unit);
                break;
            case TurnState.PLAYER_SKILL_SELECTED:
                HandleTargetUnitSelect(unit);
                break;
        }
    }

    void HandleMainUnitSelect(Unit unit)
    {
        if (!unit.IsEnemy && unit.HasTurn && unit != selectedUnit)
        {
            uiChannel.OnRemoveSelectors();
            uiChannel.OnUnitSelect(unit);
            turnState = TurnState.PLAYER_UNIT_SELECTED;
            selectedUnit = unit;
            uiChannel.OnAssignSkills(unit.skills);
            uiChannel.OnTurnChange(turnState, new List<Unit> { unit });
        }
    }

    void HandleTargetUnitSelect(Unit unit)
    {
        switch (selectedSkill.targetType)
        {

            case TargetType.ENEMY_UNIT_SINGLE:
                if (unit.IsEnemy)
                    ExecuteSelectedSkill(unit);
                break;

            case TargetType.ENEMY_UNIT_MULTIPLE:
                if (unit.IsEnemy && !selectedTargets.Contains(unit))
                    selectedTargets.Add(unit);

                if (selectedTargets.Count < selectedSkill.numberOfTargets && selectedTargets.Count < enemyUnits.Count)
                    uiChannel.OnUnitSelect(unit);
                else
                    ExecuteSelectedSkill(selectedTargets);
                break;

            case TargetType.PLAYER_UNIT_SINGLE:
                if (!unit.IsEnemy && unit != selectedUnit)
                    ExecuteSelectedSkill(unit);
                break;

            case TargetType.PLAYER_UNIT_MULTIPLE:
                if (!unit.IsEnemy && unit != selectedUnit && !selectedTargets.Contains(unit))
                    selectedTargets.Add(unit);

                if (selectedTargets.Count < selectedSkill.numberOfTargets && selectedTargets.Count < playerUnits.Count)
                    uiChannel.OnUnitSelect(unit);
                else
                    ExecuteSelectedSkill(selectedTargets);
                break;

        }
    }

    void ExecuteSelectedSkill(List<Unit> units)
    {
        ActionQueueManager.EnqueueEngageUnitsAction(selectedUnit, units, !selectedUnit.IsEnemy);
        ActionQueueManager.EnqueueSkillAction(selectedUnit, selectedSkill, units, !selectedUnit.IsEnemy);

        uiChannel.OnRemoveSelectors();
        turnState = TurnState.PLAYER_ACTION_PERFORMING;
    }

    void ExecuteSelectedSkill(Unit unit)
    {
        ActionQueueManager.EnqueueEngageUnitsAction(selectedUnit, new List<Unit>() { unit }, !selectedUnit.IsEnemy);
        ActionQueueManager.EnqueueSkillAction(selectedUnit, selectedSkill, unit, !selectedUnit.IsEnemy);

        uiChannel.OnRemoveSelectors();
        turnState = TurnState.PLAYER_ACTION_PERFORMING;
    }

    void HandleUnitHover(Unit unit)
    {
        switch (turnState)
        {
            case TurnState.PLAYER_TURN or TurnState.PLAYER_UNIT_SELECTED:
                HandleMainUnitHover(unit);
                break;
            case TurnState.PLAYER_SKILL_SELECTED:
                HandleTargetUnitHover(unit);
                break;
        }
    }

    void HandleMainUnitHover(Unit unit)
    {
        if (!unit.IsEnemy && unit.HasTurn && unit != selectedUnit)
        {
            uiChannel.OnUnitHover(unit);
        }
    }

    void HandleTargetUnitHover(Unit unit)
    {
        switch (selectedSkill.targetType)
        {
            case TargetType.PLAYER_UNIT_SINGLE or TargetType.PLAYER_UNIT_MULTIPLE:
                if (!unit.IsEnemy && !selectedTargets.Contains(unit) && unit != selectedUnit)
                    uiChannel.OnUnitHover(unit);
                break;
            case TargetType.ENEMY_UNIT_SINGLE or TargetType.ENEMY_UNIT_MULTIPLE:
                if (unit.IsEnemy && !selectedTargets.Contains(unit))
                    uiChannel.OnUnitHover(unit);
                break;
        }
    }

    void HandleTurnChange()
    {
        switch (turnState)
        {
            case TurnState.ENEMY_TURN:
                var numOfAvailableEnemyUnits = enemyUnits.Count(u => u.HasTurn);

                if (numOfAvailableEnemyUnits == 0)
                {
                    enemyUnits.ForEach(u => u.HasTurn = true);
                }
                break;

            case TurnState.PLAYER_TURN:
                var numOfAvailablePlayerUnits = playerUnits.Count(u => u.HasTurn);

                if (numOfAvailablePlayerUnits == 0)
                {
                    currentRound++;
                    CombatEvent.OnNewTurn(this);
                    uiChannel.OnNewTurn(currentRound);

                    playerUnits.ForEach(u => u.HasTurn = true);
                }
                break;
        }
    }

    public void GettoSkipEnemy()
    {
        turnState = TurnState.PLAYER_TURN;
        uiChannel.OnTurnChange(turnState, playerUnits);
        CombatEvent.OnTurnChanged(this);
        HandleTurnChange();
    }

    void AdvanceTurn()
    {
        switch (turnState)
        {
            case TurnState.PLAYER_ACTION_PERFORMING:
                selectedUnit.HasTurn = false;
                selectedUnit = null;
                selectedTargets.Clear();

                turnState = TurnState.ENEMY_TURN;
                uiChannel.OnTurnChange(turnState, playerUnits);
                CombatEvent.OnTurnChanged(this);
                break;

            case TurnState.ENEMY_ACTION_PERFORMING:
                selectedUnit.HasTurn = false;
                selectedUnit = null;
                selectedTargets.Clear();

                turnState = TurnState.PLAYER_TURN;
                uiChannel.OnTurnChange(turnState, playerUnits);
                CombatEvent.OnTurnChanged(this);
                break;
        }
        HandleTurnChange();
    }
}