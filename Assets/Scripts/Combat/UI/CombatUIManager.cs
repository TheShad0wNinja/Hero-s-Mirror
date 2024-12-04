using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CombatUIManager : MonoBehaviour
{
    public Unit test;
    public GameObject selector;
    public GameObject selectorSelected;
    public TextMeshProUGUI notifactionText;
    public Canvas canvas;
    public Canvas selectorCanvas;
    public TextMeshProUGUI currentTurnText;
    public GameObject skillBoxPrefap;
    public GameObject skillsPanel;
    public CombatUIChannel uiChannel;
    public MouseChannel mouseChannel;

    List<GameObject> currentSelectors = new();
    GameObject currentHoverSelector;
    List<GameObject> currentSkills = new();

    void Start()
    {
        CombatEvent.Instance.OnDamage.AddListener(TriggerDamageEffect);
        CombatEvent.Instance.OnSkill.AddListener(TriggerSkillEffect);
        CombatEvent.Instance.OnEffect.AddListener(TriggerStatusEffectEffect);
        CombatEvent.Instance.OnDeath.AddListener(TriggerDeathEffect);

        if (uiChannel != null)
        {
            uiChannel.OnTurnChange += HandleCurrentTurn;
            uiChannel.OnAssignSkills += AddNewSkills;
            uiChannel.OnUnitHover += HandleUnitHover;
            uiChannel.OnUnitSelect += HandleUnitSelect;
        }

        if (mouseChannel != null)
        {
            mouseChannel.OnUnitUnhover += HandleUnitUnhover;
        }
    }

    private void HandleUnitUnhover(Unit arg0)
    {
        RemoveHoverSelector();
    }

    private void RemoveHoverSelector()
    {
        if (currentHoverSelector != null)
            Destroy(currentHoverSelector);
        currentHoverSelector = null;
    }

    private void HandleUnitSelect(Unit character)
    {
        RemoveAllSelectors();
        var characterScale = GetUnitUIScale(character);
        var characterCenter = GetUnitUICenter(character);

        var newSelector = Instantiate(selectorSelected, characterCenter, Quaternion.identity, selectorCanvas.transform);
        currentSelectors.Add(newSelector);

        var selectorRectTransform = newSelector.GetComponent<RectTransform>();
        selectorRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, characterScale.y);
        selectorRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 150f);
    }

    private void HandleUnitHover(Unit character)
    {
        var characterScale = GetUnitUIScale(character);
        var characterCenter = GetUnitUICenter(character);

        var newSelector = Instantiate(selector, characterCenter, Quaternion.identity, selectorCanvas.transform);
        currentHoverSelector = newSelector;

        var selectorRectTransform = newSelector.GetComponent<RectTransform>();
        selectorRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, characterScale.y);
        selectorRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 150f);
    }

    void HandleCurrentTurn(CurrentTurn currentTurn, List<Unit> characters)
    {
        currentTurnText.text = currentTurn switch
        {
            CurrentTurn.PLAYER_TURN or CurrentTurn.PLAYER_UNIT_SELECTED or CurrentTurn.PLAYER_SKILL_SELECTED => "Player's Turn",
            CurrentTurn.ENEMY_TURN => "Enemy's Turn",
            _ => ""
        };

        switch (currentTurn)
        {
            case CurrentTurn.ENEMY_TURN:
                RemoveAllSkills();
                RemoveAllSelectors();
                break;
        }
    }

    Vector3 GetUnitUIScale(Unit character)
    {
        const float screenPointToRectRelativeMargin = 1.9f;
        const float relativeWidth = 150f;

        var bounds = character.GetComponent<SpriteRenderer>().bounds;

        var topPoint = Camera.main.WorldToScreenPoint(new(bounds.center.x, bounds.max.y));
        var bottomPoint = Camera.main.WorldToScreenPoint(new(bounds.center.x, bounds.min.y));
        var height = (topPoint.y - bottomPoint.y) / screenPointToRectRelativeMargin;

        return new Vector3(relativeWidth, height);
    }

    Vector3 GetUnitUICenter(Unit character)
    {
        var bounds = character.GetComponent<SpriteRenderer>().bounds;

        var uiPosition = Camera.main.WorldToScreenPoint(bounds.center);
        return uiPosition;
    }

    Vector3 GetCharacterUIBottom(Unit character)
    {
        var bounds = character.GetComponent<SpriteRenderer>().bounds;
        var bottomPosition = new Vector3(bounds.center.x, bounds.max.y, 0);
        var uiPosition = Camera.main.WorldToScreenPoint(bottomPosition);
        return uiPosition;
    }

    void RemoveAllSelectors()
    {
        currentSelectors.ForEach(s => Destroy(s));
        currentSelectors.Clear();
        Destroy(currentHoverSelector);
        currentHoverSelector = null;
    }

    public void AddNewSkill(SkillSO skill)
    {
        var instance = Instantiate(skillBoxPrefap, skillsPanel.transform);
        instance.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => uiChannel.RaiseOnSkillEvent(skill));
        instance.GetComponent<SkillItemController>().SetSkillTitle(skill.skillName);
        currentSkills.Add(instance);
    }

    public void AddNewSkills(List<SkillSO> skills)
    {
        RemoveAllSkills();
        skills.ForEach(skill => AddNewSkill(skill));
    }

    void RemoveAllSkills()
    {
        currentSkills.ForEach(s => Destroy(s));
        currentSkills.Clear();
    }

    private void TriggerStatusEffectEffect(Unit arg0, StatusEffect arg1)
    {
        CreateText(arg0.transform.position + Vector3.up * 2, $"{arg0.name} inflicted with {arg1.name}", 2f);
    }

    void TriggerSkillEffect(Unit attacker, SkillSO skill, Unit target)
    {
        CreateText(attacker.transform.position, $"{attacker.name} user {skill.name}", 1f);
    }

    void TriggerDamageEffect(Unit character, int damage)
    {
        CreateText(character.transform.position, $"{character.name} took {damage} damage", 1.2f);
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
