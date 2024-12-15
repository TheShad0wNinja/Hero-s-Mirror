using System.Collections.Generic;
using UnityEngine;

public class CombatEnemyManager : MonoBehaviour
{
    public List<Character> EnemyCharacters { get; private set; } = new();
    public static CombatEnemyManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
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