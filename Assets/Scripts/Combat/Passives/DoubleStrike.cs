using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Passives/DoubleStrike")]
public class DoubleStrike : PassiveSO
{
    [SerializeField] float probability = 0.25f;
    [SerializeField] SkillSO repeatableSkil;
    bool hasRepeatedThisTurn;
    public override void SubscribeToEvent(Passive instance)
    {
        CombatEvent.Instance.SkillPerformed += HandleEvent;
        CombatEvent.Instance.NewTurn += ResetRepeat;
    }

    private void ResetRepeat(CombatManager arg0)
    {
        hasRepeatedThisTurn = false;
    }

    void HandleEvent(Unit unit, SkillSO skill, Unit target)
    {
        if (UnityEngine.Random.value <= probability && skill == repeatableSkil && !hasRepeatedThisTurn)
        {
            ActionQueueManager.EnqueueSkillAction(unit, skill, target);
            hasRepeatedThisTurn = true;
        }
    }
}