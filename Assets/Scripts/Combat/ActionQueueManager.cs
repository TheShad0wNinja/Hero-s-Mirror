using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionQueueManager : MonoBehaviour
{
    Queue<ActionQueueItem> actionQueue = new();
    bool isProcessing = false;

    public void EnqueueAction(Character character, SkillSO skillSO, Character target)
    {
        actionQueue.Enqueue(new ActionQueueItem(character, skillSO, target));
        if (!isProcessing)
            StartCoroutine(ProcessQueue());
    }

    IEnumerator ProcessQueue()
    {
        isProcessing = true;

        while (actionQueue.Count > 0)
        {
            ActionQueueItem actionItem = actionQueue.Dequeue();
            yield return StartCoroutine(ExecuteAction(actionItem));
        }

        isProcessing = false;
    }

    IEnumerator ExecuteAction(ActionQueueItem actionItem)
    {
        yield return StartCoroutine(actionItem.character.AnimateAction(actionItem.skill));
        actionItem.character.PerformSkill(actionItem.target, actionItem.skill);
        yield return null;
    }
}

public class ActionQueueItem
{
    public Character character;
    public SkillSO skill;
    public Character target;

    public ActionQueueItem(Character character, SkillSO skill, Character target)
    {
        this.character = character;
        this.skill = skill;
        this.target = target;
    }
}