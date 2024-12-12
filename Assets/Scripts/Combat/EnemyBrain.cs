using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    void Start()
    {
        if (CombatEvent.Instance != null)
        {
            Debug.Log("ALLO");
            CombatEvent.Instance.TurnChanged = HandleTurnChange;
        }
    }

    private void HandleTurnChange(CombatManager cm)
    {
        Debug.Log("LIGMA: " + cm.turnState);
        switch (cm.turnState)
        {
            case TurnState.ENEMY_TURN:
                HandleEnemyTurnStart(cm);
                break;

            case TurnState.ENEMY_UNIT_SELECTED:
                HandleEnemySkillSelection(cm);
                break;
            case TurnState.ENEMY_SKILL_SELECTED:
                HandleEnemyTargetSelection(cm);
                break;
            case TurnState.ENEMY_TARGET_SELECTED:
                break;
            case TurnState.ENEMY_ACTION_PERFORMING:
                break;
        }
    }

    private void HandleEnemyTargetSelection(CombatManager cm)
    {
        int numOfTargets = cm.selectedSkill.targetType switch
        {
            TargetType.PLAYER_UNIT_SINGLE or
                TargetType.ENEMY_UNIT_SINGLE => 1,
            TargetType.PLAYER_UNIT_MULTIPLE 
                => cm.selectedSkill.numberOfTargets > cm.playerUnits.Count ? cm.playerUnits.Count : cm.selectedSkill.numberOfTargets,
            TargetType.ENEMY_UNIT_MULTIPLE 
                => cm.selectedSkill.numberOfTargets > cm.enemyUnits.Count ? cm.enemyUnits.Count : cm.selectedSkill.numberOfTargets,
            _ => 0
        };

        var rnd = new System.Random();

        List<Unit> selectedTargets = cm.selectedSkill.targetType switch
        {
            TargetType.PLAYER_UNIT_SINGLE or TargetType.PLAYER_UNIT_MULTIPLE
                => cm.playerUnits.OrderBy(_ => rnd.Next()).Take(numOfTargets).ToList(),
            TargetType.ENEMY_UNIT_SINGLE or TargetType.ENEMY_UNIT_MULTIPLE
                => cm.enemyUnits.OrderBy(_ => rnd.Next()).Take(numOfTargets).ToList(),
            _ => new List<Unit>()
        };

        cm.HandleEnemyTargetUnitSelect(selectedTargets);
    }

    private void HandleEnemySkillSelection(CombatManager cm)
    {
        var availableSkills = cm.selectedUnit.skills;
        var randIdx = UnityEngine.Random.Range(0, availableSkills.Count);
        cm.HandleSkillSelected(availableSkills[randIdx]);
    }

    private void HandleEnemyTurnStart(CombatManager cm)
    {
        var availableEnemyUnits = cm.enemyUnits.FindAll(u => u.HasTurn);
        var randIdx = UnityEngine.Random.Range(0, availableEnemyUnits.Count);

        cm.HandleEnemyMainUnitSelect(cm.enemyUnits[randIdx]);
    }
}