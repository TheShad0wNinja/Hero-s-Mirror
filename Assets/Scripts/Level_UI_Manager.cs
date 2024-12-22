using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Level_UI_Manager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textGold;
    [SerializeField] GameObject characterInfoPrefab;
    [SerializeField] GameObject characterInfoList;

    UI_Behaviour_Manager inventoryManager;
    Dictionary<Character, CharacterOverlayController> characterOverlays = new();

    void Start()
    {
        inventoryManager = UI_Behaviour_Manager.Instance;
        SetValues();
        UpdateUI();
    }

    public void UpdateUI()
    {
        foreach(var i in characterOverlays)
        {
            i.Value.healthBar.SetBarValue(i.Key.currentHealth, i.Key.currentStats["health"]);
        }
        textGold.text = inventoryManager.gold.ToString();
    }

    public void DisableUI()
    {
        foreach(var i in characterOverlays)
            i.Value.gameObject.SetActive(false);

        textGold.gameObject.SetActive(false);
    }

    void SetValues() 
    {
        foreach(var ch in inventoryManager.teamAssembleCharacters)
        {
            var newPanel = Instantiate(characterInfoPrefab, characterInfoList.transform);
            var co = newPanel.GetComponent<CharacterOverlayController>(); 
            co.portrait.sprite = ch.portrait;
            if (ch.stats.flipped)
                co.portrait.transform.localScale = new (-1, 1);
            co.characterName.text = ch.name;
            characterOverlays.Add(ch, co);
        }
    }
}
