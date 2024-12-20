using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CombatEnemyManager : MonoBehaviour
{
    public List<Character> EnemyCharacters { get; private set; } = new();
    public static CombatEnemyManager Instance { get; private set; }
    public UnityAction<bool> OnCombatEnd;
    public CombatBackgroundTypes combatBackgroundType;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
            Destroy(this);

    }

    public void ClearEnemyList()
    {
        EnemyCharacters.Clear();
    }

    public void AssignEnemies(List<UnitSO> enemies)
    {
        foreach(var enemy in enemies)
        {
            EnemyCharacters.Add(new Character(enemy));
        }
    }
}