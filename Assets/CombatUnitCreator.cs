using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatUnitCreator : MonoBehaviour
{
    // [Serializable]
    // public class UnitPrefabListWrapper { }
    public List<UnitPrefab> unitPrefabs;
    public CombatManager combatManager;

    [Serializable]
    public class UnitPrefab
    {
        public GameObject prefab;
        public string unitName;
    }  

    void Start()
    {
        if (UI_Behaviour_Manager.Instance == null) return;

        var characters =  UI_Behaviour_Manager.Instance.teamAssembleCharacters;
        foreach (var c in characters)
        {
            Debug.Log(c.name);
            var x = unitPrefabs.Find(u => u.unitName == c.name);
            Debug.Log(x.unitName + " " + x.prefab);
            if (x != null)
            {
                var unitObject = Instantiate(x.prefab);
                var unitScript = unitObject.GetComponent<Unit>();
                unitScript.InitilizeUnit(c);

                combatManager.units.Add(unitScript);
            }
        }

        combatManager.StartCombat();
    }
}
