using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatDashboardUIManager : MonoBehaviour
{
    public CombatUIChannel uiChannel;
    public TextMeshProUGUI unitNameText;
    public GameObject skillBoxPrefap;
    public GameObject skillsPanel;
    public ProgressBarController healthBar, manaBar;
    List<GameObject> currentSkills = new();

    // Start is called before the first frame update
    void Start()
    {
        if (uiChannel != null)
        {
            uiChannel.AssignSkills += AddNewSkills;
            uiChannel.AssignStats += AssignUnitStats;
            uiChannel.TurnChanged += HandleTurnChange;
        }
        
    }

    void HandleTurnChange(TurnState turnState, List<Unit> units)
    {
        switch (turnState)
        {
            case TurnState.ENEMY_TURN or TurnState.PLAYER_TURN:
                RemoveAllSkills();
                break;
        }
    }

    private void AssignUnitStats(Unit unit)
    {
        healthBar.SetBarValue(unit.CurrentHealth, unit.MaxHealth); 
        manaBar.SetBarValue(unit.CurrentMana, unit.MaxMana);
        unitNameText.text = unit.UnitName;
    }

    void AddNewSkill(SkillSO skill)
    {
        var instance = Instantiate(skillBoxPrefap, skillsPanel.transform);
        instance.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => uiChannel.OnSkillSelected(skill));
        SkillItemController itemController = instance.GetComponent<SkillItemController>();
        itemController.SetSkillTitle(skill.skillName);
        if (skill.sprite != null)
            itemController.SetImageSprite(skill.sprite);
        currentSkills.Add(instance);
    }

    void AddNewSkills(List<SkillSO> skills)
    {
        RemoveAllSkills();
        skills.ForEach(skill => AddNewSkill(skill));
    }

    void RemoveAllSkills()
    {
        currentSkills.ForEach(s => Destroy(s));
        currentSkills.Clear();
    }
}
