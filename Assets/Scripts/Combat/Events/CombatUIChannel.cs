using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Test")]
public class CombatUIChannel : ScriptableObject
{
    public UnityAction<SkillSO> OnSkillSelected;
    public UnityAction<List<SkillSO>> OnAssignSkills;
    public UnityAction<CurrentTurn, List<Unit>> OnTurnChange;
    public UnityAction<Unit> OnUnitSelect;
    public UnityAction<Unit> OnUnitHover;

    public void RaiseOnSkillEvent(SkillSO skill)
    {
        Debug.Log($"SKILL {skill.name} SELECTED");
        OnSkillSelected?.Invoke(skill);
    }

    public void RaiseOnAssignSkills(List<SkillSO> skills)
    {
        Debug.Log($"ASSIGNING SKILLS");
        OnAssignSkills?.Invoke(skills);
    }

    public void RaiseOnTurnChange(CurrentTurn turn,List<Unit> units)
    {
        Debug.Log("TURN CHANGED " + turn);
        string unitNames = string.Join("|", units.Select(c => c.UnitName));
        Debug.Log(unitNames);
        OnTurnChange?.Invoke(turn, units);
    }

    public void RaiseOnUnitSelect(Unit unit)
    {
        Debug.Log($"{unit} selected");
        OnUnitSelect.Invoke(unit);
    }

    public void RaiseOnUnitHover(Unit unit)
    {
        Debug.Log($"{unit} hovered");
        OnUnitHover.Invoke(unit);
    }
}
