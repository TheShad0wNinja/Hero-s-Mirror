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

    public void RaiseOnTurnChange(CurrentTurn turn,List<Unit> characters)
    {
        Debug.Log("TURN CHANGED " + turn);
        string characterNames = string.Join("|", characters.Select(c => c.UnitName));
        Debug.Log(characterNames);
        OnTurnChange?.Invoke(turn, characters);
    }

    public void RaiseOnUnitSelect(Unit character)
    {
        Debug.Log($"{character} selected");
        OnUnitSelect.Invoke(character);
    }

    public void RaiseOnUnitHover(Unit character)
    {
        Debug.Log($"{character} hovered");
        OnUnitHover.Invoke(character);
    }
}
