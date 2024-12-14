using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Training_Manager : MonoBehaviour
{
    [SerializeField] private GameObject characterSlot;
    [SerializeField] private GameObject contentContainer;
    private Dictionary<GameObject, Character> selectionBoxData = new Dictionary<GameObject, Character>();
    private GameObject lastSelectedSlot;

    [SerializeField] private Sprite nullImage;

    [Header("Character 1")]
    [SerializeField] private Image characterIllustration1;
    [SerializeField] private TextMeshProUGUI characterName1;
    [SerializeField] private TextMeshProUGUI characterStats1;
    [SerializeField] private TextMeshProUGUI characterRarity1;

    [Header("Character 2")]
    [SerializeField] private Image characterIllustration2;
    [SerializeField] private TextMeshProUGUI characterName2;
    [SerializeField] private TextMeshProUGUI characterStats2;
    [SerializeField] private TextMeshProUGUI characterRarity2;

    [Header("Pop-Up Panel")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Image popupCharacterIllustration;
    [SerializeField] private TextMeshProUGUI popupCharacterName;
    [SerializeField] private TextMeshProUGUI popupCharacterStats;
    [SerializeField] private TextMeshProUGUI popupCharacterLevel;

    private int count;
    private Character character1;
    private Character character2;
    private GameObject characterSlot1;
    private GameObject characterSlot2;
    private UI_Behaviour_Manager inventoryManager;

    private void Start()
    {
        inventoryManager = UI_Behaviour_Manager.Instance;
        NullifyUIData(0);
        LoadCharacters();
    }

    public void NullifyUIData(int index)
    {
        switch (index)
        {
            case 1:
                ResetCharacterUI(ref character1, ref characterSlot1, characterIllustration1, characterName1, characterRarity1, characterStats1);
                count = 0;
                break;

            case 2:
                ResetCharacterUI(ref character2, ref characterSlot2, characterIllustration2, characterName2, characterRarity2, characterStats2);
                count = Mathf.Max(0, count - 1);
                break;

            default:
                ResetCharacterUI(ref character1, ref characterSlot1, characterIllustration1, characterName1, characterRarity1, characterStats1);
                ResetCharacterUI(ref character2, ref characterSlot2, characterIllustration2, characterName2, characterRarity2, characterStats2);
                count = 0;
                break;
        }
    }

    private void ResetCharacterUI(ref Character character, ref GameObject characterSlot, Image illustration, TextMeshProUGUI name, TextMeshProUGUI rarity, TextMeshProUGUI stats)
    {
        character = null;
        illustration.sprite = nullImage;
        name.text = "";
        rarity.text = "";
        stats.text = "";

        if (characterSlot != null)
        {
            characterSlot.GetComponent<Image>().color = Color.white;
            characterSlot = null;
        }
    }

    public void SelectItemUI(GameObject slot)
    {
        if (slot == characterSlot1 || slot == characterSlot2) return;

        count = (count + 1) % 3;

        if (selectionBoxData.ContainsKey(slot))
        {
            ShowDescription(slot, count);
            lastSelectedSlot = slot;
        }
    }

    private void ShowDescription(GameObject slot, int index)
    {
        Character temp = selectionBoxData[slot];

        switch (index)
        {
            case 1:
                AssignCharacterData(temp, ref character1, ref characterSlot1, characterIllustration1, characterName1, characterRarity1, characterStats1, slot);
                break;

            case 2:
                AssignCharacterData(temp, ref character2, ref characterSlot2, characterIllustration2, characterName2, characterRarity2, characterStats2, slot);
                break;
        }
    }

    private void AssignCharacterData(Character character, ref Character targetCharacter, ref GameObject targetSlot, Image illustration, TextMeshProUGUI name, TextMeshProUGUI rarity, TextMeshProUGUI stats, GameObject slot)
    {
        targetCharacter = character;
        illustration.sprite = character.image;
        name.text = character.name;
        rarity.text = character.rarityName.ToString();
        stats.text = "";

        if (targetSlot != null)
            targetSlot.GetComponent<Image>().color = Color.white;

        targetSlot = slot;
        targetSlot.GetComponent<Image>().color = Color.grey;

        foreach (var stat in character.currentStats)
        {
            stats.text += $"{stat.Key}    :    {stat.Value}\n";
        }
    }

    private void LoadCharacters()
    {
        for (int i = 0; i < inventoryManager.ownedCharacters.Count; i++)
        {
            CreateCharacterSlot(i);
        }
    }

    private void CreateCharacterSlot(int index)
    {
        GameObject slot = Instantiate(characterSlot, transform.position, Quaternion.identity);
        slot.transform.SetParent(contentContainer.transform);
        slot.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = inventoryManager.ownedCharacters[index].image;
        selectionBoxData.Add(slot, inventoryManager.ownedCharacters[index]);
    }

    public void Merge()
    {
        if (character1 != null && character2 != null)
        {
            float rarityDifference = character1.rarityTier <= character2.rarityTier
                ? character2.rarityTier - character1.rarityTier
                : 1 / (character1.rarityTier - character2.rarityTier);

            inventoryManager.ownedCharacters[inventoryManager.ownedCharacters.IndexOf(character1)]
                .mergeXP((int)(100 * character2.Level * character2.rarityTier));

            inventoryManager.ownedCharacters.Remove(character2);

            foreach (var element in selectionBoxData)
            {
                if (element.Value == character2)
                {
                    selectionBoxData.Remove(element.Key);
                    Destroy(element.Key);
                    break;
                }
            }

            OpenPopup(character1);
            NullifyUIData(3);
        }
    }

    private void OpenPopup(Character upgradedCharacter)
    {
        popupPanel.SetActive(true);
        popupCharacterName.text = upgradedCharacter.name;
        popupCharacterIllustration.sprite = upgradedCharacter.image;
        popupCharacterLevel.text = $"Level    :    {upgradedCharacter.Level.ToString()}";
        popupCharacterStats.text = "";

        foreach (var stat in upgradedCharacter.currentStats)
        {
            popupCharacterStats.text += $"{stat.Key}    :    {stat.Value}\n";
        }
    }
}
