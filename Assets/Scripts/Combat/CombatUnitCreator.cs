using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CombatUnitCreator : MonoBehaviour
{
    public List<UnitPrefab> unitPrefabs;
    public CombatManager combatManager;
    public CombatUIManager combatUIManager;
    public List<BackgroundTypes> backgroundTypes;
    public SpriteRenderer backgroundSpriteRenderer;
    
    [Serializable]
    public class BackgroundTypes 
    {
        public Sprite sprite;
        public CombatBackgroundTypes type;
    }

    [Serializable]
    public class UnitPrefab
    {
        public GameObject prefab;
        public string unitName;
    }  

    void Start()
    {
        if (UI_Behaviour_Manager.Instance == null || CombatEnemyManager.Instance == null) return;

        List<Unit> units = new();

        var playerCharacters =  UI_Behaviour_Manager.Instance.teamAssembleCharacters;
        foreach (var c in playerCharacters)
        {
            var x = unitPrefabs.Find(u => u.unitName == c.name);
            if (x != null)
            {
                var unitObject = Instantiate(x.prefab);
                var unitScript = unitObject.GetComponent<Unit>();
                unitScript.InitilizeUnit(c);

                units.Add(unitScript);
            }
        }

        var enemyCharacters = CombatEnemyManager.Instance.EnemyCharacters;
        foreach (var c in enemyCharacters)
        {
            var x = unitPrefabs.Find(u => u.unitName == c.name);
            if (x != null)
            {
                var unitObject = Instantiate(x.prefab);
                var unitScript = unitObject.GetComponent<Unit>();
                unitScript.InitilizeUnit(c);
                units.Add(unitScript);
            }
        }

        backgroundSpriteRenderer.sprite = backgroundTypes.Find(b => b.type == CombatEnemyManager.Instance.combatBackgroundType)?.sprite;
        combatManager.StartCombat(units);
        combatUIManager.StartCombatUI();
    }
}
