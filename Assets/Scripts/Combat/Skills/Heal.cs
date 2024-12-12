using UnityEngine;
// TODO: Create Heal ACtion
[CreateAssetMenu(menuName = "Skills/Heal")]
public class Heal : SkillSO
{
    [SerializeField] int healingAmount = 2;
    protected override void Execute(Unit owner, Unit target)
    {
        target.Heal(healingAmount);
    }

    protected override void RegisterParallelTypes()
    {
        throw new System.NotImplementedException();
    }
}