using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTrigger : MonoBehaviour
{
    public List<UnitSO> enemies;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CombatEnemyManager.Instance.AssignEnemies(enemies);
            Scene_Manager.Instance.ChangeSceneAdditive("Combat");
        }
    }
}
