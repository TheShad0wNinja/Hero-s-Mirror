using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Manager : MonoBehaviour
{
    private UI_Behaviour_Manager inventoryManager;
    public List<Potion_Stats> allPotionsStats = new List<Potion_Stats>();
    public List<Potion> allPotions = new List<Potion>();
    private List<GameObject> inventoryPotionSlots = new List<GameObject>();

    private GameObject lastSelectedSlot;
    [SerializeField] private GameObject shopSlotParent;
    [SerializeField] private GameObject inventorySlotParent;
    [SerializeField] private Sprite nullImage;
    [SerializeField] private GameObject noGoldPopupPanel;
    [SerializeField] private Image potionIlustration;
    [SerializeField] private TextMeshProUGUI potionName;
    [SerializeField] private TextMeshProUGUI potionDescription;
    [SerializeField] private TextMeshProUGUI potionStats;
    [SerializeField] private TextMeshProUGUI potionPrice;
    [SerializeField] private TextMeshProUGUI goldTextUI;

    private Dictionary<GameObject, Potion> shopData = new Dictionary<GameObject, Potion>();
    private Dictionary<GameObject, potionQuantity> inventoryData = new Dictionary<GameObject, potionQuantity>();

    void Start()
    {
        inventoryManager = UI_Behaviour_Manager.Instance;
        CreatePotions();
        LoadItems();
    }

    void Update() { }

    void LoadItems()
    {
        FillInventoryPotionSlots();
        FillShopPotionSlots();
    }

    void CreatePotions()
    {
        foreach (var element in allPotionsStats)
        {
            allPotions.Add(new Potion(element));
        }
    }

    void FillShopPotionSlots()
    {
        goldTextUI.text = inventoryManager.gold.ToString();

        List<GameObject> shopPotionSlots = new List<GameObject>();

        foreach (Transform child in shopSlotParent.transform)
        {
            shopPotionSlots.Add(child.gameObject);
        }

        for (int i = 0; i < shopPotionSlots.Count; i++)
        {
            shopPotionSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = nullImage;
        }

        for (int i = 0; i < allPotions.Count; i++)
        {
            shopData.Add(shopPotionSlots[i], allPotions[i]);
            shopPotionSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = allPotions[i].image;
        }

        FillInventoryPotionSlots();
    }

    void FillInventoryPotionSlots()
    {
        inventoryPotionSlots.Clear();
        int j = 0;
        inventoryData.Clear();
        Dictionary<Potion, int> ownedPotions = inventoryManager.ownedPotions;

        foreach (Transform child in inventorySlotParent.transform)
        {
            inventoryPotionSlots.Add(child.gameObject);
        }

        foreach (var slot in inventoryPotionSlots)
        {
            slot.transform.GetChild(0).GetComponent<Image>().sprite = nullImage;
            slot.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = string.Empty;
        }

        foreach (var element in ownedPotions)
        {
            print(inventoryPotionSlots[j].name);
            inventoryData.Add(inventoryPotionSlots[j], new potionQuantity(element.Key, element.Value));
            inventoryPotionSlots[j].transform.GetChild(0).GetComponent<Image>().sprite = element.Key.image;
            inventoryPotionSlots[j].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = element.Value.ToString();
            j++;
        }
    }

    public void SelectItemUI(GameObject slot)
    {
        if (shopData.ContainsKey(slot))
        {
            ShowDescription(slot);
            slot.GetComponent<Image>().color = Color.cyan;

            if (lastSelectedSlot != null)
            {
                lastSelectedSlot.GetComponent<Image>().color = Color.white;
            }

            lastSelectedSlot = slot;
        }
    }

    void ShowDescription(GameObject slot)
    {
        Potion temp = shopData[slot];
        potionIlustration.sprite = temp.image;
        potionName.text = temp.name;
        potionDescription.text = temp.description;
        potionPrice.text = $"- {temp.price}";
        potionStats.text = string.Empty;

        int count = 0;
        foreach (var stat in temp.currentStatsFiltered)
        {
            potionStats.text += $"{stat.Key}    :    {stat.Value}\t\t";
            if (count == 2)
            {
                potionStats.text += "\n";
            }
            count++;
        }
    }

    public void PurchasePotion()
    {
        if (shopData.ContainsKey(lastSelectedSlot) && lastSelectedSlot != null)
        {
            if (inventoryManager.gold >= shopData[lastSelectedSlot].price)
            {
                inventoryManager.gold -= shopData[lastSelectedSlot].price;
                goldTextUI.text = inventoryManager.gold.ToString();
                inventoryManager.AddPotion(shopData[lastSelectedSlot]);
                FillInventoryPotionSlots();
            }
            else
            {
                noGoldPopupPanel.SetActive(true);
            }
        }
    }

    class potionQuantity
    {
        public Potion potion { get; set; }
        public int quantity { get; set; }

        public potionQuantity(Potion potion, int quantity)
        {
            this.potion = potion;
            this.quantity = quantity;
        }
    }
}
