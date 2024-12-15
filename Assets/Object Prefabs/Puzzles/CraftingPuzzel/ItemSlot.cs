using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Needed for Image component

public class ItemSlot : MonoBehaviour
{
    public int index;                   // Index for the slot
    public CraftingTableManager itemSelected;   // Reference to CraftingTableManager
    private Image oldObjectImage;       // The Image component of the old object
    public PuzzelItems Items;           // Reference to the puzzle items (this should have access to the image component)

    public void SendIndex()
    {
        // Pass the index to the other script (CraftingTableManager)
        itemSelected.ReceiveIndexSlots(index);
    }

    public void updateUI()
    {
        // Get the Image component of the current GameObject (this GameObject)
        oldObjectImage = GetComponent<Image>();  

        if (Items != null && Items.ItemSprite != null)  // Ensure the Items object and its sprite exist
        {
            if (oldObjectImage != null)
            {
                // Assign the sprite from the puzzle item (Items) to the Image component of this GameObject
                oldObjectImage.sprite = Items.ItemSprite;
            }
          
        }
        else
        {
            Debug.LogError("Items or its ItemSprite is missing!");
        }
    }
}