using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Passives/DoubleStrike")]
public class DoubleStrike : PassiveSO
{
    public float probability = 0.25f;
    public bool debug = false;
    public override void SubscribeToEvent(Passive instance)
    {
        CombatEvent.Instance.OnSkill.AddListener(HandleEvent);
    }

    void HandleEvent(Character character, SkillSO skill, Character target)
    {
        if (UnityEngine.Random.value <= probability || debug)
        {
            ActionQueueManager.Instance.EnqueueAction(character, skill, target);
            debug = false;
        }
    }
}