using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    SpriteRenderer selectedUnitSpriteRenderer;

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
            StartCoroutine(EngageLeftSide(selectedUnit, 0));
        else
            StartCoroutine(EngageRightSide(selectedUnit, 0));

        // Debug.Log("ABVDLFJ");
        // selectedUnitSpriteRenderer = selectedUnit.GetComponent<SpriteRenderer>();
        // Debug.Log("aaavx " + selectedUnitSpriteRenderer.name);
        // selectedUnitSpriteRenderer.sortingOrder = 1;


        int offset = 0;

        foreach(var targetUnit in targetUnits)
        {
            if (isPlayerAction)
                StartCoroutine(EngageRightSide(targetUnit, offset));
            else 
                StartCoroutine(EngageLeftSide(targetUnit, offset));

            offset++;
        }

        yield return new WaitUntil(() => engagedUnits.Count >= targetUnits.Count);
    }

    IEnumerator EngageLeftSide(Unit unit, int unitOffset)
    {
        Debug.Log($"Engaging Left: {unit}");
        Vector2 newPosition = new Vector2(leftLocation.position.x - unitOffset * offsetAmount, leftLocation.position.y);
        Vector2 oldPosition = unit.transform.position;

        yield return Helper.MoveObject(unit.transform.position, newPosition, unit.transform, movementDuration);

        engagedUnits.Add(new EngagedUnitInformation { oldLocation = oldPosition, unit = unit });

        yield return null;
    }

    IEnumerator EngageRightSide(Unit unit, int unitOffset)
    {
        Debug.Log($"Engaging Right: {unit}");
        Vector2 newPosition = new Vector2(rightLocation.position.x + unitOffset * offsetAmount, rightLocation.position.y);
        Vector2 oldPosition = unit.transform.position;

        yield return Helper.MoveObject(unit.transform.position, newPosition, unit.transform, movementDuration);

        engagedUnits.Add(new EngagedUnitInformation { oldLocation = oldPosition, unit = unit });

        yield return null;
    }

    public IEnumerator DisengageUnits()
    {
        // selectedUnitSpriteRenderer.sortingOrder = 1;
        // selectedUnitSpriteRenderer = null;

        foreach(var engagedUnit in engagedUnits.ToList())
        {
            StartCoroutine(DisengageUnit(engagedUnit));
        }

        yield return new WaitUntil(() => engagedUnits.Count == 0);
        engagedUnits.Clear();
        yield return null;

    }

    IEnumerator DisengageUnit(EngagedUnitInformation engagedUnit)
    {
        Debug.Log($"Disengage {engagedUnit.unit} to {engagedUnit.oldLocation}");

        yield return Helper.MoveObject(engagedUnit.unit.transform.position, engagedUnit.oldLocation, engagedUnit.unit.transform, movementDuration);

        engagedUnits.Remove(engagedUnit);

        yield return null;
    }

    // IEnumerator MoveUnit(Vector2 origin, Vector2 destination, Transform objectTransform)
    // {
    //     float currentMovementTime = 0f;
    //     while (Vector2.Distance(objectTransform.position, destination) > 0)
    //     {
    //         currentMovementTime += Time.deltaTime;
    //         objectTransform.localPosition = Vector3.Lerp(origin, destination, currentMovementTime / movementDuration);
    //         yield return null;
    //     }
    // }

}