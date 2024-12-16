using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverPuzzle : MonoBehaviour
{
    [System.Serializable]
    public class Lever
    {
        public GameObject leverObject;
        public bool isOn = false;
        [HideInInspector] public Collider2D proximityCollider;
        public SpriteRenderer leverSprite; // Added to change visual state
    }

    public Lever[] levers;
    public bool[] correctCombination;
    private Lever currentLever = null;

    //public GameObject interactionText; // Reference to UI text or 3D text

    private void Start()
    {
        foreach (Lever lever in levers)
        {
            if (lever.leverObject != null)
            {
                lever.proximityCollider = lever.leverObject.GetComponent<Collider2D>();
                if (lever.proximityCollider == null)
                {
                    Debug.LogError($"Lever object '{lever.leverObject.name}' does not have a Collider. Add one to use proximity detection.");
                }
                else if (!lever.proximityCollider.isTrigger)
                {
                    lever.proximityCollider.isTrigger = true;
                }

                lever.leverSprite = lever.leverObject.GetComponent<SpriteRenderer>();
                if (lever.leverSprite == null)
                {
                    Debug.LogError($"Lever object '{lever.leverObject.name}' does not have a SpriteRenderer.");
                }
            }
        }

        //interactionText.SetActive(false); // Initially hide interaction text
    }

    private void Update()
    {
        if (currentLever != null)
        {
            // Show interaction text
            //interactionText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleLever(currentLever);
            }
        }
        else
        {
            // Hide interaction text when not near any lever
           // interactionText.SetActive(false);
        }
    }

    private void ToggleLever(Lever lever)
    {
        lever.isOn = !lever.isOn;

        // Change lever visual (e.g., change color or rotate)
        lever.leverSprite.color = lever.isOn ? Color.green : Color.red;

        CheckCombination();
    }

    private void CheckCombination()
    {
        for (int i = 0; i < levers.Length; i++)
        {
            if (levers[i].isOn != correctCombination[i])
            {
                Debug.Log("Incorrect combination.");
                return;
            }
        }

        OnPuzzleSolved();
    }

    private void OnPuzzleSolved()
    {
        Debug.Log("Puzzle solved!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach (Lever lever in levers)
        {
            if (other == lever.proximityCollider)
            {
                currentLever = lever;
                return;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        foreach (Lever lever in levers)
        {
            if (other == lever.proximityCollider && lever == currentLever)
            {
                currentLever = null;
                return;
            }
        }
    }
}
