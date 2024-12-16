using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectManager : MonoBehaviour
{
    public List<StatusEffectImage> statusEffectImages;
    [Serializable]
    public class StatusEffectImage
    {
        public string effectType;
        public Sprite image;
    }

    public GameObject statusEffectList, statusEffectPrefab;
    List<GameObject> currenStatusEffect = new();

    public void UpdateStatusEffects(List<StatusEffect> effects)
    {
        RemoveStatusEffectList();
        foreach (var effect in effects)
        {
            Sprite effectSprite = statusEffectImages.Find(i => i.effectType == effect.name)?.image;
            var obj = Instantiate(statusEffectPrefab, statusEffectList.transform);
            obj.GetComponent<Image>().sprite = effectSprite;
            currenStatusEffect.Add(obj);
        }
    }

    public void RemoveStatusEffectList()
    {
        currenStatusEffect.ForEach(o => Destroy(o));
        currenStatusEffect.Clear();
    }

}