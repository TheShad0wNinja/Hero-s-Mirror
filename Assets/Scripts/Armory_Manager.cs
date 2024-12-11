using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmoryManager : MonoBehaviour
{
    [SerializeField] private Image itemIllustration;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemStats;

    [SerializeField] private GameObject helmetSlot;
    [SerializeField] private GameObject weaponSlot;
    [SerializeField] private GameObject amuletSlot;
    [SerializeField] private GameObject armorSlot;
    [SerializeField] private GameObject bootSlot;

    [SerializeField] private Button equipButton;
    [SerializeField] private Button unequipButton;

    private GameObject lastSelectedSlot;
    private Character currentCharacter;

    [SerializeField] private GameObject inventoryItemGridPanel;
    [SerializeField] private Sprite nullImage;

    public List<Item> ownedItems = new List<Item>();
    public List<Item> unequippedItems = new List<Item>();
    public List<Item> equippedItems = new List<Item>();

    private List<GameObject> itemSlots = new List<GameObject>();
    private Dictionary<GameObject, Item> slotToItem = new Dictionary<GameObject, Item>();
    private Dictionary<GameObject, EquipmentItemType> equipmentSlots = new Dictionary<GameObject, EquipmentItemType>();

    public int currentCharacterIndex;
    [SerializeField] private Image characterIllustration;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI characterStats;
    [SerializeField] private TextMeshProUGUI characterLevel;
    [SerializeField] private TextMeshProUGUI characterRarity;
    private UI_Behaviour_Manager inventoryManager;

    void Start()
    {
        inventoryManager = UI_Behaviour_Manager.Instance;
        if (inventoryManager == null)
        {
            Debug.LogError("UI_Behaviour_Manager is not in scene.");
            return;
        }
        InitializeEquipmentSlots();
        FillItemSlots();
        SetCharacter(inventoryManager.ownedCharacters[currentCharacterIndex]);
        RefreshItemSlots();
        LoadCharacterEquippedItems();
        UpdateCharacterStats();
    }

    public void EquipItem()
    {
        if (lastSelectedSlot == null || !slotToItem.ContainsKey(lastSelectedSlot)) return;

        Item selectedItem = slotToItem[lastSelectedSlot];
        GameObject equipmentSlot = FindEquipmentSlotByType(selectedItem.type);

        if (equipmentSlot != null)
        {
            UnEquipItem(equipmentSlot);
            EquipItemToSlot(equipmentSlot, selectedItem);
            currentCharacter.AddEquippedItem(selectedItem);
            RefreshItemSlots();
            UpdateCharacterStats();
        }
    }

    public void UnEquipItem()
    {
        if (lastSelectedSlot == null || !equipmentSlots.ContainsKey(lastSelectedSlot)) return;

        Item equippedItem = equipmentSlots[lastSelectedSlot].item;
        if (equippedItem != null)
        {
            equippedItems.Remove(equipmentSlots[lastSelectedSlot].item);
            equipmentSlots[lastSelectedSlot].item = null;
            lastSelectedSlot.transform.GetChild(0).GetComponent<Image>().sprite = nullImage;
            currentCharacter.RemoveEquippedItem(equippedItem);

            RefreshItemSlots();
            UpdateCharacterStats();
        }
    }
    public void UnEquipItem(GameObject equipmentSlot)
    {
        Item equippedItem = equipmentSlots[equipmentSlot].item;
        if (equippedItem != null)
        {
            equippedItems.Remove(equipmentSlots[equipmentSlot].item);
            equipmentSlots[equipmentSlot].item = null;
            currentCharacter.RemoveEquippedItem(equippedItem);
        }
    }

    public void RefreshItemSlots()
    {
        FillItems();
        slotToItem.Clear();

        for (int i = 0; i < itemSlots.Count; i++)
        {
            Image slotImage = itemSlots[i].transform.GetChild(0).GetComponent<Image>();
            slotImage.sprite = nullImage;

            if (i < unequippedItems.Count)
            {
                slotToItem[itemSlots[i]] = unequippedItems[i];
                slotImage.sprite = unequippedItems[i].image;
            }
        }
    }

    public void LoadCharacterEquippedItems()
    {
        foreach (var slot in equipmentSlots)
        {
            EquipmentItemType equipmentSlot = slot.Value;
            Item equippedItem = currentCharacter.GetEquippedItem(equipmentSlot.type);

            if (equippedItem != null)
            {
                slot.Key.transform.GetChild(0).GetComponent<Image>().sprite = equippedItem.image;
                equipmentSlot.item = equippedItem;
            }
            else
            {
                slot.Key.transform.GetChild(0).GetComponent<Image>().sprite = nullImage;
                equipmentSlot.item = null;
            }
        }
    }

    public void SelectItemUI(GameObject slot)
    {
        if (!equipmentSlots.ContainsKey(slot) && !slotToItem.ContainsKey(slot)) return;

        if (lastSelectedSlot != null)
        {
            lastSelectedSlot.GetComponent<Image>().color = Color.white;
        }

        slot.GetComponent<Image>().color = Color.cyan;
        lastSelectedSlot = slot;

        equipButton.interactable = slotToItem.ContainsKey(slot);
        print($"{equipButton.interactable} equipe button");
        unequipButton.interactable = equipmentSlots.ContainsKey(slot) && equipmentSlots[slot].item != null;

        ShowItemDetails(slot);
    }

    public void SwitchCharacter(bool next)
    {
        currentCharacterIndex = next
            ? (currentCharacterIndex + 1) % inventoryManager.ownedCharacters.Count
            : (currentCharacterIndex - 1 + inventoryManager.ownedCharacters.Count) % inventoryManager.ownedCharacters.Count;

        SetCharacter(inventoryManager.ownedCharacters[currentCharacterIndex]);
        LoadCharacterEquippedItems();
        UpdateCharacterStats();
        RefreshItemSlots();
    }

    private void ShowItemDetails(GameObject slot)
    {
        Item item = slotToItem.ContainsKey(slot) ? slotToItem[slot] : equipmentSlots[slot].item;

        if (item != null)
        {
            itemIllustration.sprite = item.image;
            itemName.text = item.name;
            itemDescription.text = item.description;

            itemStats.text = "";
            foreach (var stat in item.currentStatsFiltered)
            {
                itemStats.text += $"{stat.Key}: {stat.Value}\n";
            }
        }
    }

    private void EquipItemToSlot(GameObject slot, Item item)
    {
        slot.transform.GetChild(0).GetComponent<Image>().sprite = item.image;
        equipmentSlots[slot].item = item;
        equippedItems.Add(item);
        unequippedItems.Remove(item);
    }

    private GameObject FindEquipmentSlotByType(string type)
    {
        foreach (var slot in equipmentSlots)
        {
            if (slot.Value.type == type) return slot.Key;
        }
        return null;
    }

    private void InitializeEquipmentSlots()
    {
        equipmentSlots[helmetSlot] = new EquipmentItemType(null, "Helmet");
        equipmentSlots[armorSlot] = new EquipmentItemType(null, "Armor");
        equipmentSlots[amuletSlot] = new EquipmentItemType(null, "Amulet");
        equipmentSlots[bootSlot] = new EquipmentItemType(null, "Boot");
        equipmentSlots[weaponSlot] = new EquipmentItemType(null, "Weapon");
    }

    private void FillItemSlots()
    {
        foreach (Transform child in inventoryItemGridPanel.transform)
        {
            itemSlots.Add(child.gameObject);
        }
    }
    private void FillItems()
    {
        unequippedItems.Clear();
        foreach (Item child in inventoryManager.ownedItems)
        {
            if(!equippedItems.Contains(child)) unequippedItems.Add(child);
        }
    }

    private void SetCharacter(Character character)
    {
        currentCharacter = character;
        characterIllustration.sprite = character.image;
    }

    private void UpdateCharacterStats()
    {
        characterName.text = currentCharacter.name;
        characterLevel.text = $"Level :     { currentCharacter.Level.ToString()}";
        characterRarity.text = currentCharacter.rarityName;
        characterStats.text = "";
        int counter = 0;
        foreach (var stat in currentCharacter.currentStats)
        {
            characterStats.text += $"{stat.Key} :     {stat.Value}\t\t";
            counter++;
            if (counter == 2) { counter = 0; characterStats.text += "\n"; };
        }
    }
}

class EquipmentItemType
{
    public Item item;
    public string type;

    public EquipmentItemType(Item item, string type)
    {
        this.item = item;
        this.type = type;
    }
}