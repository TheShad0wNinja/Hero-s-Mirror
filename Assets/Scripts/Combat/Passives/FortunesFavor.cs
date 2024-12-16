using UnityEngine;

[CreateAssetMenu(menuName = "Passives/Fortunes Favor")]
public class FortunesFavor : PassiveSO
{
    [Range(0f, 1f), SerializeField] float probability = 0.25f;
    [SerializeField] SkillSO skill;
    [SerializeField] UnitSO unit;
    public override void SubscribeToEvent(Passive instance)
    {
        if (CombatEvent.Instance != null)
            CombatEvent.Instance.NewTurn += Execute;
    }

    public override void UnsubscribeToEvent(Passive instance)
    {
        if (CombatEvent.Instance != null)
            CombatEvent.Instance.NewTurn -= Execute;
    }

    void Execute(CombatManager cm)
    {
        float ranVal = Random.Range(0f, 1f);        
        if (ranVal <= probability) 
        {
            var jester = cm.playerUnits.Find(u => u.name == unit.name);
            // ActionQueueManager.EnqueueEngageUnitsAction(jester, cm.enemyUnits, !jester.IsEnemy);
            ActionQueueManager.EnqueueSkillAction(jester, skill, cm.enemyUnits, !jester.IsEnemy);
            // ActionQueueManager.EnqueueDisengageUnitsAction();
        }
    }
}