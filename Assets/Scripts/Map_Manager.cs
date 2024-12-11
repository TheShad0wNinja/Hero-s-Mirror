using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Map_Manager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject characterSlot;
    [SerializeField] private GameObject contentContainer;
    [SerializeField] private Sprite nullImage;
    [SerializeField] private GameObject popupPanel;

    [Header("Character Slots")]
    [SerializeField] private Image characterIllustration1;
    [SerializeField] private TextMeshProUGUI characterName1;
    [SerializeField] private TextMeshProUGUI characterLevel1;
    [SerializeField] private TextMeshProUGUI characterRarity1;

    [SerializeField] private Image characterIllustration2;
    [SerializeField] private TextMeshProUGUI characterName2;
    [SerializeField] private TextMeshProUGUI characterLevel2;
    [SerializeField] private TextMeshProUGUI characterRarity2;

    [SerializeField] private Image characterIllustration3;
    [SerializeField] private TextMeshProUGUI characterName3;
    [SerializeField] private TextMeshProUGUI characterLevel3;
    [SerializeField] private TextMeshProUGUI characterRarity3;

    private Dictionary<GameObject, Character> selectionBoxData = new();
    private UI_Behaviour_Manager inventoryManager;
    private GameObject lastSelectedSlot;

    private Character character1, character2, character3;
    private GameObject characterSlot1, characterSlot2, characterSlot3;
    List<GameObject> characterSlots = new List<GameObject>();

   int count = 0;
    private void Start()
    {
        inventoryManager = UI_Behaviour_Manager.Instance;
        AddSlots();
        ResetUI();
        LoadCharacters();
    }
    void AddSlots() 
    {
        characterSlots.Add(characterSlot1);
        characterSlots.Add(characterSlot2);
        characterSlots.Add(characterSlot3);
    }
    int SlotIndex() 
    {
        if (characterSlot1 == null) 
        {
            count = 0;
            return 1;
        }
        else if (characterSlot2 == null)
        {
            count = 0;
            return 2;
        }
        else if (characterSlot3 == null)
        {
            count = 0;
            return 3;
        }
        if (count == 2) count = 0;
        count++;
        return count;
    }
    public void ResetUI(int slotIndex = 0)
    {
        switch (slotIndex)
        {
            case 1:
                ClearSlot(ref character1, ref characterSlot1, characterIllustration1, characterName1, characterLevel1, characterRarity1);
                break;
            case 2:
                ClearSlot(ref character2, ref characterSlot2, characterIllustration2, characterName2, characterLevel2, characterRarity2);
                break;
            case 3:
                ClearSlot(ref character3, ref characterSlot3, characterIllustration3, characterName3, characterLevel3, characterRarity3);
                break;
            default:
                ClearAllSlots();
                break;
        }
        

        lastSelectedSlot = null;
    }

    private void ClearSlot(ref Character character, ref GameObject characterSlot, Image illustration, TextMeshProUGUI name, TextMeshProUGUI level, TextMeshProUGUI rarity)
    {
        character = null;
        if (characterSlot != null) characterSlot.GetComponent<Image>().color = Color.white;
        characterSlot = null;
        illustration.sprite = nullImage;
        name.text = "";
        level.text = "";
        rarity.text = "";
    }

    private void ClearAllSlots()
    {
        ClearSlot(ref character1, ref characterSlot1, characterIllustration1, characterName1, characterLevel1, characterRarity1);
        ClearSlot(ref character2, ref characterSlot2, characterIllustration2, characterName2, characterLevel2, characterRarity2);
        ClearSlot(ref character3, ref characterSlot3, characterIllustration3, characterName3, characterLevel3, characterRarity3);
    }

    public void SelectItemUI(GameObject slot)
    {
        if (slot == characterSlot1 || slot == characterSlot2 || slot == characterSlot3) return;

        if (selectionBoxData.ContainsKey(slot))
        {
            ShowDescription(slot, SlotIndex());
            lastSelectedSlot = slot;
        }
    }

    private void ShowDescription(GameObject slot, int slotIndex)
    {
        Character selectedCharacter = selectionBoxData[slot];

        switch (slotIndex)
        {
            case 1:
                SetCharacterSlot(ref character1, ref characterSlot1, characterIllustration1, characterName1, characterLevel1, characterRarity1, slot, selectedCharacter);
                break;
            case 2:
                SetCharacterSlot(ref character2, ref characterSlot2, characterIllustration2, characterName2, characterLevel2, characterRarity2, slot, selectedCharacter);
                break;
            case 3:
                SetCharacterSlot(ref character3, ref characterSlot3, characterIllustration3, characterName3, characterLevel3, characterRarity3, slot, selectedCharacter);
                break;
        }
    }

    private void SetCharacterSlot
        (
        ref Character character,
        ref GameObject characterSlot,
        Image illustration,
        TextMeshProUGUI name,
        TextMeshProUGUI level,
        TextMeshProUGUI rarity,
        GameObject slot,
        Character selectedCharacter
        )
    {
        if (character != null) characterSlot.GetComponent<Image>().color = Color.white;
        character = selectedCharacter;
        characterSlot = slot;
        characterSlot.GetComponent<Image>().color = Color.grey;
        illustration.sprite = selectedCharacter.image;
        name.text = selectedCharacter.name;
        level.text = selectedCharacter.Level.ToString();
        rarity.text = selectedCharacter.rarityName.ToString();
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
        GameObject newSlot = Instantiate(characterSlot, contentContainer.transform);
        newSlot.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = inventoryManager.ownedCharacters[index].image;
        selectionBoxData[newSlot] = inventoryManager.ownedCharacters[index];
    }

    public void Venture()
    {
        if (character1 != null && character2 != null && character3 != null)
        {
            inventoryManager.AddTeamCharacters(new List<Character> { character1, character2, character3 });
            // load Level Scene
        }
        else
        {
            OpenPopUp();
        }
    }

    private void OpenPopUp()
    {
        popupPanel.SetActive(true);
    }
}
