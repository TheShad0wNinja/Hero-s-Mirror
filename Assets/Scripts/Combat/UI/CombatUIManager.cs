using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIManager : MonoBehaviour
{
    public Unit test;
    public GameObject hoverSelector;
    public GameObject selectSelector;
    public TextMeshProUGUI notifactionText;
    public Canvas canvas;
    public Canvas selectorCanvas;
    public TextMeshProUGUI currentTurnText;
    public TextMeshProUGUI currentRoundText;
    public GameObject skillBoxPrefap;
    public GameObject skillsPanel;
    public CombatUIChannel uiChannel;
    public MouseChannel mouseChannel;
    public CombatEvent combatEvent;

    List<GameObject> currentSelectors = new();
    GameObject currentHoverSelector;
    List<GameObject> currentSkills = new();

    void Start()
    {
        SetupEvents();
    }

    void SetupEvents()
    {
        CombatEvent.Instance.UnitDamage += TriggerDamageEffect;
        CombatEvent.Instance.UnitShieldDamage += TriggerDamageEffect;
        CombatEvent.Instance.SkillPerformed += TriggerSkillEffect;
        CombatEvent.Instance.UnitStatusEffect += TriggerStatusEffectEffect;
        CombatEvent.Instance.UnitDeath += TriggerDeathEffect;

        if (uiChannel != null)
        {
            uiChannel.NewTurn += HandleNewTurn;            

            uiChannel.TurnChanged += HandleTurnChange;
            uiChannel.AssignSkills += AddNewSkills;

            uiChannel.UnitHovered += HandleUnitHover;
            uiChannel.UnitSelected += HandleUnitSelect;
            uiChannel.RemoveSelectors += RemoveAllSelectors;
        }

        if (mouseChannel != null)
        {
            mouseChannel.OnUnitUnhover += HandleUnitUnhover;
        }

    }

    void HandleNewTurn(int currentRound)
    {
        currentRoundText.text = $"Round: {currentRound}";
    }

    void HandleTurnChange(TurnState turnState, List<Unit> units)
    {
        currentTurnText.text = turnState switch
        {
            TurnState.PLAYER_TURN => "Player's Turn",
            TurnState.ENEMY_TURN => "Enemy's Turn",
            _ => currentTurnText.text
        };

        switch (turnState)
        {
            case TurnState.ENEMY_TURN or TurnState.PLAYER_TURN:
                RemoveAllSkills();
                RemoveAllSelectors();
                break;
        }
    }

    void RemoveHoverSelector()
    {
        if (currentHoverSelector != null)
            Destroy(currentHoverSelector);
        currentHoverSelector = null;
    }

    void CreateUnitSelector(Unit unit, bool isHover)
    {
        Vector3 unitScale = GetUnitUIScale(unit);
        Vector3 unitCenter = GetUnitUICenter(unit);

        GameObject newSelector = Instantiate(isHover ? hoverSelector : selectSelector, unitCenter, Quaternion.identity, selectorCanvas.transform);
        if (isHover)
            currentHoverSelector = newSelector;
        else
            currentSelectors.Add(newSelector);

        var selectorRectTransform = newSelector.GetComponent<RectTransform>();
        selectorRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, unitScale.y);
        selectorRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 150f);
    }

    private void HandleUnitSelect(Unit unit)
    {
        RemoveHoverSelector();
        CreateUnitSelector(unit, false);
    }

    private void HandleUnitUnhover(Unit unit)
    {
        RemoveHoverSelector();
    }

    private void HandleUnitHover(Unit unit)
    {
        CreateUnitSelector(unit, true);
    }


    Vector3 GetUnitUIScale(Unit unit)
    {
        const float screenPointToRectRelativeMargin = 1.9f;
        const float relativeWidth = 150f;

        var bounds = unit.GetComponent<SpriteRenderer>().bounds;

        var topPoint = Camera.main.WorldToScreenPoint(new(bounds.center.x, bounds.max.y));
        var bottomPoint = Camera.main.WorldToScreenPoint(new(bounds.center.x, bounds.min.y));
        var height = (topPoint.y - bottomPoint.y) / screenPointToRectRelativeMargin;

        return new Vector3(relativeWidth, height);
    }

    Vector3 GetUnitUICenter(Unit unit)
    {
        var bounds = unit.GetComponent<SpriteRenderer>().bounds;

        var uiPosition = Camera.main.WorldToScreenPoint(bounds.center);
        return uiPosition;
    }

    Vector3 GetUnitUIBottom(Unit unit)
    {
        var bounds = unit.GetComponent<SpriteRenderer>().bounds;
        var bottomPosition = new Vector3(bounds.center.x, bounds.max.y, 0);
        var uiPosition = Camera.main.WorldToScreenPoint(bottomPosition);
        return uiPosition;
    }

    void RemoveAllSelectors()
    {
        if (currentHoverSelector != null)
            Destroy(currentHoverSelector);
        currentHoverSelector = null;

        currentSelectors.ForEach(s => Destroy(s));
        currentSelectors.Clear();

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


    private void TriggerStatusEffectEffect(Unit arg0, StatusEffect arg1, StatusEffectAction.ActionType actionType)
    {
        CreateText(arg0.transform.position + Vector3.up * 3, $"{arg0.name} inflicted with {arg1.name} [{actionType}]", 2f);
    }

    void TriggerSkillEffect(Unit attacker, SkillSO skill, Unit target)
    {
        CreateText(attacker.transform.position, $"{skill.name}", 1f);
    }

    void TriggerDamageEffect(Unit unit, int damage)
    {
        CreateText(unit.transform.position + Vector3.down * 1, $"Health -{damage}", 1.5f);
    }

    void TriggerShieldDamageEffect(Unit unit, int damage)
    {
        CreateText(unit.transform.position + Vector3.up * 1, $"Shield -{damage}", 1.5f);
    }

    private void TriggerDeathEffect(Unit arg0, Unit arg1)
    {
        CreateText(arg1.transform.position + Vector3.down * 3, $"{arg1.name} Died", 3f);
    }

    void CreateText(Vector3 position, string text, float duration)
    {
        var a = Instantiate(notifactionText, canvas.transform);
        a.transform.position = Camera.main.WorldToScreenPoint(position);
        a.text = text;
        StartCoroutine(RemoveAfterX(a, duration));
    }

    IEnumerator RemoveAfterX(TextMeshProUGUI a, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(a.gameObject);
        yield return null;
    }
}
