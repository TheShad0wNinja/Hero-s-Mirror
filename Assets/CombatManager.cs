using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum CurrentTurn
{
    PLAYER_TURN,
    ENEMY_TURN
}

public class CombatManager : MonoBehaviour
{
    [Header("Initilize combat")]
    public CombatEvent combatEvent;
    public ActionQueueManager actionQueueManager;
    public List<Character> units;
    public List<Character> playerUnits = new();
    public List<Character> enemyUnits = new();


    [Header("Debugging a turn")]
    public int skillIdx = 0;
    public bool triggerSkill = false;
    public bool triggerEffects = false;
    public bool triggerPassives = false;

    public CurrentTurn currentTurn;
    public int selectedUnit = 0;
    public int selectedTarget = 0;
    public int currentTurnNumber = 1;



    void Start()
    {
        foreach (var unit in units)
        {
            if (unit.IsEnemy)
                enemyUnits.Add(unit);
            else
                playerUnits.Add(unit);
        }
    }

    public void PerformUnitAction()
    {
        Character c = GetUnit(selectedUnit);
        Character t = GetUnit(selectedTarget, true);
        if (c && t) 
        {
            actionQueueManager.EnqueueAction(c, c.skills[skillIdx], t);
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
    }

    public Character GetUnit(int idx)
    {
        return GetUnit(idx, false);
    }
    public Character GetUnit(int idx, bool isFromOppositeSide)
    {
        if (isFromOppositeSide)
        {
            if (currentTurn == CurrentTurn.PLAYER_TURN && idx < enemyUnits.Count)
                return enemyUnits[idx];
            else if (idx < playerUnits.Count)
                return playerUnits[idx];
            else
                return null;
        }
        else
        {
            if (currentTurn == CurrentTurn.PLAYER_TURN && idx < playerUnits.Count)
                return playerUnits[idx];
            else if (idx < enemyUnits.Count)
                return enemyUnits[idx];
            else
                return null;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CombatManager))]
public class CombatManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CombatManager manager = (CombatManager)target;
        if (manager == null) return;

        GUILayout.Space(10);
        GUILayout.Label("Combat Manager Debugging", EditorStyles.boldLabel);

        // Current Turn
        GUILayout.Label("Current Turn: " + manager.currentTurn, EditorStyles.largeLabel);
        GUILayout.Label("Turn Number: " + manager.currentTurnNumber, EditorStyles.largeLabel);
        manager.currentTurn = (CurrentTurn)EditorGUILayout.EnumPopup("Turn:", manager.currentTurn);

        GUILayout.Space(10);

        // Unit Selection
        GUILayout.Label("Unit Selection", EditorStyles.boldLabel);
        if (manager.currentTurn == CurrentTurn.PLAYER_TURN)
        {
            manager.selectedUnit = EditorGUILayout.IntSlider("Selected Unit:", manager.selectedUnit, 0, Math.Max(0, manager.playerUnits.Count - 1));
            manager.selectedTarget = EditorGUILayout.IntSlider("Selected Target:", manager.selectedTarget, 0, Math.Max(0, manager.enemyUnits.Count - 1));
        }
        else
        {
            manager.selectedTarget = EditorGUILayout.IntSlider("Selected Unit:", manager.selectedUnit, 0, Math.Max(0, manager.playerUnits.Count - 1));
            manager.selectedUnit = EditorGUILayout.IntSlider("Selected Target:", manager.selectedTarget, 0, Math.Max(0, manager.enemyUnits.Count - 1));
        }

        GUILayout.Space(10);

        var selectedUnit = manager.GetUnit(manager.selectedUnit);

        GUILayout.Space(10);

        // Skill Controls
        if (selectedUnit != null)
        {
            GUILayout.Label($"Selected Unit: {selectedUnit.CharacterName}", EditorStyles.boldLabel);

            // Display skills
            GUILayout.Label("Skills:");
            for (int i = 0; i < selectedUnit.skills.Count; i++)
            {
                if (manager.skillIdx == i)
                    GUILayout.Label($"{i}: [{selectedUnit.skills[i].name}]", EditorStyles.boldLabel);
                else
                    GUILayout.Label($"{i}: {selectedUnit.skills[i].name}");
            }

            // Limit skillIdx to the number of skills the unit has
            manager.skillIdx = EditorGUILayout.IntSlider("Skill Index:", manager.skillIdx, 0, selectedUnit.skills.Count - 1);
        }
        else
        {
            GUILayout.Label("No unit selected.", EditorStyles.helpBox);
        }


        GUILayout.Space(10);

        // Buttons for Manual Actions
        if (GUILayout.Button("Perform Skill"))
        {
            manager.PerformUnitAction();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Advance Turn"))
            manager.AdvanceTurn();


        GUILayout.Space(100);

        // Draw default inspector for remaining fields
        DrawDefaultInspector();
    }
}
#endif