using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif

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
    [Header("Initilize UI")]
    public CombatUIChannel uiChannel;
    [Header("Initilize combat")]
    public CombatEvent combatEvent;
    public ActionQueueManager actionQueueManager;
    public List<Character> units;
    public List<Character> playerUnits = new();
    public List<Character> enemyUnits = new();

    public List<Transform> playerLocations, enemyLocations;

    [Header("Debugging a turn")]
    public int selectedSkill = 0;
    public bool triggerSkill = false;
    public bool triggerEffects = false;
    public bool triggerPassives = false;

    public CurrentTurn currentTurn = CurrentTurn.PLAYER_TURN;
    public int selectedTarget = 0;
    public int currentTurnNumber = 1;

    private int selectedUnit = 0;
    public int SelectedUnit
    {
        get => selectedUnit;
        set
        {
            selectedUnit = value;
            uiChannel.RaiseOnAssignSkills(GetUnit(selectedUnit).skills);
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

        if (uiChannel != null)
        {
            uiChannel.OnUnitSelect += HandleUnitSelect;
            uiChannel.OnSkillSelected += HandleSkillSelected;
        }

        if (CombatEvent.Instance != null)
        {
            CombatEvent.Instance.OnSkill.AddListener(HandleOnSkill);
            CombatEvent.Instance.OnDeath.AddListener(HandleOnDeath);
        }
    }

    private void HandleOnDeath(Character arg0, Character arg1)
    {
        if (arg1.IsEnemy)
        {
            enemyUnits.RemoveAll(c => c.isDead);
        } else {
            playerUnits.RemoveAll(c => c.isDead);
        }
    }

    void HandleOnSkill(Character arg0, SkillSO arg1, Character arg2)
    {
        AdvanceTurn();
    }

    private void HandleSkillSelected(SkillSO skill)
    {
        selectedSkill = GetUnit(selectedUnit).skills.FindIndex(s => skill == s);
        currentTurn = CurrentTurn.PLAYER_SKILL_SELECTED;
        uiChannel.RaiseOnTurnChange(currentTurn, enemyUnits);
    }

    private void HandleUnitSelect(Character character)
    {
        if (!character.IsEnemy && currentTurn == CurrentTurn.PLAYER_TURN && character.hasTurn)
        {
            currentTurn = CurrentTurn.PLAYER_UNIT_SELECTED;
            selectedUnit = playerUnits.FindIndex(c => c.Equals(character));
            Debug.Log($"Selected Unit: ${selectedUnit}");
            selectedSkill = -1;
            uiChannel.RaiseOnAssignSkills(character.skills);
            uiChannel.RaiseOnTurnChange(currentTurn, new List<Character> { character } );
        } if (currentTurn == CurrentTurn.PLAYER_SKILL_SELECTED && character.IsEnemy)
        {
            actionQueueManager.EnqueueAction(GetUnit(selectedUnit), GetUnit(selectedUnit).skills[selectedSkill], character);
        }
    }

    void HandleTurn()
    {
        foreach(var player in playerUnits)
            player.hasTurn = true;

        uiChannel.RaiseOnTurnChange(
            currentTurn,
            currentTurn == CurrentTurn.PLAYER_TURN ? playerUnits.FindAll(player => player.hasTurn) : new()
        );
    }

    public void PerformUnitAction()
    {
        Character c = GetUnit(SelectedUnit);
        Character t = GetUnit(selectedTarget, true);
        if (c && t)
        {
            // actionQueueManager.EnqueueAction(c, c.skills[skillIdx], t);
            AdvanceTurn();
        }
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

    public Character GetUnit(int idx)
    {
        return GetUnit(idx, false);
    }
    public Character GetUnit(int idx, bool isFromOppositeSide)
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

// #if UNITY_EDITOR
// [CustomEditor(typeof(CombatManager))]
// public class CombatManagerEditor : Editor
// {
//     public override void OnInspectorGUI()
//     {
//         CombatManager manager = (CombatManager)target;
//         if (manager == null) return;

//         GUILayout.Space(10);
//         GUILayout.Label("Combat Manager Debugging", EditorStyles.boldLabel);

//         // Current Turn
//         GUILayout.Label("Current Turn: " + manager.currentTurn, EditorStyles.largeLabel);
//         GUILayout.Label("Turn Number: " + manager.currentTurnNumber, EditorStyles.largeLabel);
//         manager.currentTurn = (CurrentTurn)EditorGUILayout.EnumPopup("Turn:", manager.currentTurn);

//         GUILayout.Space(10);

//         // Unit Selection
//         GUILayout.Label("Unit Selection", EditorStyles.boldLabel);
//         if (manager.currentTurn == CurrentTurn.PLAYER_TURN)
//         {
//             manager.SelectedUnit = EditorGUILayout.IntSlider("Selected Unit:", manager.SelectedUnit, 0, Math.Max(0, manager.playerUnits.Count - 1));
//             manager.selectedTarget = EditorGUILayout.IntSlider("Selected Target:", manager.selectedTarget, 0, Math.Max(0, manager.enemyUnits.Count - 1));
//         }
//         else
//         {
//             manager.selectedTarget = EditorGUILayout.IntSlider("Selected Unit:", manager.SelectedUnit, 0, Math.Max(0, manager.playerUnits.Count - 1));
//             manager.SelectedUnit = EditorGUILayout.IntSlider("Selected Target:", manager.selectedTarget, 0, Math.Max(0, manager.enemyUnits.Count - 1));
//         }

//         GUILayout.Space(10);

//         var selectedUnit = manager.GetUnit(manager.SelectedUnit);

//         GUILayout.Space(10);

//         // Skill Controls
//         if (selectedUnit != null)
//         {
//             GUILayout.Label($"Selected Unit: {selectedUnit.CharacterName}", EditorStyles.boldLabel);

//             // Display skills
//             GUILayout.Label("Skills:");
//             for (int i = 0; i < selectedUnit.skills.Count; i++)
//             {
//                 if (manager.skillIdx == i)
//                     GUILayout.Label($"{i}: [{selectedUnit.skills[i].name}]", EditorStyles.boldLabel);
//                 else
//                     GUILayout.Label($"{i}: {selectedUnit.skills[i].name}");
//             }

//             // Limit skillIdx to the number of skills the unit has
//             manager.skillIdx = EditorGUILayout.IntSlider("Skill Index:", manager.skillIdx, 0, selectedUnit.skills.Count - 1);
//         }
//         else
//         {
//             GUILayout.Label("No unit selected.", EditorStyles.helpBox);
//         }


//         GUILayout.Space(10);

//         // Buttons for Manual Actions
//         if (GUILayout.Button("Perform Skill"))
//         {
//             manager.PerformUnitAction();
//         }

//         GUILayout.Space(10);

//         if (GUILayout.Button("Advance Turn"))
//             manager.AdvanceTurn();


//         GUILayout.Space(100);

//         // Draw default inspector for remaining fields
//         DrawDefaultInspector();
//     }
// }
// #endif