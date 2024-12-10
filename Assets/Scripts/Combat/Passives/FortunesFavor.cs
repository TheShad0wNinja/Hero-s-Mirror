using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Passives/Fortunes Favor")]
public class FortunesFavor : PassiveSO
{
    [Range(0f, 1f), SerializeField] float probability = 0.25f;
    [SerializeField] StatusEffectSO statusEffect;
    public override void SubscribeToEvent(Passive instance)
    {
        if (CombatEvent.Instance != null)
            CombatEvent.Instance.NewTurn += Execute;
    }

    void Execute(CombatManager cm)
    {
        bool didTrigger = UnityEngine.Random.Range(0f, 1f) <= probability;        
        if (didTrigger) 
        {
            cm.enemyUnits.ForEach(unit => ActionQueueManager.EnqueueStatusEffectAction(unit, new StatusEffect(statusEffect)));
            
        }
    }
}