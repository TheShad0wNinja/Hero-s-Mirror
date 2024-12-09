using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatActionMovement : MonoBehaviour
{

    [SerializeField] Transform leftLocation, rightLocation;
    [SerializeField] float offsetAmount = 2f;
    [SerializeField] float movementDuration = 0.2f;

    [Serializable]
    struct EngagedUnitInformation
    {
        public Unit unit;
        public Vector2 oldLocation;
    }
    List<EngagedUnitInformation> engagedUnits = new();
    public static CombatActionMovement Instance { get; private set; }
    int numOfEngagedSelectedUnits = 0, numOfEngagedTargetUnits = 0;
    Unit selectedUnit;

    void Start()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    public IEnumerator EngageUnits(Unit selectedUnit, List<Unit> targetUnits, bool isPlayerAction)
    {
        if (isPlayerAction)
            StartCoroutine(EngageLeftSide(selectedUnit));
        else
            StartCoroutine(EngageRightSide(selectedUnit));

        this.selectedUnit = selectedUnit;

        foreach(var targetUnit in targetUnits)
        {
            if (isPlayerAction)
                StartCoroutine(EngageRightSide(targetUnit));
            else 
                StartCoroutine(EngageLeftSide(targetUnit));
        }

        yield return new WaitUntil(() => numOfEngagedSelectedUnits == 1 && numOfEngagedTargetUnits == targetUnits.Count);

        // int numOfPlayerUnits = 0, numOfEnemyUnits = 0;
        // foreach (var unit in units)
        // {
        //     if (unit.IsEnemy)
        //     {
        //         numOfEnemyUnits++;
        //         StartCoroutine(EngageEnemyUnit(unit));
        //     }
        //     else
        //     {
        //         numOfPlayerUnits++;
        //         StartCoroutine(EngagePlayerUnit(unit));
        //     }
        // }

        // yield return new WaitUntil(() => numOfEnemyUnits == numOfEngagedTargetUnits && numOfPlayerUnits == numOfEngagedSelectedUnits);
    }

    IEnumerator EngageLeftSide(Unit unit)
    {
        engagedUnits.Add(new EngagedUnitInformation { oldLocation = unit.transform.position, unit = unit });
        Vector2 newPosition = new Vector2(leftLocation.position.x - numOfEngagedSelectedUnits * offsetAmount, leftLocation.position.y);
        // unit.transform.position = newPosition;

        yield return MoveUnit(unit.transform.position, newPosition, unit.transform);

        numOfEngagedSelectedUnits++;
        yield return null;
    }

    IEnumerator EngageRightSide(Unit unit)
    {
        engagedUnits.Add(new EngagedUnitInformation { oldLocation = unit.transform.position, unit = unit });
        Vector2 newPosition = new Vector2(rightLocation.position.x + numOfEngagedTargetUnits * offsetAmount, rightLocation.position.y);
        // unit.transform.position = newPosition;

        yield return MoveUnit(unit.transform.position, newPosition, unit.transform);

        numOfEngagedTargetUnits++;
        yield return null;
    }

    public IEnumerator DisengageUnits()
    {
        Debug.Log("Disengaging");
        foreach(var engagedUnit in engagedUnits)
        {
            StartCoroutine(DisengageUnit(engagedUnit));
        }

        yield return new WaitUntil(() => numOfEngagedTargetUnits == 0 && numOfEngagedSelectedUnits == 0);
        Debug.Log("FINISHED DISENGAGING");
        engagedUnits.Clear();
        yield return null;

    }

    IEnumerator DisengageUnit(EngagedUnitInformation engagedUnit)
    {
        Debug.Log($"Disengage {engagedUnit.unit} at {engagedUnit.oldLocation}");
        // engagedUnit.unit.transform.position = engagedUnit.oldLocation;

        yield return MoveUnit(engagedUnit.unit.transform.position, engagedUnit.oldLocation, engagedUnit.unit.transform);

        if (engagedUnit.unit == selectedUnit)
            numOfEngagedSelectedUnits--;
        else
            numOfEngagedTargetUnits--;
        
        yield return null;
    }

    IEnumerator MoveUnit(Vector2 origin, Vector2 destination, Transform unit)
    {
        float currentMovementTime = 0f;
        while (Vector2.Distance(unit.position, destination) > 0)
        {
            currentMovementTime += Time.deltaTime;
            unit.localPosition = Vector3.Lerp(origin, destination, currentMovementTime / movementDuration);
            yield return null;
        }
    }

}