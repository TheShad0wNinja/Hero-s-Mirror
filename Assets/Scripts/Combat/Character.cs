using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

public abstract class Character : MonoBehaviour
{

    [Header("Character Information")]
    [SerializeField] CharacterSO characterData;
    [SerializeField] CombatEvent combatEvent;
    [SerializeField] public List<SkillSO> skills;
    [SerializeField] public List<PassiveSO> passives;
    [SerializeField] List<StatusEffect> activeEffects;

    public string CharacterName => characterData.characterName;
    public bool IsEnemy => characterData.isEnemy;

    public int Shield { get ; set; }

    SpriteRenderer sr;
    Animator anim;
    public int currentHealth;
    public int currentMana;
    public float attackbonus = 1;
    public float critChance = 0.1f;
    public bool hasTurn = true;
    public bool isDead = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        sr.sprite = characterData.sprite;

        if (characterData.flipped)
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

        currentHealth = characterData.baseHealth;
        currentMana = characterData.baseMana;
        Shield = characterData.baseShield;
        attackbonus = characterData.baseAttackBonus;
        critChance = characterData.baseCritChance;

        SetupEvents();
    }

    void SetupEvents()
    {
        combatEvent.OnNewTurn.AddListener(TriggerEffects);

        foreach(var passive in passives)
        {
            passive.SubscribeToEvent(null);
        }
    }

    public void TriggerEffects(CombatManager m)
    {
        foreach (var effect in activeEffects.ToList())
        {
            ActionQueueManager.Instance.EnqueueAction(this, effect, StatusEffectAction.EffectAction.TICK);

            if (effect.IsExpired)
                ActionQueueManager.Instance.EnqueueAction(this, effect, StatusEffectAction.EffectAction.REMOVE);
        }
    }

    public void AddStatusEffect(StatusEffect effect)
    {
        activeEffects.Add(effect);
    }

    public void RemoveStatusEffect(StatusEffect effect)
    {
        activeEffects.Remove(effect);
    }

    public virtual void TakeRawDamage(Character attacker, int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Kill(attacker);
    }

    private void Kill(Character attacker)
    {
        ActionQueueManager.Instance.EnqueueAction(attacker, this);
    }

    /// <summary>
    /// Calculates the damage that will affect the character and how much shield is left after if any
    /// </summary>
    /// <param name="damage"></param>
    /// <returns>(The amount of damage, the amount of shield left)</returns>
    public (int actualDamage, int shieldLeft) CalculateDamage(int damage)
    {
        if (Shield > 0)
        {
            if (Shield < damage)
            {
                damage -= Shield;
                Shield = 0;
            }
            else
            {
                Shield -= damage;
                damage = 0;
            }
        }
        return (damage, Shield);
    }

    public IEnumerator AnimateAction(SkillSO skill)
    {
        transform.rotation = Quaternion.Euler(0, 0, 16);
        yield return new WaitForSeconds(0.5f);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        yield return null;
    }

    public IEnumerator AnimateAction(bool hit)
    {
        if (hit)
        {
            transform.rotation = Quaternion.Euler(0, 0, -16);
            yield return new WaitForSeconds(0.5f);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        } else
             transform.rotation = Quaternion.Euler(0, 0, 16);
            yield return new WaitForSeconds(0.5f);
            transform.rotation = Quaternion.Euler(0, 0, 0);       {
        }
        yield return null;
    }
}