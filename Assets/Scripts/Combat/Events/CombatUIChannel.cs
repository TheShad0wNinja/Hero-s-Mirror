using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event Channels/Combat UI Channel")]
public class CombatUIChannel : ScriptableObject
{
    public UnityAction<SkillSO> SkillSelected;
    public UnityAction<Potion> PotionSelected;
    public UnityAction<Unit> AssignStats;
    public UnityAction<Unit> UpdateStats;
    public UnityAction<List<Unit>> AssignEnemies;
    public UnityAction<TurnState, List<Unit>> TurnChanged;
    public UnityAction<Unit> UnitSelected;
    public UnityAction<Unit> UnitHovered;
    public UnityAction<int> NewTurn;
    public UnityAction RemoveUnitInfo;
    public UnityAction RemoveSelectors;
    public UnityAction UpdatePotion;

    public void OnSkillSelected(SkillSO skill)
    {
        Debug.Log($"SKILL {skill.name} SELECTED");
        SkillSelected?.Invoke(skill);
    }

    public void OnPotionSelected(Potion potion)
    {
        PotionSelected?.Invoke(potion);
    }

    public void OnAssignStats(Unit unit)
    {
        Debug.Log($"ASSIGNING SKILLS");
        AssignStats?.Invoke(unit);
    }

    public void OnUpdateStats(Unit unit)
    {
        UpdateStats?.Invoke(unit);
    }

    public void OnTurnChange(TurnState turn,List<Unit> units)
    {
        Debug.Log("TURN CHANGED " + turn);
        TurnChanged?.Invoke(turn, units);
    }

    public void OnUnitSelect(Unit unit)
    {
        // Debug.Log($"{unit} selected");
        UnitSelected.Invoke(unit);
    }

    public void OnUnitHover(Unit unit)
    {
        // Debug.Log($"{unit} hovered");
        UnitHovered.Invoke(unit);
    }

    public void OnRemoveSelectors()
    {
        // Debug.Log("Selector Removal: Invoke");
        RemoveSelectors?.Invoke();
    }

    public void OnNewTurn(int roundNumber)
    {
        // Debug.Log("OnNewTurn Event Raised");
        NewTurn?.Invoke(roundNumber);
    }

    public void OnAssignEnemies(List<Unit> enemies)
    {
        AssignEnemies?.Invoke(enemies);
    }

    public void OnRemoveUnitInfo()
    {
        RemoveUnitInfo?.Invoke();
    }

    public void UpdatePotions()
    {
        UpdatePotion.Invoke();
    }
}