using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Random Skill")]
public class RandomSkill : SkillSO
{
    [SerializeField] SkillSO positiveSkill, negativeSkill;
    [Range(0, 1)] public float probability = 0.5f;
    [SerializeField] bool positiveUnitIsEnemy = false, negativeUnitIsEnemy = true;
    protected override void Execute(Unit owner, Unit target)
    {
    }

    public (SkillSO, List<Unit>) GetFate(List<Unit> units)
    {
        bool isPositive = Random.Range(0f, 1f) <= probability;

        if (isPositive && !positiveUnitIsEnemy)
            return (positiveSkill, units.FindAll(u => !u.IsEnemy));
        else if (isPositive && positiveUnitIsEnemy)
            return (positiveSkill, units.FindAll(u => u.IsEnemy));
        else if (!isPositive && !negativeUnitIsEnemy)
            return (negativeSkill, units.FindAll(u => !u.IsEnemy));
        else 
            return (negativeSkill, units.FindAll(u => u.IsEnemy));
    }

    protected override void RegisterParallelTypes()
    {
    }
}