using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public enum CurrentTurn
{
    PLAYER_TURN,
    PLAYER_UNIT_SELECTED,
    PLAYER_SKILL_SELECTED,
    PLAYER_TARGET_SELECTED,
    ENEMY_TURN,
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
    public int selectedSkillIdx = 0;
    public bool triggerSkill = false;
    public bool triggerEffects = false;
    public bool triggerPassives = false;

    public CurrentTurn currentTurn = CurrentTurn.PLAYER_TURN;
    public int selectedTargetIdx = 0;
    public int currentTurnNumber = 1;

    SkillSO selectedSkill;
    Unit selectedTarget;
    Unit _selectedUnit;
    public Unit SelectedUnit
    {
        get => _selectedUnit;
        set
        {
            _selectedUnit = value;
            if (_selectedUnit != null)
                uiChannel.RaiseOnAssignSkills(_selectedUnit.skills);
        }
    }

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
        HandleTurn();

        if (mouseChannel != null)
        {
            mouseChannel.OnUnitHover += HandleUnitHover;
            mouseChannel.OnUnitSelect += HandleUnitSelect;
        }

        if (CombatEvent.Instance != null)
        {
            CombatEvent.Instance.OnSkill.AddListener(HandleOnSkill);
            CombatEvent.Instance.OnDeath.AddListener(HandleOnDeath);
        }

        if (uiChannel != null)
        {
            uiChannel.OnSkillSelected += HandleSkillSelected;
        }
    }

    private void HandleOnDeath(Unit arg0, Unit arg1)
    {
        if (arg1.IsEnemy)
        {
            foreach (var c in enemyUnits)
            {
                if (c.isDead)
                    Destroy(c.gameObject);
            }
            enemyUnits.RemoveAll(c => c.isDead);
        }
        else
        {
            foreach (var c in playerUnits)
            {
                if (c.isDead)
                    Destroy(c.gameObject);
            }
            playerUnits.RemoveAll(c => c.isDead);
        }
    }

    void HandleOnSkill(Unit arg0, SkillSO arg1, Unit arg2)
    {
        AdvanceTurn();
    }

    private void HandleSkillSelected(SkillSO skill)
    {
        currentTurn = CurrentTurn.PLAYER_SKILL_SELECTED;
        selectedSkill = skill;
        // selectedSkill = GetUnit(selectedUnitIdxOld).skills.FindIndex(s => skill == s);
        // currentTurn = CurrentTurn.PLAYER_SKILL_SELECTED;
        // uiChannel.RaiseOnTurnChange(currentTurn, enemyUnits);
    }

    private void HandleUnitSelect(Unit unit)
    {
        switch (currentTurn)
        {
            case CurrentTurn.PLAYER_TURN or CurrentTurn.PLAYER_UNIT_SELECTED:
                if (!unit.IsEnemy && unit.hasTurn && unit != SelectedUnit)
                {
                    uiChannel.RaiseOnUnitSelect(unit);
                    currentTurn = CurrentTurn.PLAYER_UNIT_SELECTED;
                    SelectedUnit = unit;
                    uiChannel.RaiseOnAssignSkills(unit.skills);
                    uiChannel.RaiseOnTurnChange(currentTurn, new List<Unit> { unit });
                }
                break;
            case CurrentTurn.PLAYER_SKILL_SELECTED:
                if (unit.IsEnemy && !unit.isDead)
                {
                    ActionQueueManager.EnqueueSkillAction(SelectedUnit, selectedSkill, unit);
                    SelectedUnit = null;
                }
                break;
        }
        // if (!unit.IsEnemy && (currentTurn == CurrentTurn.PLAYER_TURN || currentTurn == CurrentTurn.PLAYER_UNIT_SELECTED) && unit.hasTurn)
        // {
        //     currentTurn = CurrentTurn.PLAYER_UNIT_SELECTED;
        //     selectedUnit = playerUnits.FindIndex(c => c.Equals(unit));
        //     Debug.Log($"Selected Unit: ${selectedUnit}");
        //     selectedSkill = -1;
        //     uiChannel.RaiseOnAssignSkills(unit.skills);
        //     uiChannel.RaiseOnTurnChange(currentTurn, new List<Character> { unit });
        // }
        // if (currentTurn == CurrentTurn.PLAYER_SKILL_SELECTED && unit.IsEnemy)
        // {
        //     ActionQueueManager.EnqueueSkillAction(GetUnit(selectedUnit), GetUnit(selectedUnit).skills[selectedSkill], unit);
        // }
    }

    private void HandleUnitHover(Unit unit)
    {
        switch (currentTurn)
        {
            case CurrentTurn.PLAYER_TURN or CurrentTurn.PLAYER_UNIT_SELECTED:
                if (!unit.IsEnemy && unit.hasTurn && unit != SelectedUnit)
                {
                    uiChannel.RaiseOnUnitHover(unit);
                }
                break;
            case CurrentTurn.PLAYER_SKILL_SELECTED:
                if (unit.IsEnemy && !unit.isDead)
                {
                    uiChannel.RaiseOnUnitHover(unit);
                }
                break;
        }
        // if (!unit.IsEnemy && (currentTurn == CurrentTurn.PLAYER_TURN || currentTurn == CurrentTurn.PLAYER_UNIT_SELECTED) && unit.hasTurn)
        // {
        //     uiChannel.RaiseOnTurnChange(currentTurn, new List<Character> { unit });
        // }
        // if (currentTurn == CurrentTurn.PLAYER_SKILL_SELECTED && unit.IsEnemy)
        // {
        //     ActionQueueManager.EnqueueSkillAction(GetUnit(selectedUnit), GetUnit(selectedUnit).skills[selectedSkill], unit);
        // }
    }

    void HandleTurn()
    {
        foreach (var player in playerUnits)
            player.hasTurn = true;

        uiChannel.RaiseOnTurnChange(
            currentTurn,
            currentTurn == CurrentTurn.PLAYER_TURN ? playerUnits.FindAll(player => player.hasTurn) : new()
        );
    }

    public void AdvanceTurn()
    {
        if (currentTurn == CurrentTurn.ENEMY_TURN)
        {
            currentTurn = CurrentTurn.PLAYER_TURN;
            combatEvent.RaiseOnNewTurnEvent(this);
            currentTurnNumber++;
        }
        else
        {
            currentTurn = CurrentTurn.ENEMY_TURN;
        }
        HandleTurn();
    }

    public Unit GetUnit(int idx)
    {
        return GetUnit(idx, false);
    }
    public Unit GetUnit(int idx, bool isFromOppositeSide)
    {
        if (isFromOppositeSide)
        {
            if (currentTurn != CurrentTurn.ENEMY_TURN && idx < enemyUnits.Count)
                return enemyUnits[idx];
            else if (idx < playerUnits.Count)
                return playerUnits[idx];
            else
                return null;
        }
        else
        {
            if (currentTurn != CurrentTurn.ENEMY_TURN && idx < playerUnits.Count)
                return playerUnits[idx];
            else if (idx < enemyUnits.Count)
                return enemyUnits[idx];
            else
                return null;
        }
    }
}