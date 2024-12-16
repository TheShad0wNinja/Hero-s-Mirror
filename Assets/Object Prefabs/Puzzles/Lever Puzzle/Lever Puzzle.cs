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
    }

    public Lever[] levers;
    public bool[] correctCombination;
    private Lever currentLever = null;

    private void Start()
{
    foreach (Lever lever in levers)
    {
        if (lever.leverObject != null)
        {
            lever.proximityCollider = lever.leverObject.GetComponent<BoxCollider2D>();
            if (lever.proximityCollider == null)
            {
                Debug.LogError($"Lever object '{lever.leverObject.name}' does not have a Collider2D. Add one to use proximity detection.");
                continue;
            }
            else if (!lever.proximityCollider.isTrigger)
            {
                lever.proximityCollider.isTrigger = true;
            }

            // Attach the LeverInteraction component
            LeverInteraction interaction = lever.leverObject.AddComponent<LeverInteraction>();
            if (interaction != null)
            {
                interaction.Setup(this, lever);
                Debug.Log($"LeverInteraction set up for lever '{lever.leverObject.name}'.");
            }
            else
            {
                Debug.LogError($"Failed to attach LeverInteraction to lever '{lever.leverObject.name}'.");
            }
        }
        else
        {
            Debug.LogError("Lever object is null in the levers array. Ensure all levers are assigned.");
        }
    }
}


    public void SetCurrentLever(Lever lever)
    {
        Debug.Log(lever);
        currentLever = lever;
    }

    public void ClearCurrentLever(Lever lever)
    {
        if (currentLever == lever)
        {
            currentLever = null;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentLever != null)
        {
            ToggleLever(currentLever);
        }
    }

    private void ToggleLever(Lever lever)
    {
        lever.isOn = !lever.isOn;
        Animator animator = lever.leverObject.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("SwitchOn", lever.isOn);
        }
        else
        {
            Debug.LogError($"Lever object '{lever.leverObject.name}' does not have an Animator component.");
        }
        Debug.Log($"Lever '{lever.leverObject.name}' is now {(lever.isOn ? "On" : "Off")}.");

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
        FindObjectOfType<PuzzleDoor>().UnlockDoorPuzzle();
    }
}