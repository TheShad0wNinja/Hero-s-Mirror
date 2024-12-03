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
    public UnityAction<CurrentTurn, List<Character>> OnTurnChange;
    public UnityAction<Character> OnUnitSelect;

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

    public void RaiseOnTurnChange(CurrentTurn turn,List<Character> characters)
    {
        Debug.Log("TURN CHANGED " + turn);
        string characterNames = string.Join("|", characters.Select(c => c.CharacterName));
        Debug.Log(characterNames);
        OnTurnChange?.Invoke(turn, characters);
    }

    public void RaiseOnUnitSelect(Character character)
    {
        Debug.Log($"{character} selected");
        OnUnitSelect.Invoke(character);
    }
}
