using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzelItemSlot : MonoBehaviour
{
    public PuzzelItems Items;
    public CraftingTableManager NewItemslot;
    public GameObject targetObject; // The object with the Image component
    private UnityEngine.UI.Image imageComponent;
    Color lightGray = Color.gray * 1.5f; // Brightens the default gray

    void Start()
    {
        imageComponent = targetObject.GetComponent<UnityEngine.UI.Image>();
        if (Items != null)
        {
            imageComponent.sprite = Items.ItemSprite;
        }
    }

    public void SendIndex()
    {
        if (Items != null)
        {
            // Change the parent sprite color (SpriteRenderer on parent)
            ChangeParentVisuals(lightGray);
            // Pass the index to the other script
            NewItemslot.ReceiveIndexItems(Items.index);
        }
    }

    public void ChangeParentVisuals(Color newColor)
    {
        // Get the parent Transform
        Transform parentTransform = this.transform.parent;
        UnityEngine.UI.Image parentImageComponent = parentTransform.GetComponent<UnityEngine.UI.Image>();

        // Change the color of the parent's Image component
        parentImageComponent.color = newColor;
        return;

    }
    public void updateUI()
    {
        if (imageComponent != null)
        {
            imageComponent.sprite = null; // Removes the image sprite
            imageComponent.enabled = false; // Hides the Image component
            ChangeParentVisuals(Color.white);

        }
    }
}