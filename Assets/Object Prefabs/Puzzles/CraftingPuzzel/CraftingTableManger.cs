using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;


public class CraftingTableManager : MonoBehaviour
{
    public List<PuzzelItemSlot> listofpuzzelItems;    // List of puzzle items
    public List<ItemSlot> listofGridSlots;             // List of grid slots
    public List<PuzzelItems> correctPuzzelorder;       // Correct puzzle order
    public int currentSelectedItemPosition;            // Index of the selected item
    public int currentSelectedSlotPosition;            // Index of the selected slot
    public PuzzelItems selectedItem;                    // The selected item GameObject
    public int numofCorrect = 0;
    public TextMeshProUGUI numofCorrectText;
    public TextMeshProUGUI headerText;
    public FadeAnimation fadeAnimation;


    // Function to receive index of the selected item
    public void ReceiveIndexItems(int index)
    {
        currentSelectedItemPosition = index;
    }

    // Function to receive index of the selected slot
    public void ReceiveIndexSlots(int index)
    {
        currentSelectedSlotPosition = index;
    }
    void Start()
    {
        fadeAnimation.FadeIn();

        for (int i = 0; i < listofpuzzelItems.Count; i++)
        {
            listofpuzzelItems[i].Items.correctSpot = false;
        }
        numofCorrectText.text = numofCorrect.ToString();

    }
    void Update()
    {

        if (currentSelectedSlotPosition != 0 && currentSelectedItemPosition != 0)
        {
            for (int i = 0; i < listofpuzzelItems.Count; i++)
            {
                if (listofpuzzelItems[i].Items != null && listofpuzzelItems[i].Items.index == currentSelectedItemPosition)
                {

                    selectedItem = listofpuzzelItems[i].Items;
                    // Change the parent sprite color before adding the item
                    if (AddItem())
                    {
                        listofpuzzelItems[i].updateUI();
                        listofpuzzelItems[i].Items = null;

                        currentSelectedSlotPosition = 0;
                        currentSelectedItemPosition = 0;
                        break;
                    }

                }
            }

            checkMatching();
        }
    }



    // Adds item to the correct slot in the grid
    public bool AddItem()
    {
        for (int i = 0; i < listofGridSlots.Count; i++)
        {
            if (listofGridSlots[i].index == currentSelectedSlotPosition)
            {
                // Check if the slot is empty
                if (listofGridSlots[i].Items == null)
                {
                    listofGridSlots[i].Items = selectedItem; // Assign selected item to the slot
                    listofGridSlots[i].updateUI();  // Update the UI for the grid slot
                    return true;
                }
                else
                {
                    // Optionally, give feedback that the slot is already occupied
                    Debug.LogWarning("Slot is already occupied!");
                }
            }
        }
        return false;
    }

    // Checks if the puzzle has been solved correctly
    public void checkMatching()
    {
        for (int i = 0; i < correctPuzzelorder.Count; i++)
        {
            if (correctPuzzelorder[i] != null)
            {
                if (correctPuzzelorder[i] == listofGridSlots[i].Items && correctPuzzelorder[i].correctSpot == false)
                {
                    correctPuzzelorder[i].correctSpot = true;
                    numofCorrect++;  // Increment correct pieces count
                    Debug.Log("You got one right!");
                    headerText.text = "You got them all right!";

                    numofCorrectText.text = numofCorrect.ToString();
                    // Exit the loop after finding a match
                    break;
                }
            }

        }
        // Check if all pieces are correctly placed
        if (numofCorrect == 4)
        {
            Debug.Log("Congrats, you got them all right!");
            headerText.text = "You got them all right!";
            SwitchScenes();
        }
    }
    public void SwitchScenes()
    {
        SceneManager.LoadScene(0); // Loads the scene at index 0 in Build Settings
    }



}