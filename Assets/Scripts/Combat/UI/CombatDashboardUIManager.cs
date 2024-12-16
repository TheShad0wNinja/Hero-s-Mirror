using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatDashboardUIManager : MonoBehaviour
{
    public CombatUIChannel uiChannel;
    public TextMeshProUGUI unitNameText;
    public GameObject skillBoxPrefap;
    public GameObject skillsPanel;
    public ProgressBarController healthBar, manaBar;
    public GameObject enemyInfoPrefab, enemyListPanel;
    public Image portrait;
    public Sprite defaultPortrait;
    public GameObject potionItemPrefab, potionList;
    public StatusEffectManager statusEffectManager;

    List<GameObject> currentSkills = new();
    List<GameObject> currentPotions = new();
    Dictionary<Unit, EnemyInfoController> activeEnemies = new();

    void OnEnable()
    {
        if (uiChannel != null)
        {
            uiChannel.AssignStats += AssignUnitStats;
            uiChannel.AssignEnemies += AssignEnemyList;
            uiChannel.TurnChanged += HandleTurnChange;
            uiChannel.UpdatePotion += AssignPotionList;
            uiChannel.UpdateStats += UpdateUnitStats;
        }

        if (CombatEvent.Instance != null)
        {
            CombatEvent.Instance.UnitDamage += HandleDamage;
            CombatEvent.Instance.UnitDeath += HandleDeath;
            // CombatEvent.Instance.UnitStatusEffect += HandleStatusEffect;
        }
    }

    void OnDisable()
    {
        if (uiChannel != null)
        {
            uiChannel.AssignStats -= AssignUnitStats;
            uiChannel.AssignEnemies -= AssignEnemyList;
            uiChannel.TurnChanged -= HandleTurnChange;
            uiChannel.UpdatePotion -= AssignPotionList;
            uiChannel.UpdateStats -= UpdateUnitStats;
        }

        if (CombatEvent.Instance != null)
        {
            CombatEvent.Instance.UnitDamage -= HandleDamage;
            CombatEvent.Instance.UnitDeath -= HandleDeath;
            // CombatEvent.Instance.UnitStatusEffect -= HandleStatusEffect;
        } 
    }

    private void HandleDeath(Unit _, Unit unit)
    {
        if (activeEnemies.ContainsKey(unit))
        {
            Destroy(activeEnemies[unit].gameObject);
            activeEnemies.Remove(unit);
        }
    }

    private void HandleDamage(Unit unit, int amount)
    {
        if (activeEnemies.ContainsKey(unit))
        {
            activeEnemies[unit].UpdateInfo(unit);
        }
    }

    void HandleTurnChange(TurnState turnState, List<Unit> units)
    {
        switch (turnState)
        {
            case TurnState.ENEMY_TURN or TurnState.PLAYER_TURN:
                RemoveUnitStats();
                break;
        }
    }

    private void AssignEnemyList(List<Unit> enemies)
    {
        foreach (var enemy in enemies)
        {
            var box = Instantiate(enemyInfoPrefab, enemyListPanel.transform);
            var controller = box.GetComponent<EnemyInfoController>();
            controller.UpdateInfo(enemy);
            activeEnemies.Add(enemy, controller);
        }
    }

    private void UpdateUnitStats(Unit unit)
    {
        healthBar.SetBarValue(unit.CurrentHealth, unit.MaxHealth); 
        manaBar.SetBarValue(unit.CurrentMana, unit.MaxMana);

        AssignPotionList();
        AddNewSkills(unit.skills, unit.CurrentMana);
        statusEffectManager.UpdateStatusEffects(unit.activeEffects);
    }


    private void AssignUnitStats(Unit unit)
    {
        healthBar.SetBarValue(unit.CurrentHealth, unit.MaxHealth); 
        manaBar.SetBarValue(unit.CurrentMana, unit.MaxMana);
        unitNameText.text = unit.name;
        if (unit.portrait != null)
            portrait.sprite = unit.portrait;
        else
            portrait.sprite = defaultPortrait;
        if (unit.IsFlipped)
            portrait.transform.localScale *= new Vector2(-1, 1);
        AddNewSkills(unit.skills, unit.CurrentMana);
        AssignPotionList();
        statusEffectManager.UpdateStatusEffects(unit.activeEffects);
    }

    private void AssignPotionList()
    {
        RemovePotionList();

        foreach (var v in UI_Behaviour_Manager.Instance.ownedPotions)
        {
            var obj = Instantiate(potionItemPrefab, potionList.transform);
            obj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => uiChannel.OnPotionSelected(v.Key));
            PotionItemController controller = obj.GetComponent<PotionItemController>();
            controller.SetHoverDesc(v.Key.description);
            controller.SetPotionImage(v.Key.image);
            controller.SetPotionTitle(v.Key.name);
            controller.SetPotionStats(v.Key.currentStatsFiltered);
            controller.SetPotionQuantity(v.Value);
            currentPotions.Add(obj);
        }
    }

    private void RemovePotionList()
    {
        currentPotions.ForEach(o => Destroy(o));
        currentPotions.Clear();
    }

    private void RemoveUnitStats()
    {
        healthBar.Clear(); 
        manaBar.Clear();
        unitNameText.text = "";
        portrait.sprite = defaultPortrait;
        portrait.transform.localScale = new Vector2(1, 1);
        RemoveAllSkills();
        RemovePotionList();
    }

    void AddNewSkill(SkillSO skill, int manaAmount)
    {
        var instance = Instantiate(skillBoxPrefap, skillsPanel.transform);
        var button = instance.GetComponent<UnityEngine.UI.Button>();
        if (skill.manaCost <= manaAmount)
        {
            button.onClick.AddListener(() => uiChannel.OnSkillSelected(skill));
        } else
        {
            button.interactable = false;
        }
        SkillItemController itemController = instance.GetComponent<SkillItemController>();
        itemController.SetSkillTitle(skill.skillName);
        itemController.SetSkillDesc(skill.description);
        itemController.SetManaCost(skill.manaCost);
        if (skill.sprite != null)
            itemController.SetImageSprite(skill.sprite);
        currentSkills.Add(instance);
    }

    void AddNewSkills(List<SkillSO> skills, int manaAmount)
    {
        RemoveAllSkills();
        skills.ForEach(skill => AddNewSkill(skill, manaAmount));
    }

    void RemoveAllSkills()
    {
        currentSkills.ForEach(s => Destroy(s));
        currentSkills.Clear();
    }
}
