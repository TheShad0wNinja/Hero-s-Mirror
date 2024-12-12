using UnityEngine;

[CreateAssetMenu(menuName = "Passives/DoubleStrike")]
public class DoubleStrike : PassiveSO
{
    [SerializeField] float probability = 0.25f;
    [SerializeField] UnitSO unitSO;
    bool hasRepeatedThisTurn = false;
    public override void SubscribeToEvent(Passive instance)
    {
        hasRepeatedThisTurn = false;
        CombatEvent.Instance.SkillPerformed += HandleEvent;
        CombatEvent.Instance.NewTurn += ResetRepeat;
    }

    private void ResetRepeat(CombatManager arg0)
    {
        hasRepeatedThisTurn = false;
    }

    void HandleEvent(Unit unit, SkillSO skill, Unit target)
    {
        float randValue = Random.Range(0f, 1f);
        if (unit.UnitName == unitSO.unitName && randValue <= probability && !hasRepeatedThisTurn)
        {
            ActionQueueManager.EnqueueSkillAction(unit, skill, target, !unit.IsEnemy);
            hasRepeatedThisTurn = true;
        }
    }
}