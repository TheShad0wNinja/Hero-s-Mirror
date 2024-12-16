using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatBackgroundTypes
{
    FOREST,
    DUNGEON,
    HOUSE
}

public class CombatTrigger : MonoBehaviour
{
    public List<UnitSO> enemies;
    public CombatBackgroundTypes combatBackgroundType = CombatBackgroundTypes.FOREST;
    bool hasTriggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            CombatEnemyManager.Instance.AssignEnemies(enemies);
            CombatEnemyManager.Instance.combatBackgroundType = combatBackgroundType;
            Scene_Manager.Instance.ChangeSceneAdditiveRemoveLight("Combat");
            hasTriggered = true;
        }
    }
}
